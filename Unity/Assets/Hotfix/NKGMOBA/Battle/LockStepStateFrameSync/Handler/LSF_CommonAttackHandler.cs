namespace ET
{
    [LSF_MessageHandler(LSF_CmdHandlerType = LSF_CommonAttackCmd.CmdType)]
    public class LSF_CommonAttackHandler : ALockStepStateFrameSyncMessageHandler<LSF_CommonAttackCmd>
    {
        protected override async ETVoid Run(Unit unit, LSF_CommonAttackCmd cmd)
        {
            Log.Info(cmd.TargetUnitId.ToString());
            unit.GetComponent<CommonAttackComponent_Logic>()
                .SetAttackTarget(unit.BelongToRoom.GetComponent<UnitComponent>().Get(cmd.TargetUnitId));

            
#if !SERVER
            UnitComponent unitComponent = unit.BelongToRoom.GetComponent<UnitComponent>();
            Game.EventSystem.Publish(new EventType.CommonAttack()
            {
                AttackCast = unit,
                AttackTarget = unitComponent.Get(cmd.TargetUnitId)
            }).Coroutine();
            Log.Info(cmd.TargetUnitId.ToString());
#endif
            
            await ETTask.CompletedTask;
        }
    }
}