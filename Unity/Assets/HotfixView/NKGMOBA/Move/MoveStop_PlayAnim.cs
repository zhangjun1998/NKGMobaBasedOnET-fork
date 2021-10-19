using ET.EventType;

namespace ET
{
    public class MoveStop_PlayAnim: AEvent<EventType.MoveStop>
    {
        protected override async ETTask Run(MoveStop a)
        {
            a.Unit.GetComponent<StackFsmComponent>().RemoveState("Navigate");
            a.Unit.GetComponent<AnimationComponent>().PlayAnimByStackFsmCurrent();
            await ETTask.CompletedTask;
        }
    }
}