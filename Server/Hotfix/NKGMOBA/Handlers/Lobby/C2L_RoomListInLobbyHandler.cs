using System;
using System.Collections.Generic;

namespace ET
{
    [ActorMessageHandler]
    public class C2L_RoomListInLobbyHandler : AMActorRpcHandler<Scene, C2L_RoomListInLobby, L2C_RoomListInLobby>
    {
        protected override async ETTask Run(Scene scene, C2L_RoomListInLobby request, L2C_RoomListInLobby response, Action reply)
        {
            response.RoomList.AddRange(scene.GetComponent<RoomManagerComponent>().Rooms.Values);
            reply();
            await ETTask.CompletedTask;
        }
    }
}