using System;
using ETModel;


namespace ETHotfix
{
    [ActorMessageHandler(AppType.Room)]
    public class C2RM_QuitRoomHandler : AMActorLocationRpcHandler<Unit, C2RM_QuitRoom, RM2C_QuitRoom>
    {
        protected override async ETTask Run(Unit unit, C2RM_QuitRoom request, RM2C_QuitRoom response, Action reply)
        {
            bool isMaster = unit.GetComponent<RoomPlayerData>().IsMaster;
            RoomEntity room = unit.TempScene;
            if (!unit.TempScene.CanUnitChangeState)
            {
                response.Error = ErrorCode.ERR_AlreadyInBattle;
                reply();
                return;
            }
            unit.TempScene.RemoveUnit(unit.Id,RoomPlayerQuitTypeEnum.SelfQuit);
            reply();
            if (isMaster)
            {
                room.Dispose();
            }
        }
    }
}