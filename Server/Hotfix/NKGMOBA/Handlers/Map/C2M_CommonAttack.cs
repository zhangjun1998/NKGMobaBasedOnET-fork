//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年10月21日 15:25:33
//------------------------------------------------------------

using System;

namespace ET
{
    public class C2M_CommonAttackHandler : AMActorHandler<Player, C2M_CommonAttack>
    {
        protected override async ETTask Run(Player player, C2M_CommonAttack request)
        {
            Unit unit = player.Domain.GetComponent<UnitComponent>().Get(player.UnitId);
            unit.GetComponent<CommonAttackComponent>()
                .SetAttackTarget(unit.Domain.GetComponent<UnitComponent>().Get(request.TargetUnitId));
            await ETTask.CompletedTask;
        }
    }
}