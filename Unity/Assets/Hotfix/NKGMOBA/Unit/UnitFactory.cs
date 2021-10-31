using UnityEngine;

namespace ET
{
    public static class UnitFactory
    {
        public static Unit CreateUnit(Room room, long id, int configId)
        {
            UnitComponent unitComponent = room.GetComponent<UnitComponent>();
            
            Unit unit = unitComponent.AddChildWithId<Unit, int>(id, configId);
            unit.BelongToRoom = room;
            
            unitComponent.Add(unit);

            return unit;
        }

        public static Unit CreateHero(Room room, UnitInfo unitInfo)
        {
            Unit unit = CreateUnit(room, unitInfo.UnitId, unitInfo.ConfigId);
            unit.Position = new Vector3(unitInfo.X, unitInfo.Y, unitInfo.Z);

            unit.AddComponent<DataModifierComponent>();
            unit.AddComponent<NumericComponent>();
            //增加栈式状态机，辅助动画切换
            unit.AddComponent<StackFsmComponent>();
            unit.AddComponent<MoveComponent>();

            //增加Buff管理组件
            unit.AddComponent<BuffManagerComponent>();
            unit.AddComponent<SkillCanvasManagerComponent>();
            unit.AddComponent<B2S_RoleCastComponent, RoleCamp>((RoleCamp) unitInfo.RoleCamp);

            unit.AddComponent<NP_RuntimeTreeManager>();
            //Log.Info("行为树创建完成");
            unit.AddComponent<ObjectWait>();
            unit.AddComponent<LSF_TickComponent>();


            Game.EventSystem.Publish(new EventType.AfterHeroCreate_CreateGo()
                {Unit = unit, HeroConfigId = unitInfo.ConfigId}).Coroutine();
            return unit;
        }
        
        public static Unit CreateHeroSpiling(Room room, UnitInfo unitInfo)
        {
            Unit unit = CreateUnit(room, unitInfo.UnitId, unitInfo.ConfigId);
            unit.Position = new Vector3(unitInfo.X, unitInfo.Y, unitInfo.Z);

            unit.AddComponent<DataModifierComponent>();
            unit.AddComponent<NumericComponent>();
            //增加栈式状态机，辅助动画切换
            unit.AddComponent<StackFsmComponent>();
            unit.AddComponent<MoveComponent>();

            //增加Buff管理组件
            unit.AddComponent<BuffManagerComponent>();
            unit.AddComponent<SkillCanvasManagerComponent>();
            unit.AddComponent<B2S_RoleCastComponent, RoleCamp>((RoleCamp) unitInfo.RoleCamp);

            unit.AddComponent<NP_RuntimeTreeManager>();
            //Log.Info("行为树创建完成");
            unit.AddComponent<ObjectWait>();


            Game.EventSystem.Publish(new EventType.AfterHeroSpilingCreate_CreateGO()
                {Unit = unit, HeroSpilingConfigId = unitInfo.ConfigId}).Coroutine();
            return unit;
        }
    }
}