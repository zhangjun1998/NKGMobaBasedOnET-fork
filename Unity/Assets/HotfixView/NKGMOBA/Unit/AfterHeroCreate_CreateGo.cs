using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class AfterHeroCreate_CreateGo : AEvent<EventType.AfterHeroCreate_CreateGo>
    {
        protected override async ETTask Run(EventType.AfterHeroCreate_CreateGo args)
        {
            Client_UnitConfig clientUnitConfig = Client_UnitConfigCategory.Instance.Get(args.HeroConfigId);
            GameObjectComponent gameObjectComponent =
                args.Unit.AddComponent<GameObjectComponent, string>(clientUnitConfig.UnitName);

            gameObjectComponent.GameObject.transform.position =
                args.Unit.Position;

            args.Unit.AddComponent<AnimationComponent>();
            args.Unit.AddComponent<UnitTransformComponent>();
            args.Unit.AddComponent<TurnComponent>();
            args.Unit.AddComponent<EffectComponent>();
            args.Unit.AddComponent<CommonAttackComponent>();
            args.Unit.AddComponent<FallingFontComponent>();

            gameObjectComponent.GameObject.GetComponent<MonoBridge>().BelongToUnitId = args.Unit.Id;
            
            Client_SkillCanvasConfig unitPassiveSkillConfig =
                Client_SkillCanvasConfigCategory.Instance.Get(clientUnitConfig.UnitPassiveSkillId);
            Client_SkillCanvasConfig unitQSkillConfig =
                Client_SkillCanvasConfigCategory.Instance.Get(clientUnitConfig.UnitQSkillId);
            Client_SkillCanvasConfig unitWSkillConfig =
                Client_SkillCanvasConfigCategory.Instance.Get(clientUnitConfig.UnitWSkillId);
            Client_SkillCanvasConfig unitESkillConfig =
                Client_SkillCanvasConfigCategory.Instance.Get(clientUnitConfig.UnitESkillId);

            //英雄属性组件
            args.Unit.AddComponent<UnitAttributesDataComponent, long>(clientUnitConfig.UnitAttributesDataId);

            //Log.Info("开始装载技能");
            NP_RuntimeTreeFactory.CreateSkillNpRuntimeTree(args.Unit, unitPassiveSkillConfig.NPBehaveId,
                unitPassiveSkillConfig.BelongToSkillId).Start();
            NP_RuntimeTreeFactory
                .CreateSkillNpRuntimeTree(args.Unit, unitQSkillConfig.NPBehaveId, unitQSkillConfig.BelongToSkillId)
                .Start();
            NP_RuntimeTreeFactory
                .CreateSkillNpRuntimeTree(args.Unit, unitWSkillConfig.NPBehaveId, unitWSkillConfig.BelongToSkillId)
                .Start();
            NP_RuntimeTreeFactory
                .CreateSkillNpRuntimeTree(args.Unit, unitESkillConfig.NPBehaveId, unitESkillConfig.BelongToSkillId)
                .Start();

            await ETTask.CompletedTask;
        }
    }
}