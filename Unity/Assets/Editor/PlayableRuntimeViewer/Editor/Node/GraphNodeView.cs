using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace ET
{
    public abstract class GraphNodeView<T>:GraphNodeViewBase
    {
        protected T _data;

        public T Data => _data;

        public const string NodeViewUssPath = "PlayablePreviewStyles/NodeView";
        private VisualElement m_NodeTittle;

        public GraphNodeView(T data)
        {
            _data = data;
            styleSheets.Add(Resources.Load<StyleSheet>(NodeViewUssPath));
            m_NodeTittle = this.Q<Label>("title-label", (string)null);
            m_NodeTittle.AddToClassList("PlayablePreviewNodeView");
            m_NodeTittle.style.color = Color.black;
        }

        public static Color GetColor(Type type)
        {
            if (type == null)
                return Color.red;

            string shortName = type.ToString().Split('.').Last();
            float h = (float)Math.Abs(shortName.GetHashCode()) / int.MaxValue;
            return Color.HSVToRGB(h, 0.6f, 1.0f);
        }

        public override string ToString()
        {
            return Data.ToString();
        }
    }
}