using Vector3 = UnityEngine.Vector3;

namespace ET
{
    [MessageHandler]
    public class M2C_CreateHeroUnitsHandler : AMHandler<M2C_CreateUnits>
    {
        protected override async ETVoid Run(Session session, M2C_CreateUnits message)
        {
            UnitComponent unitComponent = session.Domain.GetComponent<UnitComponent>();
            RoomManagerComponent roomManagerComponent = session.DomainScene().GetComponent<RoomManagerComponent>();
            Room room = roomManagerComponent.GetRoom(message.RoomId);
            PlayerComponent playerComponent = Game.Scene.GetComponent<PlayerComponent>();

            foreach (UnitInfo unitInfo in message.Units)
            {
                if (unitComponent.Get(unitInfo.UnitId) != null)
                {
                    continue;
                }

                Unit unit = UnitFactory.CreateHero(session.Domain, unitInfo);

                if (unitInfo.BelongToPlayerId == playerComponent.PlayerId)
                {
                    unitComponent.MyUnit = unit;
                    unitComponent.MyUnit.AddComponent<PlayerHeroControllerComponent>();
                    var moveComponent = unit.GetComponent<MoveComponent>();
                }

                playerComponent.HasCompletedLoadCount++;
                Log.Debug("playerComponent.HasCompletedLoadCount:" + playerComponent.HasCompletedLoadCount);
                if ((playerComponent.HasCompletedLoadCount == 1 && room.PlayerCount == 1) ||
                    (playerComponent.HasCompletedLoadCount == 2 && room.PlayerCount == 2 )||(playerComponent.HasCompletedLoadCount == 4 && room.PlayerCount == 4)||
                    (playerComponent.HasCompletedLoadCount == 6 && room.PlayerCount == 6)) //playerComponent.BelongToRoom.PlayerMaxCount)
                {
                    // 要取LobbySession才行，因为M2C的session为Gate
                    Session lobbySession = playerComponent.LobbySession;
                    lobbySession.Send(new C2L_PreparedToEnterBattle() {PlayerId = playerComponent.PlayerId});
                }
            }

            await ETTask.CompletedTask;
        }
    }
}