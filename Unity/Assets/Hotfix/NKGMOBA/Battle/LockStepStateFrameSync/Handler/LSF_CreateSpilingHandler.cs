namespace ET
{
    [LSF_MessageHandler(LSF_CmdHandlerType = LSF_CreateSpilingCmd.CmdType)]
    public class LSF_CreateSpilingHandler: ALockStepStateFrameSyncMessageHandler<LSF_CreateSpilingCmd>
    {
        protected override async ETVoid Run(Unit unit, LSF_CreateSpilingCmd cmd)
        {
#if SERVER
            // 类似召唤物的对象由于其创建指令实际上归属于系统，而不是召唤者
            cmd.UnitId = -1;
            cmd.UnitInfo.UnitId = IdGenerater.Instance.GenerateUnitId(unit.DomainZone());
            // 对于客户端发来的每一条指令，都要进行一次广播，因为多人模式需要进行同步，
            LSF_Component lsfComponent = unit.BelongToRoom.GetComponent<LSF_Component>();
            lsfComponent.AddCmdToSendQueue(cmd);
#endif
            UnitFactory.CreateHeroSpilingUnit(unit.BelongToRoom, cmd.UnitInfo);
            
            await ETTask.CompletedTask;
        }
    }
}