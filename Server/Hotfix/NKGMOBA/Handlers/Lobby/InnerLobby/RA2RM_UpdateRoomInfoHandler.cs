namespace ET
{
    [ActorMessageHandler]
    //Room向RoomManager发送信息更新请求
    //收到信息后.会根据信息的状态变更来决定是否移除房间
    public class RA2RM_UpdateRoomInfoHandler : AMActorHandler<Scene, RA2RM_UpdateRoomInfo>
    {
        protected override async ETTask Run(Scene scene, RA2RM_UpdateRoomInfo message)
        {
            await ETTask.CompletedTask;
            var roommanager = scene.GetComponent<RoomManagerComponent>();
            int needCloseRoom = 0;
            if (roommanager.Rooms.TryGetValue(message.RoomInfo.RoomId, out var roomInfo))
            {
                //战斗从战斗中变为未战斗.说明战斗结束了.关闭房间
                if (roomInfo.IsGameing && message.RoomInfo.IsGameing == false)
                {
                    needCloseRoom = 1;
                }
                //判断房主退出.关闭房间
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