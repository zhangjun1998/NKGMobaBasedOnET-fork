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
                return true;
            }

            using var list = ListComponent<Vector3>.Create();

            unit.BelongToRoom.GetComponent<RecastPathComponent>().SearchPath(10001, unit.Position, target, list.List);

            List<Vector3> path = list.List;
            if (path.Count < 2)
            {
                return true;
            }

            LSF_PathFindCmd pathFindCmd =
                ReferencePool.Acquire<LSF_PathFindCmd>().Init(unit.Id) as LSF_PathFindCmd;

            pathFindCmd.PosX = target.x;
            pathFindCmd.PosY = target.y;
            pathFindCmd.PosZ = target.z;

            unit.BelongToRoom.GetComponent<LSF_Component>().SendMessage(pathFindCmd);

            bool ret = await unit.GetComponent<MoveComponent>()
                .MoveToAsync(path, speed, 100, targetRange, cancellationToken);

            return ret;
        }

        public static void Stop(this Unit unit, int error)
        {
            unit.GetComponent<MoveComponent>().Stop();
        }

    }
}