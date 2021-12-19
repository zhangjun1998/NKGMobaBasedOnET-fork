using System.Collections.Generic;

namespace ET
{
    [LSF_MessageHandler(LSF_CmdHandlerType = LSF_ChangeBBValue.CmdType)]
    public class LSF_ChangeBBValueHandler : ALockStepStateFrameSyncMessageHandler<LSF_ChangeBBValue>
    {
        protected override async ETVoid Run(Unit unit, LSF_ChangeBBValue cmd)
        {
            SkillCanvasManagerComponent skillCanvasManagerComponent = unit.GetComponent<SkillCanvasManagerComponent>();

            foreach (var allSkillCanva in skillCanvasManagerComponent.GetAllSkillCanvas())
            {
                foreach (var sTree in allSkillCanva.Value)
                {
                    if (sTree.Id == cmd.TargetNPBehaveTreeId)
                    {
                        foreach (var toBeChangedBBValues in cmd.TargetBBValues)
                        {
                            BBValueHelper.SetTargetBlackboardUseANP_BBValue(toBeChangedBBValues.Value,
                                sTree.GetBlackboard(), toBeChangedBBValues.Key);
                        }
                    }
                }
            }

            await ETTask.CompletedTask;
        }
    }
}