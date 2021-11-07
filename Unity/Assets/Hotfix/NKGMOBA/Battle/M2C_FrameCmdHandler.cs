namespace ET
{
    public class M2C_FrameCmdHandler: AMHandler<M2C_FrameCmd>
    {
        protected override async ETVoid Run(Session session, M2C_FrameCmd message)
        {
            LSF_Component lsfComponent = session.DomainScene()
                .GetComponent<RoomManagerComponent>().BattleRoom.GetComponent<LSF_Component>();
            
            lsfComponent.RefreshClientNetInfoByCmdFrameAndHalfRTT(message.CmdContent.Frame);

            //说明需要回滚
            if (!lsfComponent.CheckConsistencyCompareSpecialFrame(message.CmdContent.Frame, message.CmdContent))
            {
                //将消息加入待处理列表
                lsfComponent.AddCmdToHandle(message.CmdContent);
                
                lsfComponent.CurrentAheadOfFrame = (int) (lsfComponent.CurrentFrame - lsfComponent.ServerCurrentFrame);

                lsfComponent.CurrentFrame = message.CmdContent.Frame;
                
                //回顾处理
                lsfComponent.RollBack(message.CmdContent.Frame, message.CmdContent);

                //Log.Error("收到服务器回包后发现模拟的结果与服务器不一致，即需要强行回滚，则回滚，然后开始追帧");
                uint count = lsfComponent.CurrentArrivedFrame - message.CmdContent.Frame;
                while (count-- > 0)
                {
                    lsfComponent.LSF_Tick();
                }
            }
            
            await ETTask.CompletedTask;
        }
    }
}