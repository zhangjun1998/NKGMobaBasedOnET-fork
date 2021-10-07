//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年6月16日 20:25:50
//------------------------------------------------------------

using ET;
using GraphProcessor;
using UnityEngine.UIElements;

namespace Plugins.NodeEditor
{
    [NodeCustomEditor(typeof(NP_NodeBase))]
    public class NP_NodeView: BaseNodeView
    {
        public override void Enable()
        {
            NP_NodeDataBase nodeDataBase = (this.nodeTarget as NP_NodeBase).NP_GetNodeData();
            TextField textField = new TextField(){ value = nodeDataBase.NodeDes};
            textField.style.marginTop = 4;
            textField.style.marginBottom = 4;
            textField.RegisterValueChangedCallback((changedDes) => { nodeDataBase.NodeDes = changedDes.newValue; });
            controlsContainer.Add(textField);
        }
    }
}