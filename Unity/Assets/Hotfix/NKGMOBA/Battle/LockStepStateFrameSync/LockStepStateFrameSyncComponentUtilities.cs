using System;
using System.Collections.Generic;
using ET.EventType;


namespace ET
{
    public static class LockStepStateFrameSyncComponentUtilities
    {
        /// <summary>
        /// 正式的帧同步Tick，所有的战斗逻辑都从这里出发
        /// </summary>
        /// <param name="self"></param>
        public static void LSF_Tick(this LockStepStateFrameSyncComponent self)
        {
            Log.Info($"------------帧同步Tick Time Point： {TimeHelper.ClientNow()}");
            self.CurrentFrame++;

            if (self.FrameCmdsToHandle.TryGetValue(self.CurrentFrame, out var currentFrameCmdToHandle))
            {
                foreach (var cmd in currentFrameCmdToHandle)
                {
                    //TODO 处理客户端/服务端cmd
                }
            }

            //TODO Tick Unit及其相关模块
        }

        public static void AddMessageToHandle(this LockStepStateFrameSyncComponent self, Object messageToHandle)
        {
#if SERVER
            C2M_FrameCmd cmd = messageToHandle as C2M_FrameCmd;
#else
            M2C_FrameCmd cmd = messageToHandle as M2C_FrameCmd;
#endif

            if (self.FrameCmdsToHandle.TryGetValue(self.CurrentFrame, out var queue))
            {
                queue.Enqueue(cmd);
            }
            else
            {
                Queue<Object> newQueue = new Queue<Object>();
                newQueue.Enqueue(cmd);
                self.FrameCmdsToHandle[self.CurrentFrame] = newQueue;
            }
        }

        /// <summary>
        /// 发送消息，所有的帧同步消息都通过这个接口发送
        /// </summary>
        /// <param name="self"></param>
        /// <param name="messageToSend"></param>
        /// <typeparam name="T"></typeparam>
        public static void SendMessage<T>(this LockStepStateFrameSyncComponent self, T messageToSend)
            where T : ALSF_Cmd
        {
#if SERVER
            M2C_FrameCmd m2CFrameCmd = new M2C_FrameCmd()
                {CmdContent = messageToSend, Frame = self.CurrentFrame};
            
            MessageHelper.BroadcastToRoom(self.GetParent<Room>(), m2CFrameCmd);
#else

            C2M_FrameCmd c2MFrameCmd = new C2M_FrameCmd()
                {CmdContent = messageToSend, Frame = self.CurrentFrame};

            Game.Scene.GetComponent<PlayerComponent>().GateSession.Send(c2MFrameCmd);

            //将消息放入玩家输入缓冲区，用于预测回滚
            if (self.FrameCmdsBuffer.TryGetValue(self.CurrentFrame, out var queue))
            {
                queue.Enqueue(c2MFrameCmd);
            }
            else
            {
                Queue<Object> newQueue = new Queue<Object>();
                newQueue.Enqueue(c2MFrameCmd);
                self.FrameCmdsToHandle[self.CurrentFrame] = newQueue;
            }
#endif
        }

        public static void StartFrameSync(this LockStepStateFrameSyncComponent self)
        {
            self.StartSync = true;
            self.FixedUpdate = new FixedUpdate() {UpdateCallback = self.LSF_Tick};
        }

#if !SERVER
        /// <summary>
        /// 根据消息包中服务端帧数 + 半个RTT来计算出服务端当前帧数
        /// </summary>
        public static void CaculateServerCurrentFrameByCmdFrameAndHalfRTT(this LockStepStateFrameSyncComponent self,
            uint messageFrame)
        {
            self.ServerCurrentFrame = messageFrame + (uint) (self.HalfRTT / GlobalDefine.FixedUpdateTargetDTTime_Long);
        }
#endif
    }
}