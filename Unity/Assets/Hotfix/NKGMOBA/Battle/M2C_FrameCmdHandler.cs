namespace ET
{
    public class M2C_FrameCmdHandler: AMHandler<M2C_FrameCmd>
    {
        protected override async ETVoid Run(Session session, M2C_FrameCmd message)
        {
            LockStepStateFrameSyncComponent lockStepStateFrameSyncComponent = session.DomainScene()
                .GetComponent<RoomManagerComponent>().BattleRoom.GetComponent<LockStepStateFrameSyncComponent>();
            
            lockStepStateFrameSyncComponent.AddMessageToHandle(message);
            lockStepStateFrameSyncComponent.CaculateServerCurrentFrameByCmdFrameAndHalfRTT(message.Frame);

            await ETTask.CompletedTask;
        }
    }
}