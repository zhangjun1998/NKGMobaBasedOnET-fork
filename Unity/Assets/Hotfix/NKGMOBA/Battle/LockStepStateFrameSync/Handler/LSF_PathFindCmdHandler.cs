namespace ET
{
    public class LSF_PathFindCmdHandler : ALockStepStateFrameSyncMessageHandler<LSF_PathFindCmd>
    {
        protected override async ETVoid Run(Unit unit, LSF_PathFindCmd cmd)
        {
            LSF_Component lsfComponent = unit.BelongToRoom.GetComponent<LSF_Component>();

            LSF_MoveCmd lsfMoveCmd =
                ReferencePool.Acquire<LSF_MoveCmd>().Init(unit.Id) as LSF_MoveCmd;
            lsfComponent.SendMessage(lsfMoveCmd);

            await ETTask.CompletedTask;
        }

        public override uint GetLSF_CmdType()
        {
            return LSF_PathFindCmd.CmdType;
        }
    }
}