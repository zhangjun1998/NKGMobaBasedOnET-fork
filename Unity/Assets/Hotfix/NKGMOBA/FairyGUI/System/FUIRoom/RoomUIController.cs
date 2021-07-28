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
    /// 收到房间信息.进入房间
    /// </summary>
    [Event(EventIdType.ShowRoom)]
    public class ReceiveMsg_CreateRoomUI: AEvent<RoomInfo>
    {
        public override void Run(RoomInfo roomInfo)
        {
            var room = FUIRoom.CreateInstance();
            //默认将会以Id为Name，也可以自定义Name，方便查询和管理
            room.Name = FUIPackage.FUIRoom_FUIRoom;
            room.GObject.sortingOrder = 30;
            room.MakeFullScreen();
            room.RoomName.text = roomInfo.RoomName;
            room.Team1.RemoveChildren();
            room.Team2.RemoveChildren();
            bool isMaster = false;
            foreach (RoomPlayer roomplayer in roomInfo.RoomPlayers)
            {
                var playerfgui = PlayerData.CreateInstance();
                //当是玩家自己的时候无论如何也没有管理员选项(踢人)
                if (roomplayer.PlayerId == PlayerComponent.Instance.MyPlayer.Id)
                {
                    isMaster = roomplayer.IsMaster;
                }
                else
                {
                    playerfgui.HasAdminFunc.selectedIndex = Convert.ToInt32(isMaster);
                }
                playerfgui.IsMaster.selectedIndex = Convert.ToInt32(roomplayer.IsMaster);
                playerfgui.RoomPlayerName.text = roomplayer.Name;
                playerfgui.RoomPlayerLevel.text = "30";
                playerfgui.PlayerId.text = roomplayer.PlayerId.ToString();
                if (roomplayer.IsRed)
                {
                    room.Team1.AddChild(playerfgui.GObject);
                }
                else
                {
                    room.Team2.AddChild(playerfgui.GObject);
                }
                
            }
            room.IsMaster.selectedIndex = Convert.ToInt32(isMaster);
            Game.Scene.GetComponent<FUIComponent>().Add(room, true);
        }
    }
    /// <summary>
    /// 收到房间信息.进入房间
    /// </summary>
    [Event(EventIdType.UpdateRoom)]
    public class ReceiveMsg_UpdateRoomUI : AEvent<RoomInfo>
    {
        public override void Run(RoomInfo roomInfo)
        {
            var room=(FUIRoom)Game.Scene.GetComponent<FUIComponent>().Get(FUIPackage.FUIRoom_FUIRoom);
            room.Team1.RemoveChildren();
            room.Team2.RemoveChildren();
            bool isMaster = false;
            foreach (RoomPlayer roomplayer in roomInfo.RoomPlayers)
            {
                var playerfgui = PlayerData.CreateInstance();
                //当是玩家自己的时候无论如何也没有管理员选项(踢人)
                if (roomplayer.PlayerId == PlayerComponent.Instance.MyPlayer.Id)
                {
                    isMaster = roomplayer.IsMaster;
                }
                else
                {
                    playerfgui.HasAdminFunc.selectedIndex = Convert.ToInt32(isMaster);
                }
                playerfgui.IsMaster.selectedIndex = Convert.ToInt32(roomplayer.IsMaster);
                playerfgui.RoomPlayerName.text = roomplayer.Name;
                playerfgui.RoomPlayerLevel.text = "30";
                playerfgui.PlayerId.text = roomplayer.PlayerId.ToString();
                if (roomplayer.IsRed)
                {
                    room.Team1.AddChild(playerfgui.GObject);
                }
                else
                {
                    room.Team2.AddChild(playerfgui.GObject);
                }

            }
            room.IsMaster.selectedIndex = Convert.ToInt32(isMaster);
        }
    }
    [Event(EventIdType.QuitRoomToRoomList)]
    public class CloseRoomUI: AEvent
    {
        public override void Run()
        {
            Game.Scene.GetComponent<FUIComponent>().Remove(FUIPackage.FUIRoom_FUIRoom);

        }
    }
    [Event(EventIdType.EnterMapFinish)]
    public class EnterMapCloseRoomUI : AEvent
    {
        public override void Run()
        {
            Game.Scene.GetComponent<FUIComponent>().Remove(FUIPackage.FUIRoom_FUIRoom);
        }
    }
}