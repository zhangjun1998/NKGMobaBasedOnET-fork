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
                lsfComponent.IsInChaseFrameState = true;
                
                //将消息加入待处理列表
                lsfComponent.AddCmdToHandle(message.CmdContent);
                
                lsfComponent.CurrentFrame = message.CmdContent.Frame;
                
                //回滚处理
                lsfComponent.RollBack(message.CmdContent.Frame, message.CmdContent);

                //因为这一帧已经重置过数据，所以从下一帧开始追帧
                lsfComponent.CurrentFrame++;

                //Log.Error("收到服务器回包后发现模拟的结果与服务器不一致，即需要强行回滚，则回滚，然后开始追帧");
                uint count = lsfComponent.CurrentArrivedFrame - message.CmdContent.Frame - 1;
                while (count-- > 0)
                {
                    lsfComponent.LSF_Tick();
                }
                
                lsfComponent.IsInChaseFrameState = false; 
            }
            
            await ETTask.CompletedTask;
        }
    }
}