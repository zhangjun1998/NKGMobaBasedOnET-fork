﻿using System;

namespace ET
{
    public class StartGameHelper
    {
        public static async ETTask StartGame(Entity fuiComponent)
        {
            try
            {
                Scene zoneScene = fuiComponent.DomainScene();

                PlayerComponent playerComponent = Game.Scene.GetComponent<PlayerComponent>();
                
                await playerComponent.GateSession.Call(new C2L_StartGameLobby(){});
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}