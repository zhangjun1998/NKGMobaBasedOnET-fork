//此文件格式由工具自动生成

using Sirenix.OdinInspector;

namespace ET
{
    #region System

    [ObjectSystem]
    public class CommonAttackComponentAwakeSystem : AwakeSystem<CommonAttackComponent_Logic>
    {
        public override void Awake(CommonAttackComponent_Logic self)
        {
            self.Awake();
        }
    }

    #endregion

    /// <summary>
    /// 攻击执行或取消时执行替换的类型
    /// </summary>
    public enum AttackCastReplaceType
    {
        [LabelText("替换攻击")] Attack,

        [LabelText("替换取消攻击")] CancelAttack,
    }

    public class CommonAttackComponent_Logic : Entity
    {
        #region 私有成员

        public StackFsmComponent m_StackFsmComponent;

        /// <summary>
        /// 上次选中的Unit，用于自动攻击
        /// </summary>
        public Unit CachedUnitForAttack;

        /// <summary>
        /// 替换攻击流程目标行为树Id
        /// </summary>
        public long AttackReplaceNPTreeId;

        /// <summary>
        /// 用于替换攻击流程的黑板键值
        /// </summary>
        public NP_BlackBoardRelationData AttackReplaceBB;

        /// <summary>
        /// 替换取消攻击流程目标行为树Id
        /// </summary>
        public long CancelAttackReplaceNPTreeId;

        /// <summary>
        /// 用于替换取消攻击流程的黑板键值
        /// </summary>
        public NP_BlackBoardRelationData CancelAttackReplaceBB;
        public ETCancellationToken CancellationTokenSource;

        #endregion

        #region 公有成员

        /// <summary>
        /// 设置攻击替换信息
        /// </summary>
        /// <param name="npTreeId"></param>
        /// <param name="attackReplaceBB"></param>
        public void SetAttackReplaceInfo(long npTreeId, NP_BlackBoardRelationData attackReplaceBB)
        {
            this.AttackReplaceNPTreeId = npTreeId;
            this.AttackReplaceBB = attackReplaceBB;
        }

        public bool HasAttackReplaceInfo()
        {
            return this.AttackReplaceBB != null;
        }

        /// <summary>
        /// 设置取消攻击替换流程
        /// </summary>
        /// <param name="npTreeId"></param>
        /// <param name="attackReplaceBB"></param>
        public void SetCancelAttackReplaceInfo(long npTreeId, NP_BlackBoardRelationData attackReplaceBB)
        {
            this.CancelAttackReplaceNPTreeId = npTreeId;
            this.CancelAttackReplaceBB = attackReplaceBB;
        }

        public bool HasCancelAttackReplaceInfo()
        {
            return this.CancelAttackReplaceBB != null;
        }

        /// <summary>
        /// 重置攻击替换信息
        /// </summary>
        public void ReSetAttackReplaceInfo()
        {
            this.AttackReplaceNPTreeId = 0;
            this.AttackReplaceBB = null;
        }

        /// <summary>
        /// 重置取消攻击替换流程
        /// </summary>
        public void ReSetCancelAttackReplaceInfo()
        {
            this.CancelAttackReplaceNPTreeId = 0;
            this.CancelAttackReplaceBB = null;
        }

        #endregion

        #region 生命周期函数

        public void Awake()
        {
            Unit unit = this.GetParent<Unit>();

            //此处填写Awake逻辑
            m_StackFsmComponent = unit.GetComponent<StackFsmComponent>();
            this.CancellationTokenSource = new ETCancellationToken();

            CDInfo attackCDInfo = ReferencePool.Acquire<CDInfo>();
            attackCDInfo.Name = "CommonAttack";
            attackCDInfo.Interval = 750;

            CDInfo moveCDInfo = ReferencePool.Acquire<CDInfo>();
            moveCDInfo.Name = "MoveToAttack";
            moveCDInfo.Interval = 300;

            CDComponent.Instance.AddCDData(unit.Id, attackCDInfo);
            CDComponent.Instance.AddCDData(unit.Id, moveCDInfo);
        }

        public override void Dispose()
        {
            if (IsDisposed)
                return;
            base.Dispose();
            //此处填写释放逻辑,但涉及Entity的操作，请放在Destroy中
            this.CancellationTokenSource?.Cancel();
            this.CancellationTokenSource = null;

            this.ReSetAttackReplaceInfo();
            this.ReSetCancelAttackReplaceInfo();
        }

        #endregion
    }
}