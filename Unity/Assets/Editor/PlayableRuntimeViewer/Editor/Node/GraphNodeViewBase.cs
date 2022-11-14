using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace ET
{
    public abstract class GraphNodeViewBase:UnityEditor.Experimental.GraphView.Node
    {
        public int Depth;
        public GraphNodeViewBase ParentNode;
        public List<GraphNodeViewBase> ChildNodes = new List<GraphNodeViewBase>();

        public Vector2 Position;

        public int SiblingIndex
        {
            get
            {
                if (ParentNode == null)
                {
                    return 0;
                }
                return ParentNode.ChildNodes.IndexOf(this);
            }
        }

        public virtual void AddChild(GraphNodeViewBase child)
        {
            ChildNodes.Add(child);
            child.ParentNode = this;
        }

        public bool IsLeaf => ChildNodes.Count == 0;

        public bool IsLeftMost => SiblingIndex == 0;
        public float Mod { get; set; }

        public abstract void UpdateView();
        public abstract Port GetPort(Direction direction, int index);

    }
}