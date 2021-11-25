namespace ET
{
    [LSF_MessageHandler(LSF_CmdHandlerType = LSF_CreateSpilingCmd.CmdType)]
    public class LSF_CreateSpilingHandler: ALockStepStateFrameSyncMessageHandler<LSF_CreateSpilingCmd>
    {
        protected override async ETVoid Run(Unit unit, LSF_CreateSpilingCmd cmd)
        {
            UnitFactory.CreateHeroSpilingUnit(unit.BelongToRoom, cmd.UnitInfo);
            await ETTask.CompletedTask;
        }
    }
}