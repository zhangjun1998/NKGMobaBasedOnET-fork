//------------------------------------------------------------
// 此代码由工具自动生成，请勿更改
// 此代码由工具自动生成，请勿更改
// 此代码由工具自动生成，请勿更改
//------------------------------------------------------------

using ETModel;
using GraphProcessor;
using UnityEditor;

namespace Plugins.NodeEditor
{
    [NodeMenuItem("技能数据部分/治疗Buff", typeof (SkillGraph))]
    public class TreatmentBuffNode: BuffNodeBase
    {
        public override string name => "治疗Buff";

        public NormalBuffNodeData SkillBuffBases =
                new NormalBuffNodeData()
                {
                    BuffDes = "治疗Buff",
                    BuffData = new TreatmentBuffData() { BelongBuffSystemType = BuffSystemType.TreatmentBuffSystem }
                };
        
        public override BuffNodeDataBase Skill_GetNodeData()
        {
            return SkillBuffBases;
        }
    }
}
