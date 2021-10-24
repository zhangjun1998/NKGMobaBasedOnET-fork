//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年12月6日 16:13:53
//------------------------------------------------------------

namespace ET
{
    public class M2C_CancelAttackHandler : AMHandler<M2C_CancelCommonAttack>
    {
        protected override async ETVoid Run(Session session, M2C_CancelCommonAttack message)
        {
            Unit unit = session.DomainScene().GetComponent<RoomManagerComponent>().GetBattleRoom().GetComponent<UnitComponent>().Get(message.TargetUnitId);
            Game.EventSystem.Publish(new EventType.CancelCommonAttack() {AttackCast = unit}).Coroutine();

            await ETTask.CompletedTask;
        }
    }
}