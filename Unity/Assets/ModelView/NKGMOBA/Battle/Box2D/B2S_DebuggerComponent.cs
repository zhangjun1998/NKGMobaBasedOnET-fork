//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月8日 21:11:46
//------------------------------------------------------------

using System.Collections.Generic;
using ET;
using UnityEngine;

namespace ET
{
#if !SERVER
    /// <summary>
    /// Box2D的碰撞体可视化Debugger组件
    /// </summary>
    public class B2S_DebuggerComponent : Entity
    {
        public Dictionary<Unit, B2S_DebuggerProcessor> AllLinerRendersDic =
            new Dictionary<Unit, B2S_DebuggerProcessor>();

        public Dictionary<Unit, Vector3[]> AllVexs = new Dictionary<Unit, Vector3[]>();

        public GameObject GoSupportor;

        public List<Unit> TobeRemovedProcessors = new List<Unit>();

        public B2S_DebuggerProcessor AddBox2dCollider(Unit colliderUnit)
        {
            B2S_DebuggerProcessor b2SDebuggerProcessor;
            if (AllLinerRendersDic.TryGetValue(colliderUnit, out b2SDebuggerProcessor))
            {
                return b2SDebuggerProcessor;
            }
            else
            {
                GameObject gameObject = new GameObject();
                b2SDebuggerProcessor = gameObject.AddComponent<B2S_DebuggerProcessor>();
                gameObject.transform.SetParent(GoSupportor.transform);
                B2S_ColliderComponent b2SColliderComponent = colliderUnit.GetComponent<B2S_ColliderComponent>();

                // 用于Transform变换的矩阵，方便计算缩放，旋转和平移
                Matrix4x4 transformMatrix4X4;
                Vector3[] finalVexs = null;
                switch (b2SColliderComponent.B2S_ColliderDataStructureBase)
                {
                    case B2S_BoxColliderDataStructure b2SBoxColliderDataStructure:
                        finalVexs = new Vector3[4];
                        Vector3 boxCenterPoint =
                            new Vector3(colliderUnit.Position.x + b2SBoxColliderDataStructure.finalOffset.X, 1,
                                colliderUnit.Position.z + b2SBoxColliderDataStructure.finalOffset.Y);

                        // TODO 因为暂时没有缩放碰撞体的需求，所以Scale矩阵为单位正交矩阵
                        transformMatrix4X4 = Matrix4x4.TRS(boxCenterPoint, colliderUnit.Rotation, Vector3.one);

                        finalVexs[0] = transformMatrix4X4.MultiplyPoint(new Vector3(-b2SBoxColliderDataStructure.hx, 1,
                            b2SBoxColliderDataStructure.hy));
                        finalVexs[1] = transformMatrix4X4.MultiplyPoint(new Vector3(b2SBoxColliderDataStructure.hx, 1,
                            b2SBoxColliderDataStructure.hy));
                        finalVexs[2] = transformMatrix4X4.MultiplyPoint(new Vector3(-b2SBoxColliderDataStructure.hx, 1,
                            -b2SBoxColliderDataStructure.hy));
                        finalVexs[3] = transformMatrix4X4.MultiplyPoint(new Vector3(b2SBoxColliderDataStructure.hx, 1,
                            -b2SBoxColliderDataStructure.hy));
                        break;
                    case B2S_CircleColliderDataStructure b2SCircleColliderDataStructure:
                        // TODO 暂时默认12个顶点绘制一个圆形
                        finalVexs = new Vector3[12];
                        Vector3 circleCenterPoint =
                            new Vector3(colliderUnit.Position.x + b2SCircleColliderDataStructure.finalOffset.X,
                                1, colliderUnit.Position.y + b2SCircleColliderDataStructure.finalOffset.Y);

                        // TODO 因为暂时没有缩放碰撞体的需求，所以Scale矩阵为单位正交矩阵
                        transformMatrix4X4 = Matrix4x4.TRS(circleCenterPoint, colliderUnit.Rotation, Vector3.one);

                        var step = Mathf.RoundToInt(360 / 12f);
                        for (int i = 0; i <= 360; i += step)
                        {
                            finalVexs[i / step] = transformMatrix4X4.MultiplyPoint(new Vector3(
                                b2SCircleColliderDataStructure.radius *
                                Mathf.Cos(i * 1.0f * Mathf.Deg2Rad), circleCenterPoint.y,
                                b2SCircleColliderDataStructure.radius *
                                Mathf.Sin(i * 1.0f * Mathf.Deg2Rad)));
                        }

                        break;

                    case B2S_PolygonColliderDataStructure b2SPolygonColliderDataStructure:
                        finalVexs = new Vector3[12];

                        Vector3 polygonCenterPoint =
                            new Vector3(colliderUnit.Position.x + b2SPolygonColliderDataStructure.finalOffset.X,
                                1, colliderUnit.Position.y + b2SPolygonColliderDataStructure.finalOffset.Y);

                        // TODO 因为暂时没有缩放碰撞体的需求，所以Scale矩阵为单位正交矩阵
                        transformMatrix4X4 = Matrix4x4.TRS(polygonCenterPoint, colliderUnit.Rotation, Vector3.one);

                        int index = 0;
                        for (int i = 0; i < b2SPolygonColliderDataStructure.finalPoints.Count; i++, index++)
                        {
                            for (int j = 0; j < b2SPolygonColliderDataStructure.finalPoints[i].Count; j++, index++)
                            {
                                finalVexs[index] = transformMatrix4X4.MultiplyPoint(new Vector3(
                                    b2SPolygonColliderDataStructure.finalPoints[i][j].X, 1,
                                    b2SPolygonColliderDataStructure.finalPoints[i][j].Y));
                            }

                            index--;
                        }

                        break;
                }

                AllVexs.Add(colliderUnit, finalVexs);
                b2SDebuggerProcessor.Init(finalVexs);
                AllLinerRendersDic.Add(colliderUnit, b2SDebuggerProcessor);
                return b2SDebuggerProcessor;
            }
        }

