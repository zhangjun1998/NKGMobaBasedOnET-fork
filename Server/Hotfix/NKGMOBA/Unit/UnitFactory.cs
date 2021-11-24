﻿using UnityEngine;

namespace ET
{
    public class UnitFactory
    {
        #region Unit创建基础

        /// <summary>
        /// 手动创建Unit
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public static Unit CreateUnit(Entity entity)
        {
            var unitComponent = entity.Domain.GetComponent<UnitComponent>();
            Unit unit = unitComponent.AddChild<Unit>();
            return unit;
        }


        /// <summary>
        /// 根据配置表创建Unit
        /// </summary>
        /// <param name="room"></param>
        /// <param name="configId"></param>
        /// <param name="pos"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public static Unit CreateUnit(Scene scene, int configId, Vector3 pos, Quaternion rotation)
        {
            Unit unit = scene.GetComponent<UnitComponent>().AddChild<Unit, int>(configId);
            unit.Position = pos;
            unit.Rotation = rotation;

            return unit;
        }

        #endregion

        #region 创建英雄

        /// <summary>
        /// 根据配置表创建Unit
        /// </summary>
        /// <param name="room"></param>
        /// <param name="configId"></param>
        /// <param name="pos"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public static Unit CreateHeroUnit(Scene scene, int configId, RoleCamp roleCamp, Vector3 pos, Quaternion rotation)
        {
            Unit unit = CreateUnit(scene, configId, pos, rotation);

            // 由于玩家操控的英雄是同步的关键，所以只需要为其添加MailBoxComponent来让Actor机制可以索引到
            unit.AddComponent<MailBoxComponent>();

            unit.AddComponent<B2S_RoleCastComponent, RoleCamp, RoleTag>(roleCamp, RoleTag.Hero);

            CreateHeroColliderArgs createHeroColliderArgs = new CreateHeroColliderArgs()
            {
                Unit = unit, B2SColliderDataStructureBase = new B2S_CircleColliderDataStructure()
                {
                    b2SColliderType = B2S_ColliderType.CircleCollider, finalOffset = System.Numerics.Vector2.Zero,
                    id = IdGenerater.Instance.GenerateId(), isSensor = true, radius = 1
                },
                CollisionHandler = "", FollowUnit = true
            };

            unit.AddComponent<B2S_ColliderComponent, CreateHeroColliderArgs>(createHeroColliderArgs);

            Server_UnitConfig serverUnitConfig = Server_UnitConfigCategory.Instance.Get(10001);

            unit.AddComponent<NumericComponent>();
            unit.AddComponent<UnitAttributesDataComponent, long>(10001);
            unit.AddComponent<ReceiveDamageComponent>();
            unit.AddComponent<CastDamageComponent>();
            unit.AddComponent<DataModifierComponent>();
            //增加移动组件
            unit.AddComponent<MoveComponent>();
            //添加栈式状态机组件
            unit.AddComponent<StackFsmComponent>();
            unit.AddComponent<CommonAttackComponent>();

            //unit.AddComponent<OperatesComponent>();

            unit.AddComponent<BuffManagerComponent>();
            unit.AddComponent<NP_RuntimeTreeManager>();
            unit.AddComponent<SkillCanvasManagerComponent>();

            //Log.Info("开始创建行为树");
            NP_RuntimeTreeFactory.CreateSkillNpRuntimeTree(unit,
                    Server_SkillCanvasConfigCategory.Instance.Get(serverUnitConfig.UnitPassiveSkillId).NPBehaveId,
                    Server_SkillCanvasConfigCategory.Instance.Get(serverUnitConfig.UnitPassiveSkillId).BelongToSkillId)
                .Start();

            NP_RuntimeTreeFactory.CreateSkillNpRuntimeTree(unit,
                    Server_SkillCanvasConfigCategory.Instance.Get(serverUnitConfig.UnitQSkillId).NPBehaveId,
                    Server_SkillCanvasConfigCategory.Instance.Get(serverUnitConfig.UnitQSkillId).BelongToSkillId)
                .Start();

            NP_RuntimeTreeFactory.CreateSkillNpRuntimeTree(unit,
                    Server_SkillCanvasConfigCategory.Instance.Get(serverUnitConfig.UnitWSkillId).NPBehaveId,
                    Server_SkillCanvasConfigCategory.Instance.Get(serverUnitConfig.UnitWSkillId).BelongToSkillId)
                .Start();

            NP_RuntimeTreeFactory.CreateSkillNpRuntimeTree(unit,
                    Server_SkillCanvasConfigCategory.Instance.Get(serverUnitConfig.UnitESkillId).NPBehaveId,
                    Server_SkillCanvasConfigCategory.Instance.Get(serverUnitConfig.UnitESkillId).BelongToSkillId)
                .Start();

            return unit;
        }

