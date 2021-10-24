using System;
using System.Collections.Generic;
using ET.EventType;


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
            self.CurrentFrame++;
            Log.Info($"------------帧同步Tick Time Point： {TimeHelper.ClientNow()} Frame : {self.CurrentFrame}");
#if !SERVER

            // 测试代码
            Unit unit = self.GetParent<Room>().GetComponent<UnitComponent>().MyUnit;
            unit.BelongToRoom.GetComponent<LSF_Component>().SendMessage(ReferencePool.Acquire<LSF_PathFindCmd>().Init(unit.Id));
#endif
            
            if (self.FrameCmdsToHandle.TryGetValue(self.CurrentFrame, out var currentFrameCmdToHandle))
            {
                foreach (var cmd in currentFrameCmdToHandle)
                {
                    //TODO 处理客户端/服务端cmd
                    Log.Info("------------处理第{self.CurrentFrame}帧指令");
                    LSF_CmdHandlerComponent.Instance.Handle(self.GetParent<Room>(), cmd);
                }
            }

            //TODO Tick Unit及其相关模块
        }

        public static void AddCmdToHandle(this LSF_Component self, ALSF_Cmd cmdToHandle)
        {
            if (self.FrameCmdsToHandle.TryGetValue(cmdToHandle.Frame, out var queue))
            {
                queue.Enqueue(cmdToHandle);
            }
            else
            {
                Queue<ALSF_Cmd> newQueue = new Queue<ALSF_Cmd>();
                newQueue.Enqueue(cmdToHandle);
                self.FrameCmdsToHandle[cmdToHandle.Frame] = newQueue;
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

            C2M_FrameCmd c2MFrameCmd = new C2M_FrameCmd(){CmdContent = cmdToSend};

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
        /// 根据消息包中服务端帧数 + 半个RTT来计算出服务端当前帧数
        /// </summary>
        public static void CaculateServerCurrentFrameByCmdFrameAndHalfRTT(this LSF_Component self,
            uint messageFrame)
        {
            self.ServerCurrentFrame = messageFrame + (uint) (self.HalfRTT / GlobalDefine.FixedUpdateTargetDTTime_Long);
        }
#endif
    }
}