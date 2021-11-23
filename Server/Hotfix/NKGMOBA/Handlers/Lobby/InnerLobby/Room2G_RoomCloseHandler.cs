namespace ET
{
    [ActorMessageHandler]
    //RoomScene通知GateSession关闭房间
    public class Room2G_RoomCloseHandler : AMActorHandler<Session, Room2G_RoomClose>
    {
        protected override async ETTask Run(Session session, Room2G_RoomClose request)
        {
            await ETTask.CompletedTask;
            session.RemoveComponent<RoomStateOnGateComponent>();
            session.Send(new L2C_RoomClose() { CloseCode = request.CloseCode });
        }
    }
}