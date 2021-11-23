namespace ET
{
    [ActorMessageHandler]
    //RoomManager通知RoomScene关闭房间
    public class RM2RA_RemoveRoomHandler : AMActorHandler<Scene, RM2RA_RemoveRoom>
    {
        protected override async ETTask Run(Scene scene, RM2RA_RemoveRoom request)
        {
            await ETTask.CompletedTask;
            //通知房间关闭.有额外处理内容所以需要内网actormessage
            MessageHelper.BroadcastToRoom(scene, new Room2G_RoomClose() { CloseCode = request.CloseCode });
            scene.Dispose();
        }
    }
}