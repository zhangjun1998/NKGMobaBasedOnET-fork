//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月2日 17:11:17
//------------------------------------------------------------

using ETModel;
using System;

namespace ETHotfix
{
    /// <summary>
    /// 进入房间列表
    /// </summary>
    [Event(EventIdType.ShowRoomList)]
    public class CreateRoomListUI: AEvent
    {
        public override void Run()
        {
            var room = FUIRoomList.CreateInstance();
            //默认将会以Id为Name，也可以自定义Name，方便查询和管理
            room.Name = FUIPackage.FUIRoom_FUIRoomList;
            room.GObject.sortingOrder = 30;
            room.MakeFullScreen();
            Game.Scene.GetComponent<FUIComponent>().Add(room, true);
        }
    }
    
    [Event(EventIdType.QuitRoomListToLobby)]
    public class CloseRoomListUI: AEvent
    {
        public override void Run()
        {
            Game.Scene.GetComponent<FUIComponent>().Remove(FUIPackage.FUIRoom_FUIRoomList);
        }
    }
    [Event(EventIdType.EnterMapFinish)]
    public class EnterMapCloseRoomListUI : AEvent
    {
        public override void Run()
        {
            Game.Scene.GetComponent<FUIComponent>().Remove(FUIPackage.FUIRoom_FUIRoomList);
        }
    }
}