//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月16日 21:12:08
//------------------------------------------------------------

using System.Collections.Generic;

namespace ETModel
{
    [ObjectSystem]
    public class BuffManagerCompoenntUpdateSystem: UpdateSystem<BuffManagerComponent>
    {
        public override void Update(BuffManagerComponent self)
        {
            self.Update();
        }
    }

    public class BuffManagerComponent: Component
    {
        /// <summary>
        /// Buff链表
        /// </summary>
        private LinkedList<IBuffSystem> m_Buffs = new LinkedList<IBuffSystem>();

        /// <summary>
        /// 用于查找的——基于Buff生效方式
        /// </summary>
        private Dictionary<BuffWorkTypes, IBuffSystem> m_BuffsForFind_BuffWorkType = new Dictionary<BuffWorkTypes, IBuffSystem>();

        /// <summary>
        /// 用于查找的——基于Buff的Id
        /// </summary>
        private Dictionary<long, IBuffSystem> m_BuffsForFind_BuffId = new Dictionary<long, IBuffSystem>();

        private LinkedListNode<IBuffSystem> m_Current, m_Next;

        public void Update()
        {
            this.m_Current = m_Buffs.First;
            //轮询链表
            while (this.m_Current != null)
            {
                IBuffSystem aBuff = this.m_Current.Value;
                if (aBuff.BuffState == BuffState.Waiting)
                {
                    aBuff.Excute();
                }
                else if (aBuff.BuffState == BuffState.Running || aBuff.BuffState == BuffState.Forever)
                {
                    aBuff.Update();
                    this.m_Current = this.m_Current.Next;
                }
                else if (aBuff.BuffState == BuffState.Finished)
                {
                    aBuff.Finished();
                    this.m_Next = this.m_Current.Next;
                    m_Buffs.Remove(this.m_Current);
                    m_BuffsForFind_BuffWorkType.Remove(this.m_Current.Value.BuffData.BuffWorkType);
                    this.m_BuffsForFind_BuffId.Remove(this.m_Current.Value.BuffData.BuffId);
                    // Log.Info(
                    //     $"移除一个Buff，Id为{this.m_Current.Value.BuffData.BuffId},BuffManager是否还有?:{this.FindBuffById(this.m_Current.Value.BuffData.BuffId)}");
                    this.m_Current = this.m_Next;
                }
            }
        }

        #region 添加，移除Buff

        /// <summary>
        /// 添加Buff到真实链表，禁止外部调用
        /// </summary>
        /// <param name="aBuff"></param>
        public void AddBuff(IBuffSystem aBuff)
        {
            m_Buffs.AddLast(aBuff);

            this.m_BuffsForFind_BuffWorkType[aBuff.BuffData.BuffWorkType] = aBuff;
            this.m_BuffsForFind_BuffId[aBuff.BuffData.BuffId] = aBuff;
            // Log.Info($"把ID为{aBuff.BuffData.BuffId}的buff加入检索表");
        }

        /// <summary>
        /// 移除Buff(下一帧才真正移除 TODO 考虑到有些Buff绕一圈下来可能会移除自己，需要做额外处理，暂时先放着)
        /// </summary>
        /// <param name="buffId">要移除的BuffId</param>
        public void RemoveBuff(long buffId)
        {
            IBuffSystem aBuffSystemBase = GetBuffById(buffId);
            if (aBuffSystemBase != null)
            {
                aBuffSystemBase.BuffState = BuffState.Finished;
            }
        }

        #endregion

        #region 查询BuffSystem

        /// <summary>
        /// 通过作用方式查找Buff
        /// </summary>
        /// <param name="buffWorkTypes"></param>
        /// <returns></returns>
        public bool FindBuffByWorkType(BuffWorkTypes buffWorkTypes)
        {
            if (this.m_BuffsForFind_BuffWorkType.TryGetValue(buffWorkTypes, out IBuffSystem _temp))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 通过标识ID查找Buff
        /// </summary>
        /// <param name="buffId">BuffData的标识ID</param>
        /// <returns></returns>
        public bool FindBuffById(long buffId)
        {
            if (this.m_BuffsForFind_BuffId.TryGetValue(buffId, out IBuffSystem _temp))
            {
                return true;
            }

            return false;
        }

        #endregion

        #region 获取BuffSystem

        /// <summary>
        /// 通过作用方式获得Buff
        /// </summary>
        /// <param name="buffWorkTypes"></param>
        public IBuffSystem GetBuffByWorkType(BuffWorkTypes buffWorkTypes)
        {
            if (m_BuffsForFind_BuffWorkType.TryGetValue(buffWorkTypes, out IBuffSystem _temp))
            {
                return _temp;
            }

            //Log.Error($"查找{buffWorkTypes}失败");
            return null;
        }

        /// <summary>
        /// 通过标识ID获得Buff
        /// </summary>
        /// <param name="buffId">BuffData的标识ID</param>
        public IBuffSystem GetBuffById(long buffId)
        {
            if (this.m_BuffsForFind_BuffId.TryGetValue(buffId, out IBuffSystem _temp))
            {
                return _temp;
            }

            return null;
        }

        #endregion
    }
}