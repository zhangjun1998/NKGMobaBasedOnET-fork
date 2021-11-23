using System;

namespace ET
{
    [ActorMessageHandler]
    //gate向RoomManager申请新的房间
    public class G2RM_CreateNewRoomLobbyHandler : AMActorRpcHandler<Scene, G2RM_CreateNewRoomLobby, RM2G_CreateNewRoomLobby>
    {
        protected override async ETTask Run(Scene scene, G2RM_CreateNewRoomLobby request, RM2G_CreateNewRoomLobby response, Action reply)
        {
            var roommanager = scene.GetComponent<RoomManagerComponent>();
            int n = RandomHelper.RandomNumber(0, StartSceneConfigCategory.Instance.RoomAgent.Count);
            var actorid = StartSceneConfigCategory.Instance.RoomAgent[n].InstanceId;
            var resp = (RA2RM_CreateNewRoomLobby)await MessageHelper.CallActor(actorid, new RM2RA_CreateNewRoomLobby() { RoomConfig = request.RoomConfig });
            if (resp.Error != 0)
            {
                response.Error = resp.Error;
                reply();
                return;
            }
            roommanager.Rooms.Add(resp.RoomInfo.RoomId, resp.RoomInfo);
            response.RoomInfo = resp.RoomInfo;
            reply();
        }
    }
}