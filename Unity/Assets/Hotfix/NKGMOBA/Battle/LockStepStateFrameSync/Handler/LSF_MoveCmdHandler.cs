namespace ET
{
    [LSF_MessageHandler(LSF_CmdHandlerType = LSF_MoveCmd.CmdType)]
    public class LSF_MoveCmdHandler: ALockStepStateFrameSyncMessageHandler<LSF_MoveCmd>
    {
        protected override async ETVoid Run(Unit unit, LSF_MoveCmd cmd)
        {
            // Log.Info($"---------收到移动指令，消息的帧号为：{cmd.Frame}");
            
            await ETTask.CompletedTask;
        }
    }
}