namespace ET
{
    public class C2M_UserInputSkillCmdHandler : AMActorLocationHandler<Unit, C2M_UserInputSkillCmd>
    {
        protected override async ETTask Run(Unit entity, C2M_UserInputSkillCmd message)
        {
            foreach (var skillTree in entity.GetComponent<NP_RuntimeTreeManager>().RuntimeTrees)
            {
                skillTree.Value.GetBlackboard().Set("PlayerInput", message.VK);
            }

            await ETTask.CompletedTask;
        }
    }
}