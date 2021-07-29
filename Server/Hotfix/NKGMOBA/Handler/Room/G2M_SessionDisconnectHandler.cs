using ETModel;


namespace ETHotfix
{
    [ActorMessageHandler(AppType.Room)]
    public class G2M_SessionDisconnectHandler : AMActorLocationHandler<Unit, G2M_SessionDisconnect>
    {
        protected override async ETTask Run(Unit unit, G2M_SessionDisconnect request)
        {
            unit.GetComponent<UnitGateComponent>().IsDisconnect = true;
            bool isMaster = unit.GetComponent<RoomPlayerData>().IsMaster;
            RoomEntity room = unit.TempScene;
            //没有开始战斗的话.当前玩家退出房间
            if (unit.TempScene.GetComponent<BattleLoadingComponent>()==null)
            {
                await unit.TempScene.RemoveUnit(unit.Id, RoomPlayerQuitTypeEnum.SelfQuit);
            }
        }
    }
}