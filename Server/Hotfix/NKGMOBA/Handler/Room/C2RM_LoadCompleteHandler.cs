using ETModel;


namespace ETHotfix
{
    [ActorMessageHandler(AppType.Room)]
    public class C2RM_LoadCompleteHandler : AMActorLocationHandler<Unit, C2RM_LoadComplete>
    {
        protected override async ETTask Run(Unit unit, C2RM_LoadComplete request)
        {
            unit.TempScene.GetComponent<BattleLoadingComponent>().Ready(unit.Id);
        }
    }
}