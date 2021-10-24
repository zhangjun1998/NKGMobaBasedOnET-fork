#if !SERVER
namespace ET
{
    public class PlayerHeroControllerUpdateSystem : UpdateSystem<PlayerHeroControllerComponent>
    {
        public override void Update(PlayerHeroControllerComponent self)
        {
            if (self.UserInputComponent.QDown)
            {
                Game.Scene.GetComponent<PlayerComponent>().GateSession.Send(new C2M_UserInputSkillCmd() {VK = "Q"});
            }

            if (self.UserInputComponent.WDown)
            {
                Game.Scene.GetComponent<PlayerComponent>().GateSession.Send(new C2M_UserInputSkillCmd() {VK = "W"});
            }

            if (self.UserInputComponent.EDown)
            {
                Game.Scene.GetComponent<PlayerComponent>().GateSession.Send(new C2M_UserInputSkillCmd() {VK = "E"});
            }

            if (self.UserInputComponent.RDown)
            {
                Game.Scene.GetComponent<PlayerComponent>().GateSession.Send(new C2M_UserInputSkillCmd() {VK = "R"});
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
