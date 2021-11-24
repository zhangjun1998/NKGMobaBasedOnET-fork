using System;

namespace ET
{
    //玩家加入房间
    public class C2L_JoinRoomHandler : AMRpcHandler<C2L_JoinRoomLobby, L2C_JoinRoomLobby>
    {
        protected override async ETTask Run(Session session, C2L_JoinRoomLobby request, L2C_JoinRoomLobby response,
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
                var resp = (L2C_JoinRoomLobby)await MessageHelper.CallActor(request.RoomId, new G2L_JoinRoomLobby() { Player = player, IsRoomHolder = false });
                if (resp.Error != 0)
                {
                    response.Error = resp.Error;
                    reply();
                    return;
                }
                response.RoomInfo = resp.RoomInfo;
                response.playerInfoRoom = resp.playerInfoRoom;
                session.AddComponent<RoomStateOnGateComponent, long, long>(resp.RoomPlayerActorId, resp.RoomInfo.RoomId);

                reply();
            }
        }
    }
}