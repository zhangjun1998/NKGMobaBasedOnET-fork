#if !SERVER
namespace ET
{
    public class PlayerHeroControllerUpdateSystem : UpdateSystem<PlayerHeroControllerComponent>
    {
        public override void Update(PlayerHeroControllerComponent self)
        {
            if (self.UserInputComponent.QDown)
            {
                Unit unit = self.GetParent<Unit>();
                LSF_PlaySkillInputCmd lsfPlaySkillInputCmd = ReferencePool.Acquire<LSF_PlaySkillInputCmd>();
                lsfPlaySkillInputCmd.Init(unit.Id);
                lsfPlaySkillInputCmd.InputKey = "Q";
                unit.BelongToRoom.GetComponent<LSF_Component>().AddCmdToSendQueue(lsfPlaySkillInputCmd);
            }

            if (self.UserInputComponent.WDown)
            {
                Unit unit = self.GetParent<Unit>();
                LSF_PlaySkillInputCmd lsfPlaySkillInputCmd = ReferencePool.Acquire<LSF_PlaySkillInputCmd>();
                lsfPlaySkillInputCmd.Init(unit.Id);
                lsfPlaySkillInputCmd.InputKey = "W";
                unit.BelongToRoom.GetComponent<LSF_Component>().AddCmdToSendQueue(lsfPlaySkillInputCmd);
            }

            if (self.UserInputComponent.EDown)
            {
                Unit unit = self.GetParent<Unit>();
                LSF_PlaySkillInputCmd lsfPlaySkillInputCmd = ReferencePool.Acquire<LSF_PlaySkillInputCmd>();
                lsfPlaySkillInputCmd.Init(unit.Id);
                lsfPlaySkillInputCmd.InputKey = "E";
                unit.BelongToRoom.GetComponent<LSF_Component>().AddCmdToSendQueue(lsfPlaySkillInputCmd);
            }

            if (self.UserInputComponent.RDown)
            {
                Unit unit = self.GetParent<Unit>();
                LSF_PlaySkillInputCmd lsfPlaySkillInputCmd = ReferencePool.Acquire<LSF_PlaySkillInputCmd>();
                lsfPlaySkillInputCmd.Init(unit.Id);
                lsfPlaySkillInputCmd.InputKey = "R";
                unit.BelongToRoom.GetComponent<LSF_Component>().AddCmdToSendQueue(lsfPlaySkillInputCmd);
            }
        }
    }
    
    public class PlayerHeroControllerAwakeSystem : AwakeSystem<PlayerHeroControllerComponent>
    {
        public override void Awake(PlayerHeroControllerComponent self)
        {
            self.UserInputComponent = Game.Scene.GetComponent<UserInputComponent>();
        }
    }
}
#endif
