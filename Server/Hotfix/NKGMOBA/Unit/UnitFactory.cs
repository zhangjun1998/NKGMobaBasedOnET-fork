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
        public static Unit CreateUnit(Room room)
        {
            Unit unit = room.AddChild<Unit, Room>(room);
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
        public static Unit CreateUnit(Room room, UnitInfo unitInfo)
        {
            Unit unit = room.AddChildWithId<Unit, Room, int>(unitInfo.UnitId, room, unitInfo.ConfigId);

            unit.Position = new Vector3(unitInfo.X, unitInfo.Y, unitInfo.Z);
            unit.Rotation = Quaternion.identity;

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
        public static Unit CreateHeroUnit(Room room, UnitInfo unitInfo)
        {
            Unit unit = CreateUnit(room, unitInfo);
            unit.NeedSyncToClient = true;

            // 由于玩家操控的英雄是同步的关键，所以只需要为其添加MailBoxComponent来让Actor机制可以索引到
            unit.AddComponent<MailBoxComponent>();

            unit.AddComponent<B2S_RoleCastComponent, RoleCamp, RoleTag>((RoleCamp) unitInfo.RoleCamp, RoleTag.Hero);

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
            unit.AddComponent<CommonAttackComponent_Logic>();

            //unit.AddComponent<OperatesComponent>();

            unit.AddComponent<BuffManagerComponent>();
            unit.AddComponent<NP_RuntimeTreeManager>();
            unit.AddComponent<SkillCanvasManagerComponent>();

            //Log.Info("开始创建行为树");
            NP_RuntimeTreeFactory.CreateSkillNpRuntimeTree(unit,
                    SkillCanvasConfigCategory.Instance.Get(serverUnitConfig.UnitPassiveSkillId).NPBehaveId,
                    SkillCanvasConfigCategory.Instance.Get(serverUnitConfig.UnitPassiveSkillId).BelongToSkillId)
                .Start();

            NP_RuntimeTreeFactory.CreateSkillNpRuntimeTree(unit,
                    SkillCanvasConfigCategory.Instance.Get(serverUnitConfig.UnitQSkillId).NPBehaveId,
                    SkillCanvasConfigCategory.Instance.Get(serverUnitConfig.UnitQSkillId).BelongToSkillId)
                .Start();

            NP_RuntimeTreeFactory.CreateSkillNpRuntimeTree(unit,
                    SkillCanvasConfigCategory.Instance.Get(serverUnitConfig.UnitWSkillId).NPBehaveId,
                    SkillCanvasConfigCategory.Instance.Get(serverUnitConfig.UnitWSkillId).BelongToSkillId)
                .Start();

            NP_RuntimeTreeFactory.CreateSkillNpRuntimeTree(unit,
                    SkillCanvasConfigCategory.Instance.Get(serverUnitConfig.UnitESkillId).NPBehaveId,
                    SkillCanvasConfigCategory.Instance.Get(serverUnitConfig.UnitESkillId).BelongToSkillId)
                .Start();

            SkillCanvasConfig Test =
                SkillCanvasConfigCategory.Instance.Get(serverUnitConfig.UnitRSkillId);

            NP_RuntimeTreeFactory
                .CreateSkillNpRuntimeTree(unit, Test.NPBehaveId,
                    SkillCanvasConfigCategory.Instance.Get(serverUnitConfig.UnitRSkillId).BelongToSkillId)
                .Start();


            unit.AddComponent<LSF_TickComponent>();

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
        public static Unit CreateHeroSpilingUnit(Room room, UnitInfo unitInfo)
        {
            Unit unit = CreateUnit(room, unitInfo);

            unit.AddComponent<B2S_RoleCastComponent, RoleCamp, RoleTag>((RoleCamp) unitInfo.RoleCamp, RoleTag.Hero);

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
            unit.AddComponent<CommonAttackComponent_Logic>();

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
        public static Unit CreateSpecialColliderUnit(Room room, Unit belongToUnit, int colliderDataConfigId,
            int collisionRelationDataConfigId)
        {
            //为碰撞体新建一个Unit
            Unit b2sColliderEntity = CreateUnit(room);
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