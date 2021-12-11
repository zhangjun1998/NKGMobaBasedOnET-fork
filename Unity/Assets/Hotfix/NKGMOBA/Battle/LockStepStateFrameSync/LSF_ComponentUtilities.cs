using System;
using System.Collections.Generic;
using System.Diagnostics;
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
#if !SERVER
            Profiler.BeginSample("LockStepStateFrameSyncComponentUpdateSystem");
#else
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
#endif


#if !SERVER
            if (!self.ShouldTickInternal)
            {
                return;
            }
#endif
            self.CurrentFrame++;

#if !SERVER
            self.CurrentArrivedFrame = self.CurrentFrame;
#endif

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
            Unit playerUnit = self.GetParent<Room>().GetComponent<UnitComponent>().MyUnit;

            foreach (var frameCmdsQueuePair in self.FrameCmdsToHandle)
            {
                // 现根据服务端发回的指令进行一致性检测，如果需要的话就进行回滚
                bool shouldRollback = false;
                Queue<ALSF_Cmd> frameCmdsQueue = frameCmdsQueuePair.Value;
                uint targetFrame = frameCmdsQueuePair.Key;

                foreach (var frameCmd in frameCmdsQueue)
                {
                    //说明需要回滚
                    if (frameCmd.UnitId == playerUnit.Id &&
                        !self.CheckConsistencyCompareSpecialFrame(targetFrame, frameCmd))
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
                        // 自己的指令才回滚
                        if (frameCmd.UnitId == playerUnit.Id)
                        {
                            //回滚处理
                            self.RollBack(self.CurrentFrame, frameCmd);
                        }
                    }

                    //因为这一帧已经重置过数据，所以从下一帧开始追帧
                    self.CurrentFrame++;

                    //Log.Error("收到服务器回包后发现模拟的结果与服务器不一致，即需要强行回滚，则回滚，然后开始追帧");
                    // 注意这里追帧到当前已抵达帧的前一帧，因为最后有一步self.LSF_TickManually();用于当前帧Tick，不属于追帧的范围
                    int count = (int) self.CurrentArrivedFrame - 1 - (int) self.CurrentFrame;

                    while (count-- >= 0)
                    {
                        self.LSF_TickManually();
                        self.CurrentFrame++;
                    }

                    self.IsInChaseFrameState = false;
                }

                // 因为其他玩家严格根据服务端Tick指令做表现，所以要限制帧数
                if (frameCmdsQueuePair.Key <= self.ServerCurrentFrame)
                {
                    foreach (var frameCmd in frameCmdsQueue)
                    {
                        //其他玩家的指令直接执行
                        if (frameCmd.UnitId != playerUnit.Id)
                        {
                            LSF_CmdDispatcherComponent.Instance.Handle(self.GetParent<Room>(), frameCmd);
                        }
                    }
                }

                // TODO 不能直接清，因为类似寻路这种指令，并不是每帧都在发起，比如在70帧发起了一次寻路，这个寻路过程一直持续到90帧，在此期间的不同步都需要使用这个70帧发起的指令
                // TODO 清空这一帧的玩家输入缓冲区，因为这一帧我们已经确保本地与服务器状态一致了
                // self.PlayerInputCmdsBuffer.Remove(targetFrame);
                // Log.Info($"rrrrrrrrrrr 移除第{targetFrame}本地数据");
            }
            
            self.FrameCmdsToHandle.Clear();
#endif
            // 执行本帧本应该执行的的Tick
            self.LSF_TickManually();

            // 发送本帧收集的指令
            self.SendCurrentFrameMessage();

#if !SERVER
            Profiler.EndSample();
#else
            stopwatch.Stop();
            //Log.Info($"LockStepStateFrameSyncComponentUpdateSystem Cost: {stopwatch.ElapsedMilliseconds}");
