namespace ET
{
    public class LSF_PathFindCmdHandler : ALockStepStateFrameSyncMessageHandler<LSF_PathFindCmd>
    {
        protected override async ETVoid Run(Unit unit, LSF_PathFindCmd cmd)
        {
            Log.Info($"---------收到寻路指令，消息的帧号为：{cmd.Frame}");
            // LSF_Component lsfComponent = unit.BelongToRoom.GetComponent<LSF_Component>();
            //
            // LSF_MoveCmd lsfMoveCmd =
            //     ReferencePool.Acquire<LSF_MoveCmd>().Init(unit.Id) as LSF_MoveCmd;
            // lsfComponent.SendMessage(lsfMoveCmd);

            await ETTask.CompletedTask;
        }

        public override uint GetLSF_CmdType()
        {
            return LSF_PathFindCmd.CmdType;
        }
    }
}