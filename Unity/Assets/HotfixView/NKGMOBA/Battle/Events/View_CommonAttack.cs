using ET.EventType;

namespace ET
{
    public class View_CommonAttack: AEvent<EventType.CommonAttack>
    {
        protected override async ETTask Run(CommonAttack a)
        {
            a.AttackCast.GetComponent<CommonAttackComponent>().CommonAttackStart(a.AttackTarget).Coroutine();
            await ETTask.CompletedTask;
        }
    }
}