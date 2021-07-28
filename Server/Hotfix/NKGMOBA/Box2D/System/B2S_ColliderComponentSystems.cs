//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月4日 16:31:14
//------------------------------------------------------------

using Box2DSharp.Common;
using ETModel;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace ETHotfix
{
    [ObjectSystem]
    public class B2S_ColliderComponentAwakeSystem: AwakeSystem<B2S_ColliderComponent, Unit, int>
    {
        public override void Awake(B2S_ColliderComponent self, Unit belongToUnit, int colliderDataConfigId)
        {
            self.BelongToUnit = belongToUnit;
            self.B2S_ColliderDataConfigId = colliderDataConfigId;
            self.Sync = Game.Scene.GetComponent<ConfigComponent>().Get<Server_B2SColliderConfig>(self.B2S_ColliderDataConfigId).SyncToUnit;
            LoadDependenceRes(self);
        }

        /// <summary>
        /// 加载依赖数据，并且进行碰撞体的生成
        /// </summary>
        /// <param name="self"></param>
        private void LoadDependenceRes(B2S_ColliderComponent self)
        {
            B2S_ColliderDataRepositoryComponent b2SColliderDataRepositoryComponent =
                    Game.Scene.GetComponent<B2S_ColliderDataRepositoryComponent>();

            self.Entity.AddComponent<B2S_CollisionResponseComponent>();

            self.B2S_ColliderDataStructureBase = b2SColliderDataRepositoryComponent.GetDataByColliderId(Game.Scene.GetComponent<ConfigComponent>()
                    .Get<Server_B2SColliderConfig>(self.B2S_ColliderDataConfigId).B2S_ColliderId);
            self.Body = B2S_BodyUtility.CreateDynamicBody(self.BelongToUnit);

            switch (self.B2S_ColliderDataStructureBase.b2SColliderType)
            {
                case B2S_ColliderType.BoxColllider:
                    B2S_BoxColliderDataStructure b2SBoxColliderDataStructure = (B2S_BoxColliderDataStructure) self.B2S_ColliderDataStructureBase;
                    self.Body.CreateBoxFixture(b2SBoxColliderDataStructure.hx, b2SBoxColliderDataStructure.hy,
                        b2SBoxColliderDataStructure.finalOffset, 0, b2SBoxColliderDataStructure.isSensor, self.Entity);
                    break;
                case B2S_ColliderType.CircleCollider:
                    B2S_CircleColliderDataStructure b2SCircleColliderDataStructure =
                            (B2S_CircleColliderDataStructure) self.B2S_ColliderDataStructureBase;
                    self.Body.CreateCircleFixture(b2SCircleColliderDataStructure.radius, b2SCircleColliderDataStructure.finalOffset,
                        b2SCircleColliderDataStructure.isSensor,
                        self.Entity);
                    break;
                case B2S_ColliderType.PolygonCollider:
                    B2S_PolygonColliderDataStructure b2SPolygonColliderDataStructure =
                            (B2S_PolygonColliderDataStructure) self.B2S_ColliderDataStructureBase;
                    foreach (var verxtPoint in b2SPolygonColliderDataStructure.finalPoints)
                    {
                        self.Body.CreatePolygonFixture(verxtPoint, b2SPolygonColliderDataStructure.isSensor, self.Entity);
                    }

                    break;
            }
        }
    }

    [ObjectSystem]
    public class B2S_HeroColliderDataFixedUpdateSystem: FixedUpdateSystem<B2S_ColliderComponent>
    {
        public override void FixedUpdate(B2S_ColliderComponent self)
        {
            //如果刚体处于激活状态，且设定上此刚体是跟随Unit的话，就同步位置和角度
            if (self.Body.IsEnabled && self.Sync && !self.BelongToUnit.TempScene.GetComponent<B2S_WorldComponent>().GetWorld().IsLocked)
            {
                self.SyncBody();
                //Log.Info($"进行了位置移动，数据结点为{self.ID}");
            }
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
            self.SetColliderBodyPos(new Vector2(self.BelongToUnit.Position.x, self.BelongToUnit.Position.z));
            self.SetColliderBodyAngle(-Quaternion.QuaternionToEuler(self.BelongToUnit.Rotation).y * Settings.Pi / 180);
        }

        /// <summary>
        /// 设置刚体位置
        /// </summary>
        /// <param name="self"></param>
        /// <param name="pos"></param>
        public static void SetColliderBodyPos(this B2S_ColliderComponent self, Vector2 pos)
        {
            (self.Entity as Unit).Position = new Vector3(pos.X, pos.Y, 0);
            self.Body.SetTransform(pos, self.Body.GetAngle());
            //Log.Info($"位置为{pos}");
        }

        /// <summary>
        /// 设置刚体角度
        /// </summary>
        /// <param name="self"></param>
        /// <param name="angle"></param>
        public static void SetColliderBodyAngle(this B2S_ColliderComponent self, float angle)
        {
            (self.Entity as Unit).Rotation = Quaternion.Euler(0, angle, 0);
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