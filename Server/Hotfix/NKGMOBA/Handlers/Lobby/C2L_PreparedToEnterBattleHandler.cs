using UnityEngine;

namespace ET
{
    public class C2L_PreparedToEnterBattleHandler : AMActorHandler<Player, C2L_PreparedToEnterBattle>
    {
        protected override async ETTask Run(Player player, C2L_PreparedToEnterBattle message)
        {
            RoomPreparedToEnterBattleComponent preparedComponent = player.DomainScene().GetComponent<RoomPreparedToEnterBattleComponent>();
            if (preparedComponent==null)
            {
                Log.Error("RoomPreparedToEnterBattleComponent is null");
                return;
            }
            preparedComponent.PlayerPrepare(player.Id);
            await ETTask.CompletedTask;
        }
    }
}