﻿using System;

namespace ET.Hotfix.Demo.Room
{
    public class JoinRoomHelper
    {
        public static async ETTask JoinRoom(Entity fuiComponent, long roomId)
        {
            try
            {
                Scene zoneScene = fuiComponent.DomainScene();

                PlayerComponent playerComponent = Game.Scene.GetComponent<PlayerComponent>();
                
                L2C_JoinRoomLobby kL2CJoinRoomLobby = (L2C_JoinRoomLobby) await playerComponent.GateSession
                    .Call(new C2L_JoinRoomLobby()
                        {RoomId = roomId});

                
                // 根据服务器回包，处理房间玩家列表
                Game.EventSystem.Publish(new EventType.JoinRoom()
                    {DomainScene = zoneScene, PlayerInfoRooms = kL2CJoinRoomLobby.playerInfoRoom}).Coroutine();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}