﻿//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年10月2日 19:41:52
//------------------------------------------------------------

using System.Collections.Generic;
using NPBehave;

namespace ET
{
    public class M2C_BuffInfoHandler : AMHandler<M2C_BuffInfo>
    {
        protected override async ETVoid Run(Session session, M2C_BuffInfo message)
        {
            Unit unit = session.DomainScene().GetComponent<RoomManagerComponent>().GetBattleRoom().GetComponent<UnitComponent>()
                .Get(message.UnitId);
            
            List<NP_RuntimeTree> skillContents =
                unit.GetComponent<SkillCanvasManagerComponent>().GetSkillCanvas(message.SkillId);
            if (skillContents == null)
            {
                return;
            }
            else
            {
                foreach (var skillContent in skillContents)
                {
                    Blackboard blackboard = skillContent.GetBlackboard();
                    if (blackboard == null)
                    {
                        continue;
                    }

                    blackboard.Set(message.BBKey, message.BuffLayers);
                    blackboard.Get<List<long>>("TheUnitFromIds").Add(message.TheUnitFromId);
                    blackboard.Get<List<long>>("TheUnitBelongToIds").Add(message.TheUnitBelongToId);
                }
            }

            await ETTask.CompletedTask;
        }
    }
}