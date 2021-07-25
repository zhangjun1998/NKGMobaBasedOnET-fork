using System;
using ETModel;


namespace ETHotfix
{
    [ActorMessageHandler(AppType.Room)]
    public class C2RM_RequestStartBattleHandler : AMActorLocationRpcHandler<Unit, C2RM_RequestStartBattle, RM2C_RequestStartBattle>
    {
        protected override async ETTask Run(Unit unit, C2RM_RequestStartBattle request, RM2C_RequestStartBattle response, Action reply)
        {
            if (unit.GetParent<RoomEntity>().GetComponent<BattleLoadingComponent>()!=null)
            {
                response.Error = ErrorCode.ERR_AlreadyInLoading;
                reply();
                return;
            }
            unit.GetParent<RoomEntity>().AddComponent<BattleEntity>();
            unit.GetParent<RoomEntity>().AddComponent<BattleLoadingComponent>();
            reply();
        }
    }
}