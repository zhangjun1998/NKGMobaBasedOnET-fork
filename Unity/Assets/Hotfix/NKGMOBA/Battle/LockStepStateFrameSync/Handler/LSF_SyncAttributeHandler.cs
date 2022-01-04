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
            
#if SERVER
            // 对于客户端发来的每一条指令，都要进行一次广播，因为多人模式需要进行同步，
            LSF_Component lsfComponent = unit.BelongToRoom.GetComponent<LSF_Component>();
            lsfComponent.AddCmdToSendQueue(cmd);
#endif

            await ETTask.CompletedTask;
        }
    }
}