        /// <summary>
        /// 根据配置表创建Unit
        /// </summary>
        /// <param name="room"></param>
        /// <param name="configId"></param>
        /// <param name="pos"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public static Unit CreateHeroSpilingUnit(Scene scene, int configId, RoleCamp roleCamp, Vector3 pos,
            Quaternion rotation)
        {
            Unit unit = CreateUnit(scene, configId, pos, rotation);

            unit.AddComponent<B2S_RoleCastComponent, RoleCamp, RoleTag>(roleCamp, RoleTag.Hero);

            CreateHeroColliderArgs createHeroColliderArgs = new CreateHeroColliderArgs()
            {
                Unit = unit, B2SColliderDataStructureBase = new B2S_CircleColliderDataStructure()
                {
                    b2SColliderType = B2S_ColliderType.CircleCollider, finalOffset = System.Numerics.Vector2.Zero,
                    id = IdGenerater.Instance.GenerateId(), isSensor = true, radius = 1
                },
                CollisionHandler = "", FollowUnit = true
            };

            unit.AddComponent<B2S_ColliderComponent, CreateHeroColliderArgs>(createHeroColliderArgs);

            Server_UnitConfig serverUnitConfig = Server_UnitConfigCategory.Instance.Get(10001);

            unit.AddComponent<NumericComponent>();
            unit.AddComponent<UnitAttributesDataComponent, long>(10001);
            unit.AddComponent<ReceiveDamageComponent>();
            unit.AddComponent<CastDamageComponent>();
            unit.AddComponent<DataModifierComponent>();
            //增加移动组件
            unit.AddComponent<MoveComponent>();
            //添加栈式状态机组件
            unit.AddComponent<StackFsmComponent>();
            unit.AddComponent<CommonAttackComponent>();

            //unit.AddComponent<OperatesComponent>();

            unit.AddComponent<BuffManagerComponent>();
            unit.AddComponent<NP_RuntimeTreeManager>();
            unit.AddComponent<SkillCanvasManagerComponent>();

            return unit;
        }

        #endregion

        #region 创建碰撞体

        public class CreateHeroColliderArgs
        {
            public Unit Unit;
            public B2S_ColliderDataStructureBase B2SColliderDataStructureBase;
            public string CollisionHandler;
            public bool FollowUnit;
        }

        /// <summary>
        /// 创建碰撞体
        /// </summary>
        /// <param name="room">归属的房间</param>
        /// <param name="belongToUnit">归属的Unit</param>
        /// <param name="colliderDataConfigId">碰撞体数据表Id</param>
        /// <param name="collisionRelationDataConfigId">碰撞关系数据表Id</param>
        /// <returns></returns>
        public static Unit CreateSpecialColliderUnit(Unit belongToUnit, int colliderDataConfigId,
            int collisionRelationDataConfigId)
        {
            //为碰撞体新建一个Unit
            Unit b2sColliderEntity = CreateUnit(belongToUnit.Domain);
            b2sColliderEntity.Position = belongToUnit.Position;

            b2sColliderEntity.AddComponent<B2S_RoleCastComponent, RoleCamp, RoleTag>(
                belongToUnit.GetComponent<B2S_RoleCastComponent>().RoleCamp, RoleTag.SkillCollision);

            b2sColliderEntity.AddComponent<B2S_ColliderComponent, Unit, int>(belongToUnit,
                collisionRelationDataConfigId);

            b2sColliderEntity.AddComponent<NP_RuntimeTreeManager>();
            b2sColliderEntity.AddComponent<SkillCanvasManagerComponent>();

            return b2sColliderEntity;
        }

        #endregion
    }
}