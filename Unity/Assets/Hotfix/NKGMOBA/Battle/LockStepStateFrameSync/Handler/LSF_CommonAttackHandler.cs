namespace ET
{
    [LSF_MessageHandler(LSF_CmdHandlerType = LSF_CommonAttackCmd.CmdType)]
    public class LSF_CommonAttackHandler : ALockStepStateFrameSyncMessageHandler<LSF_CommonAttackCmd>
    {
        protected override async ETVoid Run(Unit unit, LSF_CommonAttackCmd cmd)
        {
            unit.GetComponent<CommonAttackComponent_Logic>()
                .SetAttackTarget(unit.BelongToRoom.GetComponent<UnitComponent>().Get(cmd.TargetUnitId));

            await ETTask.CompletedTask;
        }
    }
}