using System;
using System.Collections.Generic;
using System.Linq;
using Box2DSharp.Dynamics;
using ET.EventType;
using UnityEngine;

#if !SERVER
using UnityEngine.Profiling;
#endif


namespace ET
{
    public static class LSF_ComponentUtilities
    {
        /// <summary>
        /// 正常Tick（由FixedUpdate发起调用）
        /// 对于客户端来说，自带一致性检查和预测回滚操作
        /// </summary>
        private static void LSF_TickNormally(this LSF_Component self)
        {
#if SERVER 
            //Log.Info($"------------帧同步Tick Time Point： {TimeHelper.ClientNow()} Frame : {self.CurrentFrame}");
            if (self.FrameCmdsToHandle.TryGetValue(self.CurrentFrame, out var currentFrameCmdToHandle))
            {
                foreach (var cmd in currentFrameCmdToHandle)
                {
                    //Log.Info($"------------处理第{self.CurrentFrame}帧指令");
                    LSF_CmdDispatcherComponent.Instance.Handle(self.GetParent<Room>(), cmd);
                }
            }

            self.FrameCmdsToHandle.Remove(self.CurrentFrame);
            
#else
            if (!self.ShouldTickInternal)
            {
                return;
            }
            
            // 现根据服务端发回的指令进行一致性检测，如果需要的话就进行回滚
            bool shouldRollback = false;

            foreach (var frameCmdsQueuePair in self.FrameCmdsToHandle)
            {
                Queue<ALSF_Cmd> frameCmdsQueue = frameCmdsQueuePair.Value;
                uint targetFrame = frameCmdsQueuePair.Key;
                
                foreach (var frameCmd in frameCmdsQueue)
                {
                    //说明需要回滚
                    if (!self.CheckConsistencyCompareSpecialFrame(targetFrame, frameCmd))
                    {
                        shouldRollback = true;
                        break;
                    }
                }

                if (shouldRollback)
                {
                    self.IsInChaseFrameState = true;
                    self.CurrentFrame = targetFrame;

                    foreach (var frameCmd in frameCmdsQueue)
                    {
                        //回滚处理
                        self.RollBack(self.CurrentFrame, frameCmd);
                    }

                    //因为这一帧已经重置过数据，所以从下一帧开始追帧
                    self.CurrentFrame++;

                    //Log.Error("收到服务器回包后发现模拟的结果与服务器不一致，即需要强行回滚，则回滚，然后开始追帧");
                    uint count = self.CurrentArrivedFrame - self.CurrentFrame;
                    while (count-- > 0)
                    {
                        self.LSF_TickManually();
                        self.CurrentFrame++;
                    }

                    self.IsInChaseFrameState = false;
                }

                // 清空这一帧的玩家输入缓冲区，因为这一帧我们已经确保本地与服务器状态一致了
                self.PlayerInputCmdsBuffer.Remove(targetFrame);
            }
            
            // 客户端每帧Tick完需要清空待处理列表，因为我们已经全量执行了服务端发来的指令（检测一致性，回滚），这样才能确保与服务端结果一致
            self.FrameCmdsToHandle.Clear();
#endif
            
            // 执行本帧本应该执行的的Tick
            self.LSF_TickManually();
            
            // 发送本帧收集的指令
            self.SendCurrentFrameMessage();
            
            self.CurrentFrame++;
        }

        /// <summary>
        /// 正式的帧同步Tick，所有的战斗逻辑都从这里出发，会自增CurrentFrame
        /// </summary>
        /// <param name="chaseFrame">是否处于追帧状态</param>
        private static void LSF_TickManually(this LSF_Component self)
        {
#if !SERVER
            Queue<ALSF_Cmd> lastValidCmds = null;

            if (self.IsInChaseFrameState)
            {
                lastValidCmds = self.GetLastValidCmd();
            }
            else
            {
                self.PlayerInputCmdsBuffer.TryGetValue(self.CurrentFrame, out lastValidCmds);
            }

            if (lastValidCmds != null)
            {
                foreach (var cmd in lastValidCmds)
                {
                    //处理用户输入缓冲区中的指令，用于预测
                    //Log.Info($"------------处理用户输入缓冲区第{self.CurrentFrame}帧指令");
                    LSF_CmdDispatcherComponent.Instance.Handle(self.GetParent<Room>(), cmd);
                }
            }
#endif

            // LSFTick Room，tick room的相关组件, 然后由Room去Tick其子组件，即此处是战斗的Tick起点
            self.GetParent<Room>().GetComponent<LSF_TickComponent>()
                ?.Tick(GlobalDefine.FixedUpdateTargetDTTime_Long);

#if !SERVER
            //执行预测逻辑
            self.GetParent<Room>().GetComponent<LSF_TickComponent>()
                ?.Predict(GlobalDefine.FixedUpdateTargetDTTime_Long);
#endif
        }

