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
    public class FUIPlayerDataStartSystem : StartSystem<PlayerData>
    {
        public override void Start(PlayerData self)
        {
            self.KickButton.self.onClick.Add(() => self.KickPlayer().Coroutine());
        }
    }
    public static class FUIPlayerDataEX
    {
        public static async ETVoid KickPlayer(this PlayerData self)
        {
            var resp = await ETModel.SessionComponent.Instance.Session.Call(new C2RM_KickRoomPlayer() { KickId=long.Parse(self.PlayerId.text) });
            if (resp.Error != 0)
            {
                Game.EventSystem.Run(EventIdType.ShowErrorDialog, resp.Error);
            }
        }
    }
}