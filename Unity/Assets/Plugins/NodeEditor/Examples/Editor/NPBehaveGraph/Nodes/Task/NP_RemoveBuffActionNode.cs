//------------------------------------------------------------
// 代码由工具自动生成，请勿手动修改
// 代码由工具自动生成，请勿手动修改
// 代码由工具自动生成，请勿手动修改
//------------------------------------------------------------

using ETModel;
using GraphProcessor;

namespace Plugins.NodeEditor
{
    [NodeMenuItem("NPBehave行为树/Task/移除一个Buff", typeof (NPBehaveGraph))]
    [NodeMenuItem("NPBehave行为树/Task/移除一个Buff", typeof (SkillGraph))]
    public class NP_RemoveBuffActionNode: NP_TaskNodeBase
    {
        public override string name => "移除一个Buff";

        public NP_ActionNodeData NP_ActionNodeData =
                new NP_ActionNodeData() { NodeType = NodeType.Task, NpClassForStoreAction = new NP_RemoveBuffAction() };

        public override NP_NodeDataBase NP_GetNodeData()
        {
            return NP_ActionNodeData;
        }
    }
}