
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.RoomManager)]
    public class UpdateRoomToRoomManagerManagerHandler : AMHandler<UpdateRoomToRoomManager>
    {
        protected override async ETTask Run(Session session, UpdateRoomToRoomManager message)
        {
            Game.Scene.GetComponent<RoomManagerEntity>().AllRoomDic[message.BriefInfo.RoomId]=message.BriefInfo;
            await ETTask.CompletedTask;
        }
    }
}
