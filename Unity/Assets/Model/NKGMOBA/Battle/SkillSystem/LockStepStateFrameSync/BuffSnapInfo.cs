using System.Collections.Generic;
using ProtoBuf;

namespace ET
{
    public class BuffInfo : IReference
    {
        public enum BuffOperationType
        {
            NONE,
            ADD,
            REMOVE,
            CHANGE
        }
        
        /// <summary>
        /// Buff归属的NPSupportId
        /// </summary>
        public long NP_SupportId;

        /// <summary>
        /// Buff自身的Id
        /// </summary>
        public long BuffId;

        /// <summary>
        /// Buff的层数
        /// </summary>
        public int BuffLayer;

        /// <summary>
        /// Buff会被移除的目标帧
        /// </summary>
        public uint BuffMaxLimitFrame;

        public BuffOperationType OperationType;
        
        public void Clear()
        {
        }
    }

    public class BuffSnapInfo : IReference
    {
        /// <summary>
        /// 单帧内变化的Buff信息
        /// </summary>
        [ProtoMember(1)]
        public Dictionary<long, BuffInfo> FrameBuffChangeSnap = new Dictionary<long, BuffInfo>();

        public bool Check(BuffSnapInfo buffSnapInfoToCompare)
        {
            
            
            return true;
        }

        public BuffSnapInfo GetDifference(BuffSnapInfo buffSnapInfoToCompare)
        {
            BuffSnapInfo result = ReferencePool.Acquire<BuffSnapInfo>();


            return result;
        }
        
        public void Clear()
        {
        }
    }
}