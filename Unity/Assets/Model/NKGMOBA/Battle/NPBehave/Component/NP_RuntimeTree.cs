//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月23日 14:58:54
//------------------------------------------------------------

using NPBehave;

namespace ET
{
    public class NP_RuntimeTreeAwakeSystem : AwakeSystem<NP_RuntimeTree, NP_DataSupportor, Unit>
    {
        public override void Awake(NP_RuntimeTree self, NP_DataSupportor m_BelongNP_DataSupportor, Unit belongToUnit)
        {
            self.Awake(m_BelongNP_DataSupportor, belongToUnit);
        }
    }

    public class NP_RuntimeTreeDestroySystem : DestroySystem<NP_RuntimeTree>
    {
        public override void Destroy(NP_RuntimeTree self)
        {
            self.Finish().Coroutine();
        }
    }

    public class NP_RuntimeTree : Entity
    {
        /// <summary>
        /// NP行为树根结点
        /// </summary>
        private Root m_RootNode;

        /// <summary>
        /// 所归属的数据块
        /// </summary>
        public NP_DataSupportor BelongNP_DataSupportor;

        /// <summary>
        /// 所归属的Unit
        /// </summary>
        public Unit BelongToUnit;

        public void Awake(NP_DataSupportor m_BelongNP_DataSupportor, Unit belongToUnit)
        {
            BelongToUnit = belongToUnit;
            this.BelongNP_DataSupportor = m_BelongNP_DataSupportor;
        }

        /// <summary>
        /// 设置根结点
        /// </summary>
        /// <param name="rootNode"></param>
        public void SetRootNode(Root rootNode)
        {
            this.m_RootNode = rootNode;
        }

        /// <summary>
        /// 获取黑板
        /// </summary>
        /// <returns></returns>
        public Blackboard GetBlackboard()
        {
            return this.m_RootNode.Blackboard;
        }

        /// <summary>
        /// 开始运行行为树
        /// </summary>
        public void Start()
        {
            this.m_RootNode.Start();
        }

        /// <summary>
        /// 终止行为树
        /// </summary>
        public async ETVoid Finish()
        {
            //因为编辑器模式下会因为Game.Scene的销毁而报错，但是NOBehave又只能在下一帧销毁，所以就这样写了
#if UNITY_EDITOR
            await ETTask.CompletedTask;
#else
            await TimerComponent.Instance.WaitFrameAsync();
#endif

            this.m_RootNode.CancelWithoutReturnResult();
            BelongToUnit = null;
            this.m_RootNode = null;
            this.BelongNP_DataSupportor = null;
        }
    }
}