namespace ET
{
    [ActorMessageHandler]
    //Room向RoomManager发送信息更新请求
    public class RA2RM_UpdateRoomInfoHandler : AMActorHandler<Scene, RA2RM_UpdateRoomInfo>
    {
        protected override async ETTask Run(Scene scene, RA2RM_UpdateRoomInfo message)
        {
            await ETTask.CompletedTask;
            var roommanager = scene.GetComponent<RoomManagerComponent>();
            int needCloseRoom = 0;
            if (roommanager.Rooms.TryGetValue(message.RoomInfo.RoomId, out var roomInfo))
            {
                if (roomInfo.IsGameing && message.RoomInfo.IsGameing == false)
                {
                    needCloseRoom = 1;
                }
                //判断房主退出.战斗结束关闭房间
                else if (roomInfo.RoomHolderPlayer != 0 && message.RoomInfo.RoomHolderPlayer == 0)
                {
                    needCloseRoom = 2;
                }
            }
            if (needCloseRoom != 0)
            {
                roommanager.Rooms.Remove(message.RoomInfo.RoomId);
                MessageHelper.SendActor(message.RoomInfo.RoomId, new RM2RA_RemoveRoom() { CloseCode = needCloseRoom });
                return;
            }
            roommanager.Rooms[message.RoomInfo.RoomId] = message.RoomInfo;

        }
    }
}