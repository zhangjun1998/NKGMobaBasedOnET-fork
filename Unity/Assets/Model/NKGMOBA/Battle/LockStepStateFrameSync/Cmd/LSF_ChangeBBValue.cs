using System.Collections.Generic;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    public class LSF_ChangeBBValue: ALSF_Cmd
    {
        public const uint CmdType = LSF_CmdType.ChangeBlackBoardValue;

        /// <summary>
        /// 目标行为树Id
        /// </summary>
        [ProtoMember(1)] public long TargetNPBehaveTreeId;

        /// <summary>
        /// 将要同步修改的黑板键值
        /// </summary>
        [ProtoMember(2)]
        public Dictionary<string, ANP_BBValue> TargetBBValues = new Dictionary<string, ANP_BBValue>();

        public override ALSF_Cmd Init(long unitId)
        {
            this.LockStepStateFrameSyncDataType = CmdType;
            this.UnitId = unitId;
            this.TargetNPBehaveTreeId = 0;

            return this;
        }

        public override void Clear()
        {
            TargetBBValues.Clear();
        }
    }
}