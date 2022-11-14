//=================================================
//
//    Created by jianzhou.yao
//
//=================================================

using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

namespace ET
{
    public class PlayableGraphViewerEditorWindow : EditorWindow
    {
        private PlayableGraphView _graphView;
        private int _selectedGraphIndex;
        private int _selectedOutputIndex;

        private List<PlayableGraph> _graphDatas = new List<PlayableGraph>()
        {
            new PlayableGraph() // an invalid graph, for compatible with Unity 2019
        };

        private List<string> _graphOptions = new List<string>();

        private int _lastPlayGraphPlayableCount;
        private int _lastPlayGraphRootPlayableCount;
        private int _lastPlayGraphOutputCount;

        [MonKey.Command("RuntimePlayableGraphDebugger", "运行时Playable Debug工具")]
        public static void Open()
        {
            PlayableGraphViewerEditorWindow window = GetWindow<PlayableGraphViewerEditorWindow>("PlayableGraphViewer");
        }

        void OnEnable()
        {
            //m_Graphs = new List<PlayableGraph>(UnityEditor.Playables.Utility.GetAllGraphs());
            Utility.graphCreated -= OnGraphCreated;
            Utility.destroyingGraph -= OnDestroyingGraph;

            Utility.graphCreated += OnGraphCreated;
            Utility.destroyingGraph += OnDestroyingGraph;

            _graphView = new PlayableGraphView(this);
            rootVisualElement.Add(_graphView);

            _graphDatas.Clear();
            _graphDatas.AddRange(UnityEditor.Playables.Utility.GetAllGraphs());

            if (_graphDatas.Count > 0)
            {
                var graphData = _graphDatas[0];
                _graphView.GraphData = graphData;
            }

            var toolbar = new IMGUIContainer(OnGuiHandler);
            rootVisualElement.Add(toolbar);
        }

        private void OnFrameAllButtonClicked()
        {
            _graphView.FrameAll();
        }

        private string GraphPopupFieldFormatter(PlayableGraph graph)
        {
            if (graph.IsValid())
            {
                return graph.GetEditorName();
            }

            return "No PlayableGraph";
        }


        void OnDisable()
        {
            Utility.graphCreated -= OnGraphCreated;
            Utility.destroyingGraph -= OnDestroyingGraph;
        }

        void OnGraphCreated(PlayableGraph graph)
        {
            if (!_graphDatas.Contains(graph))
            {
                _graphDatas.Add(graph);
            }
        }

        void OnDestroyingGraph(PlayableGraph graph)
        {
            _graphDatas.Remove(graph);
        }

        private void OnGuiHandler()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            _graphOptions.Clear();
            foreach (var graph in _graphDatas)
            {
                string name = graph.GetEditorName();
                _graphOptions.Add(name.Length != 0 ? name : "[Unnamed]");
            }
            
            EditorGUI.BeginChangeCheck();
            _selectedGraphIndex = EditorGUILayout.Popup(string.Empty, _selectedGraphIndex, _graphOptions.ToArray());
            if (EditorGUI.EndChangeCheck())
            {
                _graphView.GraphData = _graphDatas[_selectedGraphIndex];
            }

            if (GUILayout.Button("Auto Layout New"))
            {
                foreach (var root in _graphView.Roots)
                {
                    NodeAutoLayouter.Layout(new NewPlayableNodeConvertor().Init(root));
                }
            }

            if (GUILayout.Button("Frame All"))
            {
                OnFrameAllButtonClicked();
            }

            GUILayout.EndHorizontal();
        }

        void Update()
        {
            if (_graphDatas.Count > 0)
            {
                var firstValiedPlayableGraph = _graphDatas[0];
                if (_graphView != null && !_graphView.GraphData.IsValid())
                {
                    _graphView.GraphData = firstValiedPlayableGraph;
                }
            }

            // If in Play mode, refresh the graph each update.
            if (EditorApplication.isPlaying)
            {
                if (!ComparePlayableState())
                {
                    _graphView?.Refresh();
                }
                else
                {
                    _graphView?.UpdateView();
                }
                StoreCurrentPlayableState();
            }
        }

        void StoreCurrentPlayableState()
        {
            if (_graphView != null && _graphView.GraphData.IsValid())
            {
                _lastPlayGraphPlayableCount = _graphView.GraphData.GetPlayableCount();
                _lastPlayGraphRootPlayableCount = _graphView.GraphData.GetRootPlayableCount();
                _lastPlayGraphOutputCount = _graphView.GraphData.GetOutputCount();
            }
        }

        bool ComparePlayableState()
        {
            if (_graphView == null || !_graphView.GraphData.IsValid())
            {
                return false;
            }

            if (_graphView.GraphData.GetPlayableCount() != _lastPlayGraphPlayableCount ||
                _graphView.GraphData.GetRootPlayableCount() != _lastPlayGraphRootPlayableCount ||
                _graphView.GraphData.GetOutputCount() != _lastPlayGraphOutputCount)
            {
                return false;
            }

            return true;
        }
    }
}
