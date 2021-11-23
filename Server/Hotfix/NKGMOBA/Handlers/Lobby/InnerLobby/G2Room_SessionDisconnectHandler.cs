namespace ET
{
    [ActorMessageHandler]
    //RoomManager通知RoomScene关闭房间
    public class G2Room_SessionDisconnectHandler : AMActorHandler<Player, G2Room_SessionDisconnect>
    {
        protected override async ETTask Run(Player player, G2Room_SessionDisconnect request)
        {
            await ETTask.CompletedTask;
            player.GateSessionId = 0;
            //todo: ai接管 通知其他玩家该玩家掉线
        }
    }
}