        public void RefreshBox2dDebugInfo(Unit colliderUnitToRefresh)
        {
            B2S_ColliderComponent b2SColliderComponent = colliderUnitToRefresh.GetComponent<B2S_ColliderComponent>();
            Vector3[] finalVexs = this.AllVexs[colliderUnitToRefresh];
            // 用于Transform变换的矩阵，方便计算缩放，旋转和平移
            Matrix4x4 transformMatrix4X4;
            switch (b2SColliderComponent.B2S_ColliderDataStructureBase)
            {
                case B2S_BoxColliderDataStructure b2SBoxColliderDataStructure:
                    Vector3 boxCenterPoint =
                        new Vector3(colliderUnitToRefresh.Position.x + b2SBoxColliderDataStructure.finalOffset.X, 1,
                            colliderUnitToRefresh.Position.z + b2SBoxColliderDataStructure.finalOffset.Y);

                    // TODO 因为暂时没有缩放碰撞体的需求，所以Scale矩阵为单位正交矩阵
                    transformMatrix4X4 = Matrix4x4.TRS(boxCenterPoint, colliderUnitToRefresh.Rotation, Vector3.one);

                    finalVexs[0] = transformMatrix4X4.MultiplyPoint(new Vector3(-b2SBoxColliderDataStructure.hx, 1,
                        b2SBoxColliderDataStructure.hy));
                    finalVexs[1] = transformMatrix4X4.MultiplyPoint(new Vector3(b2SBoxColliderDataStructure.hx, 1,
                        b2SBoxColliderDataStructure.hy));
                    finalVexs[2] = transformMatrix4X4.MultiplyPoint(new Vector3(-b2SBoxColliderDataStructure.hx, 1,
                        -b2SBoxColliderDataStructure.hy));
                    finalVexs[3] = transformMatrix4X4.MultiplyPoint(new Vector3(b2SBoxColliderDataStructure.hx, 1,
                        -b2SBoxColliderDataStructure.hy));
                    break;
                case B2S_CircleColliderDataStructure b2SCircleColliderDataStructure:
                    Vector3 circleCenterPoint =
                        new Vector3(colliderUnitToRefresh.Position.x + b2SCircleColliderDataStructure.finalOffset.X,
                            1, colliderUnitToRefresh.Position.y + b2SCircleColliderDataStructure.finalOffset.Y);

                    // TODO 因为暂时没有缩放碰撞体的需求，所以Scale矩阵为单位正交矩阵
                    transformMatrix4X4 = Matrix4x4.TRS(circleCenterPoint, colliderUnitToRefresh.Rotation, Vector3.one);

                    var step = Mathf.RoundToInt(360 / 12f);
                    for (int i = 0; i <= 360; i += step)
                    {
                        finalVexs[i / step] = transformMatrix4X4.MultiplyPoint(new Vector3(
                            b2SCircleColliderDataStructure.radius *
                            Mathf.Cos(i * 1.0f * Mathf.Deg2Rad), circleCenterPoint.y,
                            b2SCircleColliderDataStructure.radius *
                            Mathf.Sin(i * 1.0f * Mathf.Deg2Rad)));
                    }

                    break;

                case B2S_PolygonColliderDataStructure b2SPolygonColliderDataStructure:
                    Vector3 polygonCenterPoint =
                        new Vector3(colliderUnitToRefresh.Position.x + b2SPolygonColliderDataStructure.finalOffset.X,
                            1, colliderUnitToRefresh.Position.y + b2SPolygonColliderDataStructure.finalOffset.Y);

                    // TODO 因为暂时没有缩放碰撞体的需求，所以Scale矩阵为单位正交矩阵
                    transformMatrix4X4 = Matrix4x4.TRS(polygonCenterPoint, colliderUnitToRefresh.Rotation, Vector3.one);

                    int index = 0;
                    for (int i = 0; i < b2SPolygonColliderDataStructure.finalPoints.Count; i++, index++)
                    {
                        for (int j = 0; j < b2SPolygonColliderDataStructure.finalPoints[i].Count; j++, index++)
                        {
                            finalVexs[index] = transformMatrix4X4.MultiplyPoint(new Vector3(
                                b2SPolygonColliderDataStructure.finalPoints[i][j].X, 1,
                                b2SPolygonColliderDataStructure.finalPoints[i][j].Y));
                        }

                        index--;
                    }

                    break;
            }
        }
    }
#endif
}