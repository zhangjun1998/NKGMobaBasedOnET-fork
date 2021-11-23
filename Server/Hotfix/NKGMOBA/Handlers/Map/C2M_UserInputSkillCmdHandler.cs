namespace ET
{
    public class C2M_UserInputSkillCmdHandler : AMActorHandler<Player, C2M_UserInputSkillCmd>
    {
        protected override async ETTask Run(Player player, C2M_UserInputSkillCmd message)
        {
            Unit unit = player.Domain.GetComponent<UnitComponent>().Get(player.UnitId);
            foreach (var skillTree in unit.GetComponent<NP_RuntimeTreeManager>().RuntimeTrees)
            {
                skillTree.Value.GetBlackboard().Set("PlayerInput", message.VK);
            }

            await ETTask.CompletedTask;
        }
    }
}