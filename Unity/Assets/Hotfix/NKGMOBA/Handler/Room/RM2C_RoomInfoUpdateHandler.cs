using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class RM2C_RoomInfoUpdateHandler : AMHandler<RM2C_RoomInfoUpdate>
    {
        protected override async ETTask Run(ETModel.Session session, RM2C_RoomInfoUpdate message)
        {
            Log.Info("收到了进入房间更新指令");
            if (Game.Scene.GetComponent<FUIComponent>().Get(FUIPackage.FUIRoom_FUIRoom) != null)
            {
                Game.EventSystem.Run(EventIdType.UpdateRoom, message.Roominfo);
            }
            else {
                Game.EventSystem.Run(EventIdType.ShowRoom,message.Roominfo);
            }
            await ETTask.CompletedTask;
        }
    }
}