using System;

namespace ET
{
    //RoomManager通知RoomAgent创建新的房间场景
    [ActorMessageHandler]
    public class RM2RA_CreateNewRoomLobbyHandler : AMActorRpcHandler<Scene, RM2RA_CreateNewRoomLobby, RA2RM_CreateNewRoomLobby>
    {
        protected override async ETTask Run(Scene scene, RM2RA_CreateNewRoomLobby request, RA2RM_CreateNewRoomLobby response, Action reply)
        {
            var newroomScene = await RoomSceneFactory.Create(scene, request.RoomConfig);
            response.RoomInfo = RoomHelper.GetRoomInfoProto(newroomScene);
            reply();
        }
    }
}