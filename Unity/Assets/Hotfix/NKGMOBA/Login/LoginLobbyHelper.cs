using System;

namespace ET
{
    public class LoginLobbyHelper
    {
        public static async ETTask LoginLobby(Entity fuiComponent)
        {
            try
            {
                Scene zoneScene = fuiComponent.DomainScene();

                PlayerComponent playerComponent = Game.Scene.GetComponent<PlayerComponent>();
                L2C_LoginLobby l2cLoginLobby =
                    (L2C_LoginLobby) await playerComponent.LobbySession.Call(new C2L_LoginLobby()
                        {PlayerId = playerComponent.PlayerId});

                Log.Debug("登陆Lobby成功!, 拉取服务器房间列表");

                zoneScene.GetComponent<RoomManagerComponent>().RemoveAllRooms();
                for (int i = 0; i < l2cLoginLobby.RoomIdList.Count; i++)
                {
                    Room room = zoneScene.GetComponent<RoomManagerComponent>().CreateRoom(l2cLoginLobby.RoomIdList[i]);
                    room.RoomName = l2cLoginLobby.RoomNameList[i];
                    room.PlayerCount = l2cLoginLobby.RoomPlayerNum[i];
                    room.RoomHolderPlayerId = (int) l2cLoginLobby.RoomIdList[i];
                }

                Game.EventSystem.Publish(new EventType.LoginLobbyFinish() {DomainScene = zoneScene}).Coroutine();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}