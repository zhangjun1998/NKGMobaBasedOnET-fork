namespace ET
{
    public class StopMoveFromFSM : AEvent<EventType.StopMoveFromFSM>
    {
        protected override async ETTask Run(EventType.StopMoveFromFSM a)
        {
            a.Unit.GetComponent<MoveComponent>().Stop();
            await ETTask.CompletedTask;
        }
    }
}