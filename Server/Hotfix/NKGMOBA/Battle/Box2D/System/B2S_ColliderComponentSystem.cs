﻿//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月4日 16:31:14
//------------------------------------------------------------

using Box2DSharp.Common;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace ET
{
    /// <summary>
    /// 通过数据表中的Id进行初始化，用于我们自定义的，在配置表中配置的碰撞体
    /// </summary>
    public class B2S_ColliderComponentAwakeSystem : AwakeSystem<B2S_ColliderComponent, Unit, int>
    {
        public override void Awake(B2S_ColliderComponent self, Unit belongToUnit, int collisionRelationDataConfigId)
        {
            Server_B2SCollisionRelationConfig serverB2SCollisionRelationConfig =
                Server_B2SCollisionRelationConfigCategory.Instance
                    .Get(collisionRelationDataConfigId);
            string collisionHandlerName = serverB2SCollisionRelationConfig.B2S_ColliderHandlerName;
            
            self.WorldComponent = self.Domain.GetComponent<B2S_WorldComponent>();
            self.BelongToUnit = belongToUnit;
            self.B2S_CollisionRelationConfigId = serverB2SCollisionRelationConfig.Id;
            self.B2S_ColliderDataConfigId = serverB2SCollisionRelationConfig.B2S_ColliderConfigId;
            self.CollisionHandlerName = collisionHandlerName;

            //先默认为True，即每帧碰撞体都会跟随
            self.Sync = true;

            LoadDependenceRes(self);
            
            self.SyncBody();
        }

        /// <summary>
        /// 加载依赖数据，并且进行碰撞体的生成
        /// </summary>
        /// <param name="self"></param>
        private void LoadDependenceRes(B2S_ColliderComponent self)
        {
            B2S_ColliderDataRepositoryComponent b2SColliderDataRepositoryComponent =
                self.DomainScene().GetComponent<B2S_ColliderDataRepositoryComponent>();

            self.B2S_ColliderDataStructureBase = b2SColliderDataRepositoryComponent.GetDataByColliderId(
                Server_B2SColliderConfigCategory.Instance.Get(self.B2S_ColliderDataConfigId).B2S_ColliderId);

            self.Body = self.WorldComponent.CreateDynamicBody();

            switch (self.B2S_ColliderDataStructureBase.b2SColliderType)
            {
                case B2S_ColliderType.BoxColllider:
                    B2S_BoxColliderDataStructure b2SBoxColliderDataStructure =
                        (B2S_BoxColliderDataStructure) self.B2S_ColliderDataStructureBase;
                    self.Body.CreateBoxFixture(b2SBoxColliderDataStructure.hx, b2SBoxColliderDataStructure.hy,
                        b2SBoxColliderDataStructure.finalOffset, 0, b2SBoxColliderDataStructure.isSensor, self.Parent);
                    break;
                case B2S_ColliderType.CircleCollider:
                    B2S_CircleColliderDataStructure b2SCircleColliderDataStructure =
                        (B2S_CircleColliderDataStructure) self.B2S_ColliderDataStructureBase;
                    Log.Info($"finalOffset {b2SCircleColliderDataStructure.finalOffset.ToString()} radius {b2SCircleColliderDataStructure.radius}");
                    self.Body.CreateCircleFixture(b2SCircleColliderDataStructure.radius,
                        b2SCircleColliderDataStructure.finalOffset,
                        b2SCircleColliderDataStructure.isSensor,
                        self.Parent);
                    break;
                case B2S_ColliderType.PolygonCollider:
                    B2S_PolygonColliderDataStructure b2SPolygonColliderDataStructure =
                        (B2S_PolygonColliderDataStructure) self.B2S_ColliderDataStructureBase;
                    foreach (var verxtPoint in b2SPolygonColliderDataStructure.finalPoints)
                    {
                        self.Body.CreatePolygonFixture(verxtPoint, b2SPolygonColliderDataStructure.isSensor,
                            self.Parent);
                    }

                    break;
            }
        }
    }


    /// <summary>
    /// 直接传递碰撞体数据进行初始化，用于我们的场景碰撞初始化
    /// </summary>
    public class
        B2S_ColliderComponentAwakeSystem1 : AwakeSystem<B2S_ColliderComponent, UnitFactory.CreateHeroColliderArgs>
    {
        public override void Awake(B2S_ColliderComponent self, UnitFactory.CreateHeroColliderArgs args)
        {
            self.WorldComponent = self.Domain.GetComponent<B2S_WorldComponent>();
            self.BelongToUnit = args.Unit;
            self.Sync = args.FollowUnit;
            self.CollisionHandlerName = args.CollisionHandler;
            self.B2S_ColliderDataStructureBase = args.B2SColliderDataStructureBase;
            LoadMapColliderDependenceRes(self);

            self.SyncBody();
        }

        /// <summary>
        /// 加载依赖数据，并且进行碰撞体的生成，注意这里是特殊的处理，因为我们将场景碰撞的位置存在了finalOffset中，而这个finalOffset是用于控制碰撞图形相对于刚体的位置的，会影响旋转行为
        /// 所以这里finalOffset被用在了外部Unit.Pos赋值上，这里传0
        /// </summary>
        /// <param name="self"></param>
        private void LoadMapColliderDependenceRes(B2S_ColliderComponent self)
        {
            self.Body = self.WorldComponent.CreateDynamicBody();

            switch (self.B2S_ColliderDataStructureBase.b2SColliderType)
            {
                case B2S_ColliderType.BoxColllider:
                    B2S_BoxColliderDataStructure b2SBoxColliderDataStructure =
                        (B2S_BoxColliderDataStructure) self.B2S_ColliderDataStructureBase;
                    self.Body.CreateBoxFixture(b2SBoxColliderDataStructure.hx, b2SBoxColliderDataStructure.hy,
                        Vector2.Zero, 0, b2SBoxColliderDataStructure.isSensor, self.Parent);
                    break;
                case B2S_ColliderType.CircleCollider:
                    B2S_CircleColliderDataStructure b2SCircleColliderDataStructure =
                        (B2S_CircleColliderDataStructure) self.B2S_ColliderDataStructureBase;
                    self.Body.CreateCircleFixture(b2SCircleColliderDataStructure.radius,
                        Vector2.Zero,
                        b2SCircleColliderDataStructure.isSensor,
                        self.Parent);
                    break;
                case B2S_ColliderType.PolygonCollider:
                    B2S_PolygonColliderDataStructure b2SPolygonColliderDataStructure =
                        (B2S_PolygonColliderDataStructure) self.B2S_ColliderDataStructureBase;
                    foreach (var verxtPoint in b2SPolygonColliderDataStructure.finalPoints)
                    {
                        self.Body.CreatePolygonFixture(verxtPoint, b2SPolygonColliderDataStructure.isSensor,
                            self.Parent);
                    }

                    break;
            }
        }
    }

    public class B2S_HeroColliderDataFixedUpdateSystem : FixedUpdateSystem<B2S_ColliderComponent>
    {
        public override void FixedUpdate(B2S_ColliderComponent self)
        {
            //如果刚体处于激活状态，且设定上此刚体是跟随Unit的话，就同步位置和角度
            if (self.Body.IsEnabled && self.Sync && !self.WorldComponent.GetWorld().IsLocked)
            {
                self.SyncBody();
                //Log.Info($"进行了位置移动");
            }
        }
    }

    public class B2S_HeroColliderDataDestroySystem : DestroySystem<B2S_ColliderComponent>
    {
        public override void Destroy(B2S_ColliderComponent self)
        {
            self.WorldComponent.AddBodyTobeDestroyed(self.Body);

            self.Body = null;
            self.B2S_ColliderDataStructureBase = null;
        }
    }


    public static class B2S_HeroColliderComponentHelper
    {
        /// <summary>
        /// 同步刚体（依据归属Unit）
        /// </summary>
        /// <param name="self"></param>
        /// <param name="pos"></param>
        public static void SyncBody(this B2S_ColliderComponent self)
        {
            //Log.Info($"{new Vector2(self.BelongToUnit.Position.x, self.BelongToUnit.Position.z)}");
            self.SetColliderBodyPos(new Vector2(self.GetParent<Unit>().Position.x, self.GetParent<Unit>().Position.z));
            self.SetColliderBodyAngle(Quaternion.QuaternionToEuler(self.BelongToUnit.Rotation).y * Settings.Pi / 180);
        }

        /// <summary>
        /// 设置刚体位置
        /// </summary>
        /// <param name="self"></param>
        /// <param name="pos"></param>
        public static void SetColliderBodyPos(this B2S_ColliderComponent self, Vector2 pos)
        {
            self.Body.SetTransform(pos, self.Body.GetAngle());
            //Log.Info($"位置为{self.Body.GetPosition()} 类型为{self.Body.IsSleepingAllowed}");
        }

        /// <summary>
        /// 设置刚体角度
        /// </summary>
        /// <param name="self"></param>
        /// <param name="angle"></param>
        public static void SetColliderBodyAngle(this B2S_ColliderComponent self, float angle)
        {
            self.Body.SetTransform(self.Body.GetPosition(), angle);
        }

        /// <summary>
        /// 设置刚体状态
        /// </summary>
        /// <param name="self"></param>
        /// <param name="state"></param>
        public static void SetColliderBodyState(this B2S_ColliderComponent self, bool state)
        {
            self.Body.IsEnabled = state;
        }
    }
}