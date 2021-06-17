using ETModel;
using GraphProcessor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Plugins.NodeEditor
{
    public class NPBehaveGraphWindow : UniversalGraphWindow
    {
        protected override void InitializeWindow(BaseGraph graph)
        {
            graphView = new UniversalGraphView(this);

            m_MiniMap = new MiniMap() {anchored = true};
            graphView.Add(m_MiniMap);
        
            m_ToolbarView = new NPBehaveToolbarView(graphView, m_MiniMap, graph);
            graphView.Add(m_ToolbarView);
            
            this.SetCurrentBlackBoardDataManager();
        }
        
        private void OnFocus()
        {
            SetCurrentBlackBoardDataManager();
        }
        
        private void SetCurrentBlackBoardDataManager()
        {
            NPBehaveGraph npBehaveGraph = (this.graph as NPBehaveGraph);
            
            if (npBehaveGraph == null)
            {
                //因为OnFocus执行时机比较诡异，在OnEnable后，或者执行一些操作后都会执行，但这时Graph可能为空，所以做判断
                return;
            }
            NP_BlackBoardDataManager.CurrentEditedNP_BlackBoardDataManager = (this.graph as NPBehaveGraph).NpBlackBoardDataManager;
        }
    }
}
