using System;
using ETModel;


namespace ETHotfix
{
    [ActorMessageHandler(AppType.Room)]
    public class C2RM_QuitRoomHandler : AMActorLocationRpcHandler<Unit, C2RM_QuitRoom, RM2C_QuitRoom>
    {
        protected override async ETTask Run(Unit unit, C2RM_QuitRoom request, RM2C_QuitRoom response, Action reply)
        {
            if (!unit.GetParent<RoomEntity>().CanUnitChangeState)
            {
                response.Error = ErrorCode.ERR_AlreadyInBattle;
                reply();
                return;
            }
            unit.GetParent<RoomEntity>().RemoveUnit(unit);
            reply();
        }
    }
}