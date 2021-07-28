//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年9月27日 10:31:04
//------------------------------------------------------------

using ETModel;
using ETModel.NKGMOBA.Battle.State;
using UnityEngine;

namespace ETHotfix.NKGMOBA.Factory
{
    public class UnitFactory
    {
        #region base

        /// <summary>
        /// 随机Id，不可以id和InstanceId一致，不然就会锁死
        /// </summary>
        /// <returns></returns>
        private static Unit CreateUnitBase()
        {
            Unit result = CreateUnitWithIdBase(IdGenerater.GenerateId());
            return result;
        }

        /// <summary>
        /// 指定Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static Unit CreateUnitWithIdBase(long id)
        {
            Unit result = ComponentFactory.CreateWithId<Unit>(id);
            UnitComponent.Instance.Add(result);
            return result;
        }

        #endregion

        #region 高级Unit相关（英雄，野怪，假人）

        /// <summary>
        /// 创建诺手
        /// TODO 后期需要改，应该是一个通用的创建英雄接口，只需要提供Id，然后自动索引其所需要的数据（比如技能数据，英雄数据）
        /// </summary>
        /// <returns></returns>
        public static Unit CreateDarius(Unit unit)
        {
            //创建战斗单位
            unit.AddComponent<ChildrenUnitComponent>();

            //增加寻路相关组件
            unit.AddComponent<UnitPathComponent>();

            ConfigComponent configComponent = Game.Scene.GetComponent<ConfigComponent>();

            Server_UnitConfig serverUnitConfig = configComponent.Get<Server_UnitConfig>(10001);
            //为Unit附加碰撞体
            CreateColliderUnit(unit, serverUnitConfig.UnitColliderDataConfigId);

            //unit.AddComponent<B2S_RoleCastComponent>();

            //Game.Scene.GetComponent<CampAllocManagerComponent>().AllocRoleCamp(unit);

            unit.AddComponent<NumericComponent>();
            unit.AddComponent<UnitAttributesDataComponent, long>(10001);
            unit.AddComponent<ReceiveDamageComponent>();
            unit.AddComponent<CastDamageComponent>();
            unit.AddComponent<DataModifierComponent>();
            //增加移动组件
            unit.AddComponent<MoveComponent>();

            unit.AddComponent<OperatesComponent>();

            unit.AddComponent<BuffManagerComponent>();
            unit.AddComponent<NP_RuntimeTreeManager>();
            unit.AddComponent<SkillCanvasManagerComponent>();

            //Log.Info("开始创建行为树");
            NP_RuntimeTreeFactory.CreateSkillNpRuntimeTree(unit,
                        configComponent.Get<Server_SkillCanvasConfig>(serverUnitConfig.UnitPassiveSkillId).NPBehaveId,
                        configComponent.Get<Server_SkillCanvasConfig>(serverUnitConfig.UnitPassiveSkillId).BelongToSkillId)
                    .Start();

            NP_RuntimeTreeFactory.CreateSkillNpRuntimeTree(unit,
                        configComponent.Get<Server_SkillCanvasConfig>(serverUnitConfig.UnitQSkillId).NPBehaveId,
                        configComponent.Get<Server_SkillCanvasConfig>(serverUnitConfig.UnitQSkillId).BelongToSkillId)
                    .Start();

            NP_RuntimeTreeFactory.CreateSkillNpRuntimeTree(unit,
                        configComponent.Get<Server_SkillCanvasConfig>(serverUnitConfig.UnitWSkillId).NPBehaveId,
                        configComponent.Get<Server_SkillCanvasConfig>(serverUnitConfig.UnitWSkillId).BelongToSkillId)
                    .Start();

            NP_RuntimeTreeFactory.CreateSkillNpRuntimeTree(unit,
                        configComponent.Get<Server_SkillCanvasConfig>(serverUnitConfig.UnitESkillId).NPBehaveId,
                        configComponent.Get<Server_SkillCanvasConfig>(serverUnitConfig.UnitESkillId).BelongToSkillId)
                    .Start();

            //Log.Info("行为树创建完成");

            //添加栈式状态机组件
            unit.AddComponent<StackFsmComponent>();
            unit.AddComponent<CommonAttackComponent>();

            //设置英雄位置
            unit.Position = new Vector3(-10, 0, -10);
            return unit;
        }

