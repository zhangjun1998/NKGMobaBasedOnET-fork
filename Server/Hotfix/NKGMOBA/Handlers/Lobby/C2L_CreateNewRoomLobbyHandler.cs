using System;

namespace ET
{
    //点击创建房间逻辑,暂时默认十人房
    public class C2L_CreateNewRoomLobbyHandler : AMRpcHandler<C2L_CreateNewRoomLobby, L2C_CreateNewRoomLobby>
    {
        protected override async ETTask Run(Session session, C2L_CreateNewRoomLobby request,
            L2C_CreateNewRoomLobby response,
            Action reply)
        {
            var player = session.GetComponent<SessionPlayerComponent>().Player;
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.Room, player.Id))
            {
                if (session.GetComponent<RoomStateOnGateComponent>() != null)
                {
                    response.Error = ErrorCode.ERR_AlreadyInRoom;
                    reply();
                    return;
                }
                var playerinfo = new PlayerInfoRoom() { camp = 1, Name = player.Name, playerid = player.Id };
                var resp = (RM2G_CreateNewRoomLobby)await MessageHelper.CallActor(
                    StartSceneConfigCategory.Instance.GetBySceneName(1, "RoomManager").InstanceId,
                    new G2RM_CreateNewRoomLobby()
                    {
                        RoomConfig = new RoomConfigProto() { RoomName = $"{player.Name}的房间", RoomPlayerNum = 10 },
                    });
                if (resp.Error != 0)
                {
                    response.Error = resp.Error;
                    reply();
                    return;
                }
                var joinresp = (L2C_JoinRoomLobby)await MessageHelper.CallActor(resp.RoomInfo.RoomId, new G2L_JoinRoomLobby() { Player = player, IsRoomHolder = true });
                if (joinresp.Error != 0)
                {
                    response.Error = joinresp.Error;
                    reply();
                    return;
                }
                session.AddComponent<RoomStateOnGateComponent, long, long>(joinresp.RoomPlayerActorId, joinresp.RoomInfo.RoomId);
                response.RoomInfo = joinresp.RoomInfo;
                response.playerInfoRoom.Add(playerinfo);
                reply();
            }
        }
    }
}