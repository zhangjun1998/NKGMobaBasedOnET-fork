namespace ET
{
    [LSF_MessageHandler(LSF_CmdHandlerType = LSF_PlaySkillInputCmd.CmdType)]
    public class LSF_PlayerSkillInputHandler : ALockStepStateFrameSyncMessageHandler<LSF_PlaySkillInputCmd>
    {
        protected override async ETVoid Run(Unit unit, LSF_PlaySkillInputCmd cmd)
        {
            foreach (var skillTree in unit.GetComponent<NP_RuntimeTreeManager>().RuntimeTrees)
            {
                skillTree.Value.GetBlackboard().Set("PlayerInput", cmd.InputKey);
            }

            await ETTask.CompletedTask;
        }
    }
}