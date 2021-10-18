//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月24日 16:42:12
//------------------------------------------------------------

using System.Collections.Generic;

namespace ET
{
    public class M2C_SyncNPBehaveBoolDataHandler: AMHandler<M2C_SyncNPBehaveBoolData>
    {
        protected override async ETVoid Run(Session session, M2C_SyncNPBehaveBoolData message)
        {
            Unit unit = session.DomainScene().GetComponent<RoomManagerComponent>().GetOrCreateBattleRoom().GetComponent<UnitComponent>().Get(message.UnitId);
            foreach (var skillCanvaList in unit.GetComponent<SkillCanvasManagerComponent>().GetAllSkillCanvas())
            {
                foreach (var skillNpRuntimeTree in skillCanvaList.Value)
                {
                    skillNpRuntimeTree.GetBlackboard().Set(message.BBKey, message.Value);
                }
            }

            await ETTask.CompletedTask;
        }
    }
}