using System;
using ET.EventType;

namespace ET
{
    public static class CreateRoomHelper
    {
        public static async ETTask CreateRoom(Entity fuiComponent)
        {
            try
            {
                Scene zoneScene = fuiComponent.DomainScene();

                PlayerComponent playerComponent = Game.Scene
                    .GetComponent<PlayerComponent>();
                
                L2C_CreateNewRoomLobby l2CCreateNewRoomLobby = (L2C_CreateNewRoomLobby)await playerComponent.GateSession
                    .Call(new C2L_CreateNewRoomLobby(){});

                Room room = zoneScene.GetComponent<RoomManagerComponent>().GetOrCreateBattleRoom();
                room.RoomHolderPlayerId = l2CCreateNewRoomLobby.RoomInfo.RoomHolderPlayer;
                room.RoomName = l2CCreateNewRoomLobby.RoomInfo.RoomConfig.RoomName;
                room.PlayerCount = l2CCreateNewRoomLobby.playerInfoRoom.Count;
                Game.EventSystem.Publish(new JoinRoom(){DomainScene = zoneScene,PlayerInfoRooms = l2CCreateNewRoomLobby.playerInfoRoom}).Coroutine();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}