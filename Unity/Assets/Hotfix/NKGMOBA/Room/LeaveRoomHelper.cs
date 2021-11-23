using System;

namespace ET
{
    public class LeaveRoomHelper
    {
        public static async ETTask LeaveRoom(Entity fuiComponent)
        {
            try
            {
                Scene zoneScene = fuiComponent.DomainScene();

                PlayerComponent playerComponent = Game.Scene.GetComponent<PlayerComponent>();

                L2C_LeaveRoomLobby l2CLeaveRoomLobby = (L2C_LeaveRoomLobby) await playerComponent.GateSession
                    .Call(new C2L_LeaveRoomLobby() {});

                if (l2CLeaveRoomLobby.Error==0)
                {
                    //zoneScene.GetComponent<RoomManagerComponent>().RemoveLobbyRoom(l2CLeaveRoomLobby.RoomId);
                    // 自己离开房间要清空本地所有玩家卡片
                    Game.EventSystem
                        .Publish(new EventType.LeaveRoom()
                        {
                            DomainScene = fuiComponent.DomainScene(), 
                        })
                        .Coroutine();
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}