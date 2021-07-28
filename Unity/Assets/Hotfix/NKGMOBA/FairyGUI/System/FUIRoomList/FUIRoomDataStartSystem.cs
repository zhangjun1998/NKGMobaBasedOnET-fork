//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月2日 17:09:27
//------------------------------------------------------------

using ETModel;
using System;

namespace ETHotfix
{
    [ObjectSystem]
    public class FUIRoomDataStartSystem : StartSystem<RoomData>
    {
        public override void Start(RoomData self)
        {
            self.JoinButton.self.onClick.Add(() => self.JoinRoom().Coroutine());
        }
    }
    public static class FUIRoomDataEX
    {
        public static async ETVoid JoinRoom(this RoomData self)
        {
            var resp = await ETModel.SessionComponent.Instance.Session.Call(new C2RM_JoinRoom() { RoomId = long.Parse(self.RoomId.text) });
            if (resp.Error != 0)
            {
                Game.EventSystem.Run(EventIdType.ShowErrorDialog, resp.Error);
            }
        }
    }
}