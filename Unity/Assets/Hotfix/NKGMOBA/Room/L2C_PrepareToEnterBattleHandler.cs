namespace ET
{
    public class L2C_PrepareToEnterBattleHandler: AMHandler<L2C_PrepareToEnterBattle>
    {
        protected override async ETVoid Run(Session session, L2C_PrepareToEnterBattle message)
        {
            //TODO 单独的加载界面UI
            FUI_LoadingComponent.ShowLoadingUI();
            await XAssetLoader.LoadSceneAsync(XAssetPathUtilities.GetScenePath("Map"));
            RoomManagerComponent roomManagerComponent = session.DomainScene().GetComponent<RoomManagerComponent>();
            
            //战斗房间，代表一场战斗
            Room battleRoom = roomManagerComponent.GetOrCreateBattleRoom();
            
            UnitComponent unitComponent = battleRoom.GetComponent<UnitComponent>();
            
            PlayerComponent playerComponent = Game.Scene.GetComponent<PlayerComponent>();

            foreach (UnitInfo unitInfo in message.Units)
            {
                if (unitComponent.Get(unitInfo.UnitId) != null)
                {
                    continue;
                }

                Unit unit = UnitFactory.CreateHero(battleRoom, unitInfo);
                if (unitInfo.BelongToPlayerId == playerComponent.PlayerId)
                {
                    unitComponent.MyUnit = unit;
                    unitComponent.MyUnit.AddComponent<PlayerHeroControllerComponent>();
                }
            }
            session.Send(new C2L_PreparedToEnterBattle() {PlayerId = playerComponent.PlayerId});
            await ETTask.CompletedTask;
        }
    }
}