#endif
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
                    //Log.Info($"------------第{self.CurrentFrame}帧处理用户输入缓冲区指令");
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
                    M2C_FrameCmd m2CFrameCmd = new M2C_FrameCmd() {CmdContent = cmdToSend, ServerTimeSnap =
 TimeHelper.ClientNow()};
                    MessageHelper.BroadcastToRoom(self.GetParent<Room>(), m2CFrameCmd);
                    Log.Info(TimeHelper.ClientNow().ToString());
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
            uint correntFrame = cmdToHandle.Frame;

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
        public static void AddCmdToSendQueue<T>(this LSF_Component self, T cmdToSend,
            bool shouldAddToPlayerInputBuffer = true) where T : ALSF_Cmd
        {
#if SERVER
            cmdToSend.Frame = self.CurrentFrame;
            self.AddCmdsToWholeCmdsBuffer(ref cmdToSend);
            
            M2C_FrameCmd m2CFrameCmd = new M2C_FrameCmd() {CmdContent = cmdToSend};

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

            //客户端用户输入有他的特殊性，往往会在Update里收集输入，在FixedUpdate里进行指令发送，所以要放到下一帧
            uint correctFrame = self.CurrentFrame + 1;

            cmdToSend.Frame = correctFrame;
            C2M_FrameCmd c2MFrameCmd = new C2M_FrameCmd() {CmdContent = cmdToSend};

            if (shouldAddToPlayerInputBuffer)
            {
                //将消息放入玩家输入缓冲区，用于预测回滚
                if (self.PlayerInputCmdsBuffer.TryGetValue(correctFrame, out var queue1))
                {
                    queue1.Enqueue(c2MFrameCmd.CmdContent);
                }
                else
                {
                    Queue<ALSF_Cmd> newQueue = new Queue<ALSF_Cmd>();
                    newQueue.Enqueue(cmdToSend);
                    self.PlayerInputCmdsBuffer[correctFrame] = newQueue;
                }
            }

            //将消息放入待发送列表，本帧末尾进行发送
            if (self.FrameCmdsToSend.TryGetValue(correctFrame, out var queue2))
            {
                queue2.Enqueue(c2MFrameCmd.CmdContent);
            }
            else
            {
                Queue<ALSF_Cmd> newQueue = new Queue<ALSF_Cmd>();
                newQueue.Enqueue(cmdToSend);
                self.FrameCmdsToSend[correctFrame] = newQueue;
            }
#endif
        }


        public static void AddCmdsToWholeCmdsBuffer<T>(this LSF_Component self, ref T cmdToSend) where T : ALSF_Cmd
        {
            cmdToSend.Frame = self.CurrentFrame;

            //将指令放入整局游戏的缓冲区，用于录像和观战系统
            if (self.WholeCmds.TryGetValue(self.CurrentFrame, out var queue))
            {
                queue.Enqueue(cmdToSend);
            }
            else
            {
                Queue<ALSF_Cmd> newQueue = new Queue<ALSF_Cmd>();
                newQueue.Enqueue(cmdToSend);
                self.WholeCmds[self.CurrentFrame] = newQueue;
            }
        }

        public static void StartFrameSync(this LSF_Component self)
        {
            self.StartSync = true;
            self.FixedUpdate = new FixedUpdate() {UpdateCallback = self.LSF_TickNormally};
        }

#if !SERVER
        public static void LSF_TickBattleView(this LSF_Component self, long deltaTime)
        {
            // LSFTick Room，tick room的相关组件, 然后由Room去Tick其子组件，即此处是战斗的Tick起点
            self.GetParent<Room>().GetComponent<LSF_TickComponent>()
                ?.TickView(deltaTime);
        }
        
        /// <summary>
        /// 根据消息包中服务端帧数 + 半个RTT来计算出服务端当前帧数并且对一些字段和数据进行处理
        /// </summary>
        public static void RefreshClientNetInfoByCmdFrameAndHalfRTT(this LSF_Component self, long serverTimeSnap,
            uint messageFrame)
        {
            self.ServerCurrentFrame = messageFrame +
                                      (uint) TimeAndFrameConverter.Frame_Long2FrameWithHalfRTT(
                                          TimeHelper.ClientNow() - serverTimeSnap,
                                          self.HalfRTT);
            self.CurrentAheadOfFrame = (int) (self.CurrentFrame - self.ServerCurrentFrame);

            Log.Info($"刷新服务端CurrentFrame成功：{self.ServerCurrentFrame} ---- {TimeHelper.ClientNow()}");
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
            while (frame >= 1 && self.PlayerInputCmdsBuffer.Count > 0 && !self.PlayerInputCmdsBuffer.ContainsKey(frame))
            {
                frame--;
            }

            if (self.PlayerInputCmdsBuffer.TryGetValue(frame, out var queue))
            {
                //Log.Info($"在第{self.CurrentFrame}帧从用户输入缓冲区获取第{frame}帧数据");
                return queue;
            }
            //Log.Info($"在第{self.CurrentFrame}帧从用户输入缓冲区获取第{frame}帧数据, 失败");

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
            else // 当前客户端帧数小于服务端帧数，是因为开局的时候由于网络延迟问题导致服务端先行于客户端，直接多次tick
            {
                self.CurrentAheadOfFrame = -(int) (self.ServerCurrentFrame - self.CurrentFrame);

                // 落后，追帧，追到目标帧
                int count = self.TargetAheadOfFrame - self.CurrentAheadOfFrame;

                while (--count >= 0)
                {
                    self.CurrentFrame++;
                    self.LSF_TickManually();
                }

                self.CurrentAheadOfFrame = self.TargetAheadOfFrame;
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