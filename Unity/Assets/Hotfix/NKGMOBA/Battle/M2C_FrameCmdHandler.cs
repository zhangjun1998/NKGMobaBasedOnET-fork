namespace ET
{
    public class M2C_FrameCmdHandler: AMHandler<M2C_FrameCmd>
    {
        protected override async ETVoid Run(Session session, M2C_FrameCmd message)
        {
            LSF_Component lsfComponent = session.DomainScene()
                .GetComponent<RoomManagerComponent>().BattleRoom.GetComponent<LSF_Component>();
            
            lsfComponent.AddCmdToHandle(message.CmdContent);
            lsfComponent.CaculateServerCurrentFrameByCmdFrameAndHalfRTT(message.CmdContent.Frame);

            await ETTask.CompletedTask;
        }
    }
}