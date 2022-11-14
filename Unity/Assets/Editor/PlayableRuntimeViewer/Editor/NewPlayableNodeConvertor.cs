using System;
using UnityEngine;

namespace ET
{
    public class NewPlayableNodeConvertor : INodeForLayoutConvertor
    {
        public float SiblingDistance => 50;
        public float TreeDistance { get; }

        private NodeAutoLayouter.TreeNode m_LayoutRootNode;
        public NodeAutoLayouter.TreeNode LayoutRootNode => m_LayoutRootNode;

        public object PrimRootNode => m_PrimRootNode;
        private object m_PrimRootNode;

        public INodeForLayoutConvertor Init(object primRootNode)
        {
            this.m_PrimRootNode = primRootNode;
            return this;
        }

        public NodeAutoLayouter.TreeNode PrimNode2LayoutNode()
        {
            GraphNodeViewBase graphNodeViewBase = m_PrimRootNode as GraphNodeViewBase;

            if (graphNodeViewBase.layout.width == Single.NaN)
            {
                return null;
            }

            m_LayoutRootNode =
                new NodeAutoLayouter.TreeNode(graphNodeViewBase.layout.height + SiblingDistance,
                    graphNodeViewBase.layout.width,
                    graphNodeViewBase.layout.y,
                    NodeAutoLayouter.CalculateMode.Horizontal | NodeAutoLayouter.CalculateMode.Negative);

            Convert2LayoutNode(graphNodeViewBase,
                m_LayoutRootNode, graphNodeViewBase.layout.y + graphNodeViewBase.layout.width,
                NodeAutoLayouter.CalculateMode.Horizontal | NodeAutoLayouter.CalculateMode.Negative);


            return m_LayoutRootNode;
        }

        private void Convert2LayoutNode(GraphNodeViewBase rootPrimNode,
            NodeAutoLayouter.TreeNode rootLayoutNode, float lastHeightPoint,
            NodeAutoLayouter.CalculateMode calculateMode)
        {
            foreach (var childNode in rootPrimNode.ChildNodes)
            {
                NodeAutoLayouter.TreeNode childLayoutNode =
                    new NodeAutoLayouter.TreeNode(childNode.layout.height + SiblingDistance, childNode.layout.width,
                        lastHeightPoint + this.SiblingDistance,
                        calculateMode);
                rootLayoutNode.AddChild(childLayoutNode);
                Convert2LayoutNode(childNode, childLayoutNode,
                    lastHeightPoint + this.SiblingDistance + childNode.layout.width, calculateMode);
            }
        }

        public void LayoutNode2PrimNode()
        {
            Vector2 calculateRootResult = m_LayoutRootNode.GetPos();

            GraphNodeViewBase root = m_PrimRootNode as GraphNodeViewBase;
            root.Position = calculateRootResult;
            root.SetPosition(new Rect(root.Position, Vector2.zero));

            Convert2PrimNode(m_PrimRootNode as GraphNodeViewBase, m_LayoutRootNode);
        }

        private void Convert2PrimNode(GraphNodeViewBase rootPrimNode,
            NodeAutoLayouter.TreeNode rootLayoutNode)
        {
            for (int i = 0; i < rootLayoutNode.children.Count; i++)
            {
                Vector2 calculateResult = rootLayoutNode.children[i].GetPos();

                rootPrimNode.ChildNodes[i].Position = calculateResult;
                rootPrimNode.ChildNodes[i].SetPosition(new Rect(rootPrimNode.ChildNodes[i].Position, Vector2.zero));

                Convert2PrimNode(rootPrimNode.ChildNodes[i], rootLayoutNode.children[i]);
            }
        }
    }
}