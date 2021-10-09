namespace ET
{
    public class StopMoveFromFSM : AEvent<EventType.CancelMoveFromFSM>
    {
        protected override async ETTask Run(EventType.CancelMoveFromFSM a)
        {
#if SERVER
                        a.Unit.Stop(1);
#endif

            await ETTask.CompletedTask;
        }
    }
}