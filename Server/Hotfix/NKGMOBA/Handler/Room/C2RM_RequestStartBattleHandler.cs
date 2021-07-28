using System;
using ETModel;


namespace ETHotfix
{
    [ActorMessageHandler(AppType.Room)]
    public class C2RM_RequestStartBattleHandler : AMActorLocationRpcHandler<Unit, C2RM_RequestStartBattle, RM2C_RequestStartBattle>
    {
        protected override async ETTask Run(Unit unit, C2RM_RequestStartBattle request, RM2C_RequestStartBattle response, Action reply)
        {
            if (unit.GetComponent<RoomPlayerData>().IsMaster)
            {
                if (unit.TempScene.GetComponent<BattleLoadingComponent>() != null)
                {
                    response.Error = ErrorCode.ERR_AlreadyInLoading;
                    reply();
                    return;
                }
                reply();
                unit.TempScene.AddComponent<B2S_WorldColliderManagerComponent>();
                unit.TempScene.AddComponent<B2S_WorldComponent>();
                unit.TempScene.AddComponent<B2S_CollisionListenerComponent>();
                unit.TempScene.AddComponent<BattleEntity>();
                unit.TempScene.AddComponent<BattleLoadingComponent>();
            }
            else
            {
                response.Error = ErrorCode.ERR_RoomSelfNotAdmin;
                reply();
            }
            
        }
    }
}