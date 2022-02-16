namespace ET
{
    [LSF_MessageHandler(LSF_CmdHandlerType = LSF_SyncBuffCmd.CmdType)]
    public class LSF_SyncBuffHandler : ALockStepStateFrameSyncMessageHandler<LSF_SyncBuffCmd>
    {
        protected override async ETVoid Run(Unit unit, LSF_SyncBuffCmd cmd)
        {
            foreach (var snapInfo in cmd.BuffSnapInfoCollection.FrameBuffChangeSnap)
            {
                switch (snapInfo.Value.OperationType)
                {
                    case BuffSnapInfo.BuffOperationType.ADD:
                        IBuffSystem addedBuffSystem = BuffFactory.AcquireBuff(unit.BelongToRoom,
                            snapInfo.Value.NP_SupportId, snapInfo.Value.BuffNodeId,
                            snapInfo.Value.FromUnitId, snapInfo.Value.BelongtoUnitId, snapInfo.Value.BelongtoNP_RuntimeTreeId);
                        addedBuffSystem.CurrentOverlay = snapInfo.Value.BuffLayer;
                        if (addedBuffSystem.CurrentOverlay != 1)
                        {
                            addedBuffSystem.Refresh(cmd.Frame);
                        }
                        break;
                    case BuffSnapInfo.BuffOperationType.CHANGE:
                        IBuffSystem changedBuffSystem =
                            unit.GetComponent<BuffManagerComponent>().GetBuffById(snapInfo.Value.BuffId);
                        BuffTimerAndOverlayHelper.CalculateTimerAndOverlay(changedBuffSystem, cmd.Frame, snapInfo.Value.BuffLayer);
                        break;
                    case BuffSnapInfo.BuffOperationType.REMOVE:
                        unit.BelongToRoom.GetComponent<UnitComponent>().Get(snapInfo.Value.BelongtoUnitId)
                            .GetComponent<BuffManagerComponent>().RemoveBuff(snapInfo.Value.BuffNodeId);
                        break;
                    default:
                        Log.Error("BuffOperationType 为 空，请检查逻辑");
                        break;
                }
            }

            await ETTask.CompletedTask;
        }
    }
}