        /// <summary>
        /// 创建假人,需要传入一个父UnitId
        /// </summary>
        /// <returns></returns>
        public static Unit CreateSpiling(Unit parentUnit)
        {
            Unit unit = CreateUnitBase();
            //Log.Info($"服务端响应木桩请求，父id为{message.ParentUnitId}");
            parentUnit.GetComponent<ChildrenUnitComponent>().AddUnit(unit);
            //Log.Info("确认找到了请求的父实体");

            //为Unit附加碰撞体
            CreateColliderUnit(unit, Game.Scene.GetComponent<ConfigComponent>().Get<Server_UnitConfig>(10001).UnitColliderDataConfigId);

            unit.AddComponent<NumericComponent>();
            unit.AddComponent<UnitAttributesDataComponent, long>(10001);
            unit.AddComponent<ReceiveDamageComponent>();
            unit.AddComponent<CastDamageComponent>();
            unit.AddComponent<DataModifierComponent>();

            unit.AddComponent<OperatesComponent>();

            //增加移动组件
            unit.AddComponent<MoveComponent>();
            unit.AddComponent<BuffManagerComponent>();
            unit.AddComponent<B2S_RoleCastComponent>();
            parentUnit.TempScene.GetComponent<CampAllocManagerComponent>().AllocRoleCamp(unit);
            //Game.Scene.GetComponent<CampAllocManagerComponent>().AllocRoleCamp(unit);

            //添加栈式状态机组件
            unit.AddComponent<StackFsmComponent>();
            unit.AddComponent<UnitPathComponent>();

            unit.AddComponent<NP_RuntimeTreeManager>();

            ConfigComponent configComponent = Game.Scene.GetComponent<ConfigComponent>();
            // if (RandomHelper.RandomNumber(0, 2) == 0)
            //         //Log.Info("开始创建行为树");
            NP_RuntimeTreeFactory.CreateNpRuntimeTree(unit, configComponent.Get<Server_AICanvasConfig>(10001).NPBehaveId)
                    .Start();

            return unit;
        }

        #endregion

        #region 碰撞体Unit

        /// <summary>
        /// 创建特殊碰撞体，会与其他碰撞体产生碰撞回调函数
        /// </summary>
        /// <param name="belongToUnit">归属的Unit（施法者Unit）</param>
        /// <param name="colliderDataConfigId">碰撞体数据配置Id（B2S_ColliderDataConfig Excel表中Id）</param>
        /// <param name="collisionRelationDataConfigId">碰撞关系数据配置Id（B2S_CollisionDataConfig Excel表中Id）</param>
        /// <returns></returns>
        public static Unit CreateSpecialColliderUnit(Unit belongToUnit, int colliderDataConfigId, int collisionRelationDataConfigId)
        {
            //为碰撞体新建一个Unit
            Unit b2sColliderEntity = CreateColliderUnit(belongToUnit, colliderDataConfigId);
            b2sColliderEntity.AddComponent<NP_RuntimeTreeManager>();
            b2sColliderEntity.AddComponent<SkillCanvasManagerComponent>();

            ConfigComponent configComponent = Game.Scene.GetComponent<ConfigComponent>();
            //添加注册碰撞回调
            Game.EventSystem.Run(
                $"B2S_{configComponent.Get<Server_B2SCollisionRelationConfig>(collisionRelationDataConfigId).B2S_ColliderHandlerName}_CRS",
                b2sColliderEntity);

            return b2sColliderEntity;
        }

        /// <summary>
        /// 创建碰撞体
        /// </summary>
        /// <param name="belongToUnit">归属的Unit（施法者Unit）</param>
        /// <param name="colliderDataConfigId">碰撞体数据配置Id（B2S_ColliderDataConfig Excel表中Id）</param>
        /// <returns></returns>
        public static Unit CreateColliderUnit(Unit belongToUnit, int colliderDataConfigId)
        {
            //为碰撞体新建一个Unit
            Unit b2sColliderEntity = CreateUnitBase();
            b2sColliderEntity.Parent = belongToUnit.Parent;
            b2sColliderEntity.AddComponent<B2S_ColliderComponent, Unit, int>(belongToUnit, colliderDataConfigId);
            //把这个碰撞实体增加到管理者维护 TODO 待优化，目的同B2S_ColliderEntityManagerComponent
            belongToUnit.TempScene.GetComponent<B2S_WorldColliderManagerComponent>().AddColliderEntity(b2sColliderEntity);
            return b2sColliderEntity;
        }

        /// <summary>
        /// 移除一个ColliderUnit
        /// </summary>
        /// <param name="id"></param>
        public static void RemoveColliderUnit(long id)
        {
            UnitComponent.Instance.Get(id).TempScene.GetComponent<B2S_WorldColliderManagerComponent>().RemoveColliderEntity(id);
            //Game.Scene.GetComponent<B2S_WorldColliderManagerComponent>().RemoveColliderEntity(id);
            UnitComponent.Instance.Remove(id);
            //Log.Error("移除碰撞体");
        }

        #endregion
    }
}