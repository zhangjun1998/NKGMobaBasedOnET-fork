using System;
using ETModel;


namespace ETHotfix
{
    [ActorMessageHandler(AppType.Room)]
    public class C2RM_KickRoomPlayerHandler : AMActorLocationRpcHandler<Unit, C2RM_KickRoomPlayer, RM2C_KickRoomPlayer>
    {
        protected override async ETTask Run(Unit unit, C2RM_KickRoomPlayer request, RM2C_KickRoomPlayer response, Action reply)
        {
            if (request.KickId==unit.Id)
            {
                response.Error = ErrorCode.ERR_RoomKickSelf;
                reply();
                return;
            }
            if (unit.GetComponent<RoomPlayerData>().IsMaster)
            {
                if (unit.GetParent<RoomEntity>().GetComponent<BattleLoadingComponent>() != null)
                {
                    response.Error = ErrorCode.ERR_AlreadyInLoading;
                    reply();
                    return;
                }
                unit.TempScene.RemoveUnit(request.KickId,RoomPlayerQuitTypeEnum.BeKicked);
            }
            reply();
        }
    }
}