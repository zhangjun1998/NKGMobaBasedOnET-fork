
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.RoomManager)]
    public class UnRegisterRoomToRoomManagerHandler : AMHandler<UnRegisterRoomToRoomManager>
    {
        protected override async ETTask Run(Session session, UnRegisterRoomToRoomManager message)
        {
            Game.Scene.GetComponent<RoomManagerEntity>().AllRoomDic.Remove(message.Roomid);
            await ETTask.CompletedTask;
        }
    }
}
