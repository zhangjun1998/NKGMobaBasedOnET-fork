//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月5日 22:07:15
//------------------------------------------------------------

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
    [MessageHandler]
    public class RM2C_LeaveRoomHandler : AMHandler<RM2C_LeaveRoom>
    {
        protected override async ETTask Run(ETModel.Session session, RM2C_LeaveRoom message)
        {
            Log.Info("收到了退出房间更新指令");

            Game.EventSystem.Run(EventIdType.QuitRoomToRoomList);
            switch (message.LeaveReason)
            {
                case 1:
                    Game.EventSystem.Run(EventIdType.ShowMsgDialog, "你被房主无情的踢了");
                    break;
                case 2:
                    Game.EventSystem.Run(EventIdType.ShowMsgDialog, "房间解散了");
                    break;
                default:
                    Game.EventSystem.Run(EventIdType.ShowMsgDialog, "从房间退出");
                    break;
            }
            await ETTask.CompletedTask;
        }
    }
    [MessageHandler]
    public class RM2C_EnterBattleMessageHandler : AMHandler<RM2C_EnterBattleMessage>
    {
        protected override async ETTask Run(ETModel.Session session, RM2C_EnterBattleMessage message)
        {
            Log.Info("收到了进入战斗指令");
            ETModel.Game.EventSystem.Run(ETModel.EventIdType.ShowLoadingUI);
            MapHelper.EnterMapAsync().Coroutine();
            await ETTask.CompletedTask;
        }
    }
}