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
    public class FUIRoomStartSystem: StartSystem<FUIRoom>
    {
        public override void Start(FUIRoom self)
        {
            self.QuitButton.self.onClick.Add(() => this.GoToRoomList().Coroutine());
            self.StartButton.self.onClick.Add(() => this.StartBattleAsync().Coroutine());
        }

        private async ETVoid GoToRoomList()
        {
            var resp = await ETModel.SessionComponent.Instance.Session.Call(new C2RM_QuitRoom());
            if (resp.Error==0)
            {
                Game.EventSystem.Run(EventIdType.QuitRoomToRoomList);
            }
            else
            {
                Game.EventSystem.Run(EventIdType.ShowErrorDialog,resp.Error);
            }
        }

        private async ETVoid StartBattleAsync()
        {
            var resp = await ETModel.SessionComponent.Instance.Session.Call(new C2RM_RequestStartBattle());
            if (resp.Error != 0)
            {
                Game.EventSystem.Run(EventIdType.ShowErrorDialog, resp.Error);
            }
        }
    }
}