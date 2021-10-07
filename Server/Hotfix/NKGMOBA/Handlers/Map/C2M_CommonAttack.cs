//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年10月21日 15:25:33
//------------------------------------------------------------

using System;

namespace ET
{
    public class C2M_CommonAttackHandler : AMActorLocationHandler<Unit, C2M_CommonAttack>
    {
        protected override async ETTask Run(Unit unit, C2M_CommonAttack request)
        {
            unit.GetComponent<CommonAttackComponent>()
                .SetAttackTarget(unit.BelongToRoom.GetComponent<UnitComponent>().Get(request.TargetUnitId));
            await ETTask.CompletedTask;
        }
    }
}