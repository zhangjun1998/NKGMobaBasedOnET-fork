using System.Collections.Generic;
using ET.EventType;

namespace ET
{
    public class LeaveRoomFinish_DestroyAllPlayerCards : AEvent<EventType.LeaveRoom>
    {
        protected override async ETTask Run(LeaveRoom a)
        {
            FUI_RoomComponent fuiRoomComponent =
                a.DomainScene.GetComponent<FUIManagerComponent>().GetFUIComponent<FUI_RoomComponent>(FUI_RoomComponent.FUIRoomListName);


                fuiRoomComponent.FuiRoom.Visible = false;
                fuiRoomComponent.FuiRoomList.Visible = true;
                FUI_RoomUtilities.RemoveAllPlayerCard(fuiRoomComponent);
        

            FUI_RoomUtilities.RefreshRoomListBaseOnRoomData(fuiRoomComponent);
            
            await ETTask.CompletedTask;
        }
    }
}