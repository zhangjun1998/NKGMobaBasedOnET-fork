//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年8月29日 9:39:13
//------------------------------------------------------------

using ET.Hotfix.Demo.Room;

namespace ET
{
    public class FUI_LobbyComponentAwakeSystem : AwakeSystem<FUI_LobbyComponent, FUI_Lobby>
    {
        public override void Awake(FUI_LobbyComponent self, FUI_Lobby fuiLobby)
        {
            fuiLobby.m_Btn_RoomMode.self.onClick.Add(() => { OnRoomModeBtnClicked(self.DomainScene()).Coroutine(); });
            self.FuiUIPanelLobby = fuiLobby;
        }

        public static async ETVoid OnRoomModeBtnClicked(Scene scene)
        {
            PlayerComponent playerComponent = Game.Scene.GetComponent<PlayerComponent>();

            L2C_RoomListInLobby l2cLoginLobby = await Game.Scene.GetComponent<PlayerComponent>().GateSession
                .Call(new C2L_RoomListInLobby() ) as L2C_RoomListInLobby;
            scene.GetComponent<RoomManagerComponent>().RemoveAllLobbyRooms();
            for (int i = 0; i < l2cLoginLobby.RoomList.Count; i++)
            {
                Room room = scene.GetComponent<RoomManagerComponent>().CreateLobbyRoom(l2cLoginLobby.RoomList[i].RoomId);
                room.RoomName = l2cLoginLobby.RoomList[i].RoomConfig.RoomName;
                room.PlayerCount = l2cLoginLobby.RoomList[i].PlayerCount;
                room.RoomHolderPlayerId = l2cLoginLobby.RoomList[i].RoomHolderPlayer;
            }

            await scene.GetComponent<FUIPackageManagerComponent>().AddPackageAsync(FUIPackage.Room);
            FUIManagerComponent fuiManagerComponent = scene.GetComponent<FUIManagerComponent>();

            FUI_RoomList fuiRoomList = await FUI_RoomList.CreateInstanceAsync(scene);
            fuiRoomList.self.MakeFullScreen();

            FUI_Room fuiRoom = await FUI_Room.CreateInstanceAsync(scene);
            fuiRoom.self.MakeFullScreen();

            FUI_RoomComponent fuiRoomComponent =
                fuiManagerComponent.AddChild<FUI_RoomComponent, FUI_RoomList, FUI_Room>(fuiRoomList, fuiRoom);

            fuiManagerComponent.Add(FUI_RoomComponent.FUIRoomListName, fuiRoomList, fuiRoomComponent);
            fuiManagerComponent.Add(FUI_RoomComponent.FUIRoomName, fuiRoom, fuiRoomComponent);
        }
    }

    public class FUI_LobbyComponentUpdateSystem : UpdateSystem<FUI_LobbyComponent>
    {
        public override void Update(FUI_LobbyComponent self)
        {
        }
    }

    public class FUI_LobbyComponentDestroySystem : DestroySystem<FUI_LobbyComponent>
    {
        public override void Destroy(FUI_LobbyComponent self)
        {
            self.FuiUIPanelLobby = null;
        }
    }
}