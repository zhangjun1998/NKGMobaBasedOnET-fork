namespace ET
{
    public class LSF_SyncAttributeHandler: ALockStepStateFrameSyncMessageHandler<LSF_SyncAttributeCmd>
    {
        protected override async ETVoid Run(Unit unit, LSF_SyncAttributeCmd cmd)
        {
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            foreach (var attributeToSync in cmd.SyncAttributes)
            {
                numericComponent[attributeToSync.Key] = attributeToSync.Value;
            }

            await ETTask.CompletedTask;
        }
    }
}