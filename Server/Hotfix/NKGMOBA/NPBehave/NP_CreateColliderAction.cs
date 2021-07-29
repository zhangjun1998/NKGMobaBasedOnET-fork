//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月29日 12:29:40
//------------------------------------------------------------

using System.Numerics;
using Box2DSharp.Collision.Shapes;
using ETHotfix;
using ETHotfix.NKGMOBA.Factory;
using ETModel;

namespace EThotfix
{
    [Event(EventIdType.CreateCollider)]
    public class NP_CreateColliderAction: AEvent<long, int, int>
    {
        /// <summary>
        /// 创建一次一定要同步一次
        /// </summary>
        /// <param name="casterUnitId">释放者unit Id</param>
        /// <param name="b2SCollisionRelationConfigId">碰撞体碰撞关系数据Id（Excel表中的）</param>
        /// <param name="npBehaveConfigId">目标行为树数据载体Id (Excel表中的)</param>
        public override void Run(long casterUnitId, int b2SCollisionRelationConfigId, int npBehaveConfigId)
        {
            Unit unit = UnitComponent.Instance.Get(casterUnitId);
            ConfigComponent configComponent = Game.Scene.GetComponent<ConfigComponent>();

            B2S_ColliderComponent colliderComponent = UnitFactory
                    .CreateSpecialColliderUnit(unit,
                        configComponent.Get<Server_B2SCollisionRelationConfig>(b2SCollisionRelationConfigId).B2S_ColliderConfigId,
                        b2SCollisionRelationConfigId)
                    .GetComponent<B2S_ColliderComponent>();

            //这里直接默认以英雄当前位置作为碰撞体生成的位置，如需提前指定位置，请在抛事件那里传参
            colliderComponent.SyncBody();
            //Log.Info("生成技能碰撞体");

            //根据传过来的行为树Id来给这个碰撞Unit加上行为树
            NP_RuntimeTreeFactory.CreateSkillNpRuntimeTree(colliderComponent.Entity as Unit,
                        configComponent.Get<Server_SkillCanvasConfig>(npBehaveConfigId).NPBehaveId,
                        configComponent.Get<Server_SkillCanvasConfig>(npBehaveConfigId).BelongToSkillId)
                    .Start();

            //下面这一部分是Debug用的，稳定后请去掉
            {
                //广播碰撞体信息
                foreach (var VARIABLE in colliderComponent.Body.FixtureList)
                {
                    switch (VARIABLE.ShapeType)
                    {
                        case ShapeType.Polygon: //多边形
                            M2C_B2S_Debugger_Polygon test = new M2C_B2S_Debugger_Polygon() { Id = unit.Id, SustainTime = 2000, };
                            foreach (var VARIABLE1 in ((PolygonShape) VARIABLE.Shape).Vertices)
                            {
                                Vector2 worldPoint = colliderComponent.Body.GetWorldPoint(VARIABLE1);
                                test.Vects.Add(new M2C_B2S_VectorBase() { X = worldPoint.X, Y = worldPoint.Y });
                            }

                            MessageHelper.Broadcast(unit.GetParent<RoomPlayerComponent>().PlayerArray, test);
                            break;
                        case ShapeType.Circle: //圆形
                            CircleShape myShape = (CircleShape) VARIABLE.Shape;
                            M2C_B2S_Debugger_Circle test1 = new M2C_B2S_Debugger_Circle()
                            {
                                Id = unit.Id,
                                SustainTime = 2000,
                                Radius = myShape.Radius,
                                Pos = new M2C_B2S_VectorBase()
                                {
                                    X = colliderComponent.Body.GetWorldPoint(myShape.Position).X,
                                    Y = colliderComponent.Body.GetWorldPoint(myShape.Position).Y
                                },
                            };
                            MessageHelper.Broadcast(unit.GetParent<RoomPlayerComponent>().PlayerArray,test1);
                            //Log.Info($"是圆形，并且已经朝客户端发送绘制数据,半径为{myShape.Radius}");
                            break;
                    }
                }
            }
        }
    }
}