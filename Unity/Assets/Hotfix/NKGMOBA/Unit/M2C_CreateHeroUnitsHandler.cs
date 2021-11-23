using Vector3 = UnityEngine.Vector3;

namespace ET
{
    [MessageHandler]
    public class M2C_CreateHeroUnitsHandler : AMHandler<M2C_CreateUnits>
    {
        protected override async ETVoid Run(Session session, M2C_CreateUnits message)
        {
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
                // if (unitInfo.BelongToPlayerId == playerComponent.PlayerId)
                // {
                //     unitComponent.MyUnit = unit;
                //     unitComponent.MyUnit.AddComponent<PlayerHeroControllerComponent>();
                // }
            }
            await ETTask.CompletedTask;
        }
    }
}