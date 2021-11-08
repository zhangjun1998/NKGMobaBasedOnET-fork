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
        /// 正式的帧同步Tick，所有的战斗逻辑都从这里出发
        /// </summary>
        /// <param name="chaseFrame">是否处于追帧状态</param>
        public static void LSF_Tick(this LSF_Component self)
        {
            //Log.Info($"------------帧同步Tick Time Point： {TimeHelper.ClientNow()} Frame : {self.CurrentFrame}");

            if (self.FrameCmdsToHandle.TryGetValue(self.CurrentFrame, out var currentFrameCmdToHandle))
            {
                foreach (var cmd in currentFrameCmdToHandle)
                {
                    // 处理客户端/服务端cmd
                    //Log.Info($"------------处理第{self.CurrentFrame}帧指令");
                    LSF_CmdDispatcherComponent.Instance.Handle(self.GetParent<Room>(), cmd);
                }
            }

            self.FrameCmdsToHandle.Remove(self.CurrentFrame);

#if !SERVER
            if (!self.ShouldTickInternal)
            {
                return;
            }

            Queue<ALSF_Cmd> lastValidCmds = null;

            if (self.IsInChaseFrameState)
            {
                lastValidCmds  = self.GetLastValidCmd();
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

            self.CurrentFrame++;
        }

        /// <summary>
        /// 注意这里的帧数是消息中的帧数
        /// 特殊的，对于服务器来说，哪一帧收到客户端指令就会当成客户端在哪一帧的输入(累加一个缓冲帧时长)
        /// </summary>
        /// <param name="self"></param>
        /// <param name="cmdToHandle"></param>
        public static void AddCmdToHandle(this LSF_Component self, ALSF_Cmd cmdToHandle)
        {
#if SERVER
            uint correntFrame = self.CurrentFrame;
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
        /// 发送消息，所有的帧同步消息都通过这个接口发送
        /// </summary>
        /// <param name="self"></param>
        /// <param name="messageToSend"></param>
        /// <typeparam name="T"></typeparam>
        public static void SendMessage<T>(this LSF_Component self, T cmdToSend)
            where T : ALSF_Cmd
        {
            cmdToSend.Frame = self.CurrentFrame;

#if SERVER
            M2C_FrameCmd m2CFrameCmd = new M2C_FrameCmd() {CmdContent = cmdToSend};

            MessageHelper.BroadcastToRoom(self.GetParent<Room>(), m2CFrameCmd);

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
#else
            C2M_FrameCmd c2MFrameCmd = new C2M_FrameCmd() {CmdContent = cmdToSend};

            Game.Scene.GetComponent<PlayerComponent>().GateSession.Send(c2MFrameCmd);

            //将消息放入玩家输入缓冲区，用于预测回滚
            if (self.PlayerInputCmdsBuffer.TryGetValue(self.CurrentFrame, out var queue))
            {
                queue.Enqueue(c2MFrameCmd.CmdContent);
            }
            else
            {
                Queue<ALSF_Cmd> newQueue = new Queue<ALSF_Cmd>();
                newQueue.Enqueue(cmdToSend);
                self.PlayerInputCmdsBuffer[self.CurrentFrame] = newQueue;
            }
#endif
        }

        public static void StartFrameSync(this LSF_Component self)
        {
            self.StartSync = true;
            self.FixedUpdate = new FixedUpdate() {UpdateCallback = self.LSF_Tick};
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
            while (!self.PlayerInputCmdsBuffer.ContainsKey(frame) && frame >= 1)
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