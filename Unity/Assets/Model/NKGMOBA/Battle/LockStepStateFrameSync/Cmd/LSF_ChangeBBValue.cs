using System.Collections.Generic;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    public class LSF_ChangeBBValue: ALSF_Cmd
    {
        public const uint CmdType = LSF_CmdType.ChangeBlackBoardValue;

        [ProtoMember(1)]
        public Dictionary<string, ANP_BBValue> TargetBBValues = new Dictionary<string, ANP_BBValue>();

        public override ALSF_Cmd Init(long unitId)
        {
            this.LockStepStateFrameSyncDataType = CmdType;
            this.UnitId = unitId;

            return this;
        }

        public override void Clear()
        {
            TargetBBValues.Clear();
        }
    }
}