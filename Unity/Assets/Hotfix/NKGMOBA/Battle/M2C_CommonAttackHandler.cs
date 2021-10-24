namespace ET
{
    public class M2C_CommonAttackHandler : AMHandler<M2C_CommonAttack>
    {
        protected override async ETVoid Run(Session session, M2C_CommonAttack message)
        {
            UnitComponent unitComponent = session.DomainScene().GetComponent<RoomManagerComponent>().GetBattleRoom().GetComponent<UnitComponent>();
            Game.EventSystem.Publish(new EventType.CommonAttack()
            {
                AttackCast = unitComponent.Get(message.AttackCasterId),
                AttackTarget = unitComponent.Get(message.TargetUnitId)
            }).Coroutine();

            await ETTask.CompletedTask;
        }
    }
}