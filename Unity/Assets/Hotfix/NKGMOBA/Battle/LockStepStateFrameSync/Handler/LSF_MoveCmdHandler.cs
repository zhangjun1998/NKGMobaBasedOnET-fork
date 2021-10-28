namespace ET
{
    public class LSF_MoveCmdHandler: ALockStepStateFrameSyncMessageHandler<LSF_MoveCmd>
    {
        protected override async ETVoid Run(Unit unit, LSF_MoveCmd cmd)
        {
            //Log.Info($"---------收到移动指令：{cmd.Frame}");
            
            await ETTask.CompletedTask;
        }

        public override uint GetLSF_CmdType()
        {
            return LSF_MoveCmd.CmdType;
        }
    }
}