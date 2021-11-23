using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public static class MoveHelper
    {
        // 可以多次调用，多次调用的话会取消上一次的协程
        public static async ETTask<bool> FindPathMoveToAsync(this Unit unit, Vector3 target, float targetRange = 0,
            ETCancellationToken cancellationToken = null)
        {
            float speed = unit.GetComponent<NumericComponent>()[NumericType.Speed] / 100f;
            if (speed < 0.01)
            {
                unit.SendStop(-1);
                return true;
            }

            using var list = ListComponent<Vector3>.Create();

            unit.DomainScene().GetComponent<RecastPathComponent>().SearchPath(10001, unit.Position, target, list.List);

            List<Vector3> path = list.List;
            if (path.Count < 2)
            {
                unit.SendStop(0);
                return true;
            }

            // 广播寻路路径
            M2C_PathfindingResult m2CPathfindingResult = new M2C_PathfindingResult();
            m2CPathfindingResult.Id = unit.Id;
            for (int i = 0; i < list.List.Count; ++i)
            {
                Vector3 vector3 = list.List[i];
                m2CPathfindingResult.Xs.Add(vector3.x);
                m2CPathfindingResult.Ys.Add(vector3.y);
                m2CPathfindingResult.Zs.Add(vector3.z);
            }

            m2CPathfindingResult.Speed = speed;
            MessageHelper.BroadcastToRoom(unit, m2CPathfindingResult);

            bool ret = await unit.GetComponent<MoveComponent>()
                .MoveToAsync(path, speed, 100, targetRange, cancellationToken);
            if (ret) // 如果返回false，说明被其它移动取消了，这时候不需要通知客户端stop
            {
                unit.SendStop(0);
            }

            return ret;
        }

        public static void Stop(this Unit unit, int error)
        {
            unit.GetComponent<MoveComponent>().Stop();
            unit.SendStop(error);
        }

        public static void SendStop(this Unit unit, int error)
        {
            MessageHelper.BroadcastToRoom(unit, new M2C_Stop()
            {
                Error = error,
                Id = unit.Id,
                X = unit.Position.x,
                Y = unit.Position.y,
                Z = unit.Position.z,

                A = unit.Rotation.x,
                B = unit.Rotation.y,
                C = unit.Rotation.z,
                W = unit.Rotation.w,
            });
        }
    }
}