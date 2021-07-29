//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月3日 19:10:37
//------------------------------------------------------------

using System.Collections.Generic;
using System.Numerics;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using ETModel;

namespace ETHotfix
{
    public static class PlayerInput_SkillCmdSystem
    {
        public static void BroadcastSkillCmd(Unit unit, string skillCmd)
        {
            M2C_UserInput_SkillCmd m2CUserInputSkillCmd = new M2C_UserInput_SkillCmd() { Message = skillCmd, Id = unit.Id };

            MessageHelper.Broadcast(unit.GetParent<RoomPlayerComponent>().PlayerArray,m2CUserInputSkillCmd);
        }

        /// <summary>
        /// 广播指令和碰撞体数据（Debug用，正式上线后请使用上面那个）
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="colliderComponent"></param>
        /// <param name="skillCmd"></param>
        public static void BroadcastB2S_ColliderData(Unit unit, B2S_ColliderComponent colliderComponent)
        {
            colliderComponent.SyncBody();

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
                        MessageHelper.Broadcast(unit.GetParent<RoomPlayerComponent>().PlayerArray, test1);
                        //Log.Info($"是圆形，并且已经朝客户端发送绘制数据,半径为{myShape.Radius}");
                        break;
                }
            }
        }
    }
}