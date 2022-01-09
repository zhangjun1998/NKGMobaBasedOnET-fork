namespace ET
{
    [LSF_MessageHandler(LSF_CmdHandlerType = LSF_CreateColliderCmd.CmdType)]
    public class LSF_CreateColliderHandler : ALockStepStateFrameSyncMessageHandler<LSF_CreateColliderCmd>
    {
        protected override async ETVoid Run(Unit unit, LSF_CreateColliderCmd cmd)
        {
#if !SERVER
            UnitFactory.CreateSpecialColliderUnit(unit.BelongToRoom, cmd.BelongtoUnitId, cmd.Id,
                cmd.ColliderNPBehaveTreeIdInExcel, cmd.ColliderDataConfigId);
#endif

            await ETTask.CompletedTask;
        }
    }
}