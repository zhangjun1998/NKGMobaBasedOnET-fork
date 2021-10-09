namespace ET
{
    public class M2C_SyncCDDataHandler : AMHandler<M2C_SyncCDData>
    {
        protected override async ETVoid Run(Session session, M2C_SyncCDData message)
        {
            session.DomainScene().GetComponent<CDComponent>().SetCD(message.UnitId, message.CDName, message.CDLength,
                message.RemainCDLength);
            await ETTask.CompletedTask;
        }
    }
}