        /// <summary>
        /// 发送本帧收集的指令，所有的帧同步消息都通过这个接口发送
        /// </summary>
        /// <param name="self"></param>
        /// <param name="messageToSend"></param>
        /// <typeparam name="T"></typeparam>
        private static void SendCurrentFrameMessage(this LSF_Component self)
        {
            if (self.FrameCmdsToSend.TryGetValue(self.CurrentFrame, out var cmdQueueToSend))
            {
                foreach (var cmdToSend in cmdQueueToSend)
                {
#if SERVER
                    M2C_FrameCmd m2CFrameCmd = new M2C_FrameCmd() {CmdContent = cmdToSend};
                    MessageHelper.BroadcastToRoom(self.GetParent<Room>(), m2CFrameCmd);
#else
                    C2M_FrameCmd c2MFrameCmd = new C2M_FrameCmd() {CmdContent = cmdToSend};
                    Game.Scene.GetComponent<PlayerComponent>().GateSession.Send(c2MFrameCmd);
#endif
                }
            }

            //因为我们KCP确保消息可靠性，所以可以直接移除
            self.FrameCmdsToSend.Remove(self.CurrentFrame);
        }
        
        /// <summary>
        /// 注意这里的帧数是消息中的帧数
        /// 特殊的，对于服务器来说，哪一帧收到客户端指令就会当成客户端在哪一帧的输入(累加一个缓冲帧时长)
        /// </summary>
        /// <param name="self"></param>
        /// <param name="cmdToHandle"></param>
        public static void AddCmdToHandleQueue(this LSF_Component self, ALSF_Cmd cmdToHandle)
        {
#if SERVER
            uint correntFrame = self.CurrentFrame + self.BufferFrame;
#else
            uint correntFrame = cmdToHandle.Frame;
#endif
            if (self.FrameCmdsToHandle.TryGetValue(correntFrame, out var queue))
            {
                queue.Enqueue(cmdToHandle);
            }
            else
            {
                Queue<ALSF_Cmd> newQueue = new Queue<ALSF_Cmd>();
                newQueue.Enqueue(cmdToHandle);

                self.FrameCmdsToHandle[correntFrame] = newQueue;
            }
        }

        /// <summary>
        /// 将指令加入待发送列表，将在本帧末尾进行发送
        /// </summary>
        /// <param name="self"></param>
        /// <param name="cmdToSend"></param>
        public static void AddCmdToSendQueue<T>(this LSF_Component self, T cmdToSend) where T : ALSF_Cmd
        {
#if SERVER
            cmdToSend.Frame = self.CurrentFrame;
            M2C_FrameCmd m2CFrameCmd = new M2C_FrameCmd() {CmdContent = cmdToSend};

            //将指令放入整局游戏的缓冲区，用于录像和观战系统
            if (self.WholeCmds.TryGetValue(self.CurrentFrame, out var queue))
            {
                queue.Enqueue(m2CFrameCmd.CmdContent);
            }
            else
            {
                Queue<ALSF_Cmd> newQueue = new Queue<ALSF_Cmd>();
                newQueue.Enqueue(cmdToSend);
                self.WholeCmds[self.CurrentFrame] = newQueue;
            }
            
            //将消息放入待发送列表，本帧末尾进行发送
            if (self.FrameCmdsToSend.TryGetValue(self.CurrentFrame, out var queue2))
            {
                queue2.Enqueue(m2CFrameCmd.CmdContent);
            }
            else
            {
                Queue<ALSF_Cmd> newQueue = new Queue<ALSF_Cmd>();
                newQueue.Enqueue(cmdToSend);
                self.FrameCmdsToSend[self.CurrentFrame] = newQueue;
            }
#else
            C2M_FrameCmd c2MFrameCmd = new C2M_FrameCmd() {CmdContent = cmdToSend};

            //将消息放入玩家输入缓冲区，用于预测回滚
            if (self.PlayerInputCmdsBuffer.TryGetValue(self.CurrentFrame, out var queue1))
            {
                queue1.Enqueue(c2MFrameCmd.CmdContent);
            }
            else
            {
                Queue<ALSF_Cmd> newQueue = new Queue<ALSF_Cmd>();
                newQueue.Enqueue(cmdToSend);
                self.PlayerInputCmdsBuffer[self.CurrentFrame] = newQueue;
            }

            //将消息放入待发送列表，本帧末尾进行发送
            if (self.FrameCmdsToSend.TryGetValue(self.CurrentFrame, out var queue2))
            {
                queue2.Enqueue(c2MFrameCmd.CmdContent);
            }
            else
            {
                Queue<ALSF_Cmd> newQueue = new Queue<ALSF_Cmd>();
                newQueue.Enqueue(cmdToSend);
                self.FrameCmdsToSend[self.CurrentFrame] = newQueue;
            }
#endif
        }

