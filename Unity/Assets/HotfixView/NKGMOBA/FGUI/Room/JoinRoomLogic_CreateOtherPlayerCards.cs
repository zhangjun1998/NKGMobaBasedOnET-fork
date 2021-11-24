namespace ET
{
    public class JoinRoomLogic_CreateOtherPlayerCards : AEvent<EventType.JoinRoom>
    {
        protected override async ETTask Run(EventType.JoinRoom a)
        {
            FUIManagerComponent fuiManagerComponent = a.DomainScene.GetComponent<FUIManagerComponent>();

            FUI_RoomComponent fuiRoomComponent =
                fuiManagerComponent.GetFUIComponent<FUI_RoomComponent>(FUI_RoomComponent.FUIRoomName);

            foreach (var playerInfoRoom in a.PlayerInfoRooms)
            {
                FUI_RoomUtilities.AddPlayerCard(fuiRoomComponent, playerInfoRoom.playerid, playerInfoRoom.Name,
                    playerInfoRoom.camp);
            }
            fuiRoomComponent.FuiRoomList.Visible = false;
            fuiRoomComponent.FuiRoom.Visible = true;
            await ETTask.CompletedTask;
        }
    }
}