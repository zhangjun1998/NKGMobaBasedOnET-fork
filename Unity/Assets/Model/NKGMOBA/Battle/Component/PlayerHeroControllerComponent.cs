//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月3日 17:44:51
//------------------------------------------------------------

using System;

namespace ET
{
    [ObjectSystem]
    public class PlayerHeroControllerAwakeSystem : AwakeSystem<PlayerHeroControllerComponent>
    {
        public override void Awake(PlayerHeroControllerComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class PlayerHeroControllerUpdateSystem : UpdateSystem<PlayerHeroControllerComponent>
    {
        public override void Update(PlayerHeroControllerComponent self)
        {
            self.Update();
        }
    }

    /// <summary>
    /// 玩家自己操控英雄组件
    /// </summary>
    public class PlayerHeroControllerComponent : Entity
    {
        private UserInputComponent userInputComponent;

        public void Awake()
        {
            this.userInputComponent = Game.Scene.GetComponent<UserInputComponent>();
        }

        public void Update()
        {
            if (this.userInputComponent.QDown)
            {
                Game.Scene.GetComponent<PlayerComponent>().GateSession.Send(new C2M_UserInputSkillCmd() {VK = "Q"});
            }

            if (this.userInputComponent.WDown)
            {
                Game.Scene.GetComponent<PlayerComponent>().GateSession.Send(new C2M_UserInputSkillCmd() {VK = "W"});
            }

            if (this.userInputComponent.EDown)
            {
                Game.Scene.GetComponent<PlayerComponent>().GateSession.Send(new C2M_UserInputSkillCmd() {VK = "E"});
            }

            if (this.userInputComponent.RDown)
            {
                Game.Scene.GetComponent<PlayerComponent>().GateSession.Send(new C2M_UserInputSkillCmd() {VK = "R"});
            }
        }
    }
}