        public static void StartFrameSync(this LSF_Component self)
        {
            self.StartSync = true;
            self.FixedUpdate = new FixedUpdate() {UpdateCallback = self.LSF_TickNormally};
        }

#if !SERVER
        /// <summary>
        /// 根据消息包中服务端帧数 + 半个RTT来计算出服务端当前帧数并且对一些字段和数据进行处理
        /// </summary>
        public static void RefreshClientNetInfoByCmdFrameAndHalfRTT(this LSF_Component self, uint messageFrame)
        {
            self.ServerCurrentFrame = messageFrame +
                                      (uint) TimeAndFrameConverter.Frame_Float2FrameWithHalfRTT(Time.deltaTime,
                                          self.HalfRTT);

            self.CurrentArrivedFrame = self.CurrentFrame;

            self.CurrentAheadOfFrame = (int) (self.CurrentFrame - self.ServerCurrentFrame);
        }

        /// <summary>
        /// 检测指定帧的数据一致性，并得出结果
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        public static bool CheckConsistencyCompareSpecialFrame(this LSF_Component self, uint frame, ALSF_Cmd alsfCmd)
        {
            return self.GetParent<Room>().GetComponent<LSF_TickComponent>()
                .CheckConsistency(frame, alsfCmd);
        }

        /// <summary>
        /// 回滚
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        public static bool RollBack(this LSF_Component self, uint frame, ALSF_Cmd alsfCmd)
        {
            return self.GetParent<Room>().GetComponent<LSF_TickComponent>()
                .RollBack(frame, alsfCmd);
        }

        /// <summary>
        /// 获取最近的有效输入
        /// </summary>
        /// <returns></returns>
        public static Queue<ALSF_Cmd> GetLastValidCmd(this LSF_Component self)
        {
            uint frame = self.CurrentFrame;
            while (!self.PlayerInputCmdsBuffer.ContainsKey(frame) && frame >= 1 && self.PlayerInputCmdsBuffer.Count > 0)
            {
                frame--;
            }

            if (self.PlayerInputCmdsBuffer.TryGetValue(frame, out var queue))
            {
                return queue;
            }

            return null;
        }

        /// <summary>
        /// 客户端处理异常的网络状况
        /// </summary>
        /// <returns></returns>
        public static async ETVoid ClientHandleExceptionNet(this LSF_Component self)
        {
            // 直到上一次异常状态处理完成之前都不会处理这一次异常
            if (!self.ShouldTickInternal)
            {
                return;
            }

            // 当前客户端帧数大于服务端帧数，两种情况，
            // 1.正常情况，客户端为了保证自己的消息在合适的时间点抵达服务端需要领先于服务器
            // 2.非正常情况，客户端由于网络延迟或者断开导致没有收到服务端的帧指令，导致ServerCurrentFrame长时间没有更新，再次收到服务端回包的时候发现是很久之前包了，也就会导致CurrentAheadOfFrame变大，当达到一个阈值的时候将会进行断线重连
            if (self.CurrentFrame > self.ServerCurrentFrame)
            {
                self.CurrentAheadOfFrame = (int) (self.CurrentFrame - self.ServerCurrentFrame);

                if (self.CurrentAheadOfFrame > LSF_Component.AheadOfFrameMax)
                {
                    self.ShouldTickInternal = false;

                    Log.Error("长时间未收到服务端回包，开始断线重连，停止模拟");
                    //TODO 开始断线重连，这里假设3s后重连完成
                    await TimerComponent.Instance.WaitAsync(3000);

                    self.ShouldTickInternal = true;

                    return;
                }
            }
            else // 当前客户端帧数小于服务端帧数，只开局的时候由于网络延迟问题导致服务端先行于客户端，加快Tick频率
            {
                self.CurrentAheadOfFrame = -(int) (self.ServerCurrentFrame - self.CurrentFrame);
            }

            // Log.Info(
            //     $"-------------------CurrentAheadOfFrame: {self.CurrentAheadOfFrame} TargetAheadOfFrame: {self.TargetAheadOfFrame} ServerCurrentFrame: {self.ServerCurrentFrame}");

            if (self.CurrentAheadOfFrame != self.TargetAheadOfFrame)
            {
                //Log.Info("------------------进入变速状态");
                self.HasInSpeedChangeState = true;
                self.FixedUpdate.TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond /
                                                                        (GlobalDefine.FixedUpdateTargetFPS +
                                                                         self.TargetAheadOfFrame -
                                                                         self.CurrentAheadOfFrame
                                                                        ));
            }
            else if (self.HasInSpeedChangeState)
            {
                //Log.Info("------------------已经对齐");
                self.HasInSpeedChangeState = false;
                self.FixedUpdate.TargetElapsedTime =
                    TimeSpan.FromTicks(TimeSpan.TicksPerSecond / (GlobalDefine.FixedUpdateTargetFPS));
            }
        }
#endif
    }
}