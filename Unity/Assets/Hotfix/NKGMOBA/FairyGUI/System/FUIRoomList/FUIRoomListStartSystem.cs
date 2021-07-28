//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月2日 17:09:27
//------------------------------------------------------------

using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class FUIRoomListStartSystem: StartSystem<FUIRoomList>
    {
        public override void Start(FUIRoomList self)
        {
            self.RefreshRoomAsync().Coroutine();
            self.CreateButton.self.onClick.Add(() => this.CreateRoom().Coroutine());
            self.RefreshButton.self.onClick.Add(() => self.RefreshRoomAsync().Coroutine());
            self.QutiButton.self.onClick.Add(() => this.QuitAsync());
            //self.normalPVPBtn.self.onClick.Add(() => this.EnterMapAsync());
        }

        private async ETVoid CreateRoom()
        {
            var resp = await ETModel.SessionComponent.Instance.Session.Call(new C2RM_CreateRoom());
            if (resp.Error!=0)
            {
                Game.EventSystem.Run(EventIdType.ShowErrorDialog,resp.Error);
            }
        }
        private void QuitAsync()
        {
            Game.EventSystem.Run(EventIdType.QuitRoomListToLobby);
        }

    }
    public static class FUIRoomListEX
    {
        public static async ETVoid RefreshRoomAsync(this FUIRoomList self)
        {
            G2C_AllRoomList resp = (G2C_AllRoomList)await ETModel.SessionComponent.Instance.Session.Call(new C2G_AllRoomList());
            if (resp.Error != 0)
            {
                Game.EventSystem.Run(EventIdType.ShowErrorDialog, resp.Error);
                return;
            }
            self.RoomList.RemoveChildren();
            foreach (RoomBriefInfo briefInfo in resp.RoomList)
            {
                var roomdata = RoomData.CreateInstance();
                roomdata.RoomName.text = briefInfo.RoomName;
                roomdata.PlayerNum.text = $"{briefInfo.CurrentMemberCount}/{briefInfo.MaxMemberCount}";
                roomdata.RoomId.text = briefInfo.RoomId.ToString();
                self.RoomList.AddChild(roomdata.GObject);
            }
        }
    }

}