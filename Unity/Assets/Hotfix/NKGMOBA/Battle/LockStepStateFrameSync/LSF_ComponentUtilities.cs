using System;
using System.Collections.Generic;
using ET.EventType;
using UnityEngine;


namespace ET
{
    public static class LSF_ComponentUtilities
    {
        /// <summary>
        /// 正式的帧同步Tick，所有的战斗逻辑都从这里出发
        /// </summary>
        /// <param name="self"></param>
        public static void LSF_Tick(this LSF_Component self)
        {
            Log.Info($"------------帧同步Tick Time Point： {TimeHelper.ClientNow()} Frame : {self.CurrentFrame}");

#if !SERVER
            if (!self.ShouldTickInternal)
            {
                return;
            }

            if (self.FrameCmdsBuffer.TryGetValue(self.CurrentFrame, out var inputCmdQueue))
            {
                foreach (var cmd in inputCmdQueue)
                {
                    //处理用户输入缓冲区中的指令，用于预测
                    Log.Info($"------------处理用户输入缓冲区第{self.CurrentFrame}帧指令");
                    LSF_CmdHandlerComponent.Instance.Handle(self.GetParent<Room>(), cmd);
                }
            }
#endif

#if SERVER
            // 测试代码
            self.GetParent<Room>().GetComponent<LSF_Component>()
                .SendMessage(ReferencePool.Acquire<LSF_MoveCmd>().Init(0));
#else
            // 测试代码
            Unit unit = self.GetParent<Room>().GetComponent<UnitComponent>().MyUnit;
            unit.BelongToRoom.GetComponent<LSF_Component>()
                .SendMessage(ReferencePool.Acquire<LSF_PathFindCmd>().Init(unit.Id));
#endif

            if (self.FrameCmdsToHandle.TryGetValue(self.CurrentFrame, out var currentFrameCmdToHandle))
            {
                foreach (var cmd in currentFrameCmdToHandle)
                {
                    //TODO 处理客户端/服务端cmd
                    Log.Info($"------------处理第{self.CurrentFrame}帧指令");
                    LSF_CmdHandlerComponent.Instance.Handle(self.GetParent<Room>(), cmd);
                }
            }

            //TODO Tick Unit及其相关模块

            self.CurrentFrame++;
        }

        /// <summary>
        /// 注意这里的帧数是自己当前帧，不论服务器还是客户端
        /// 对于服务器来说，哪一帧收到客户端指令就会当成客户端在哪一帧的输入
        /// 对于客户端来说，收到服务端指令会先将自己的当前帧设置为服务器回包的帧，然后进行追帧操作
        /// </summary>
        /// <param name="self"></param>
        /// <param name="cmdToHandle"></param>
        public static void AddCmdToHandle(this LSF_Component self, ALSF_Cmd cmdToHandle)
        {
            if (self.FrameCmdsToHandle.TryGetValue(self.CurrentFrame, out var queue))
            {
                queue.Enqueue(cmdToHandle);
            }
            else
            {
                Queue<ALSF_Cmd> newQueue = new Queue<ALSF_Cmd>();
                newQueue.Enqueue(cmdToHandle);
                self.FrameCmdsToHandle[self.CurrentFrame] = newQueue;
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
#else
            C2M_FrameCmd c2MFrameCmd = new C2M_FrameCmd() {CmdContent = cmdToSend};

            Game.Scene.GetComponent<PlayerComponent>().GateSession.Send(c2MFrameCmd);

            //将消息放入玩家输入缓冲区，用于预测回滚
            if (self.FrameCmdsBuffer.TryGetValue(self.CurrentFrame, out var queue))
            {
                queue.Enqueue(c2MFrameCmd.CmdContent);
            }
            else
            {
                Queue<ALSF_Cmd> newQueue = new Queue<ALSF_Cmd>();
                newQueue.Enqueue(cmdToSend);
                self.FrameCmdsBuffer[self.CurrentFrame] = newQueue;
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
        public static void RefreshClientNetInfoByCmdFrameAndHalfRTT(this LSF_Component self,
            uint messageFrame)
        {
            self.CurrentArrivedFrame = self.CurrentFrame;

            //TODO 进行模拟，然后对比结果，如果不一致则进行追帧，如果一致则不做处理，继续预测
            if (!self.SimulateSpecialFrame(messageFrame))
            {
                self.CurrentFrame = messageFrame;
            }

            self.ServerCurrentFrame = messageFrame +
                                      (uint) ((self.HalfRTT +
                                               (long) (Time.deltaTime * 1000)) /
                                              GlobalDefine.FixedUpdateTargetDTTime_Long);

            //将这一帧用户输入指令从本地缓冲区移除，因为服务端已经发送这一帧的指令下来了，缓冲区里的这一帧已经没用了
            self.FrameCmdsBuffer.Remove(messageFrame);
        }

        /// <summary>
        /// 对特定帧的数据输入指令进行模拟，并得出结果
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        public static bool SimulateSpecialFrame(this LSF_Component self, uint frame)
        {
            return true;
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
            // 2.非正常情况，客户端由于网络延迟或者断开导致没有收到服务端的帧指令，导致ServerCurrentFrame长时间没有更新，会导致CurrentAheadOfFrame越来越大，当达到一个阈值的时候将会进行断线重连
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
            else // 当前客户端帧数小于服务端帧数，三种情况，1.刚开局，2.玩家退游戏重连，3.玩家收到服务器回包，并且模拟之后发现结果并不一致，重置自己的currentFrame，进行追帧
            {
                self.CurrentAheadOfFrame = -(int) (self.ServerCurrentFrame - self.CurrentFrame);

                Log.Error("收到服务器回包后发现模拟的结果与服务器不一致，即需要强行回滚，则回滚，然后开始追帧");
                int count = self.TargetAheadOfFrame;
                while (count-- > 0)
                {
                    self.LSF_Tick();
                }
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