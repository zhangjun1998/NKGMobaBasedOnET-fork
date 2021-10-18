namespace ET
{
    /// <summary>
    /// 初始化战斗UI
    /// </summary>
    public class EnterMapFinish_Init : AEvent<EventType.EnterMapFinish>
    {
        protected override async ETTask Run(EventType.EnterMapFinish args)
        {
            Scene scene = args.ZoneScene;

            await scene.GetComponent<FUIPackageManagerComponent>().AddPackageAsync(FUIPackage.BattleMain);
            FUI_Battle_Main fuiUIPanelBattle = await FUI_Battle_Main.CreateInstanceAsync(args.ZoneScene);
            fuiUIPanelBattle.self.MakeFullScreen();

            FUIManagerComponent fuiManagerComponent = scene.GetComponent<FUIManagerComponent>();
            FUI_BattleComponent fuiBattleComponent =
                Entity.Create<FUI_BattleComponent, FUI_Battle_Main>(fuiManagerComponent,
                    fuiUIPanelBattle, true);

            scene.GetComponent<FUIManagerComponent>().Add(FUIPackage.BattleMain, fuiUIPanelBattle, fuiBattleComponent);
            
            Game.Scene.GetComponent<CameraComponent>().SetTargetUnit(scene.GetComponent<RoomManagerComponent>().GetOrCreateBattleRoom().GetComponent<UnitComponent>().MyUnit);
            
            await scene.GetComponent<FUIPackageManagerComponent>().AddPackageAsync(FUIPackage.FlyFont);
        }
    }
}