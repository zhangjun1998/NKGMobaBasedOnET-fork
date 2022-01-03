using System.Collections.Generic;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    public class LSF_SyncAttributeCmd: ALSF_Cmd
    {
        public const uint CmdType = LSF_CmdType.SyncAttribute;

        [ProtoMember(1)]
        public Dictionary<NumericType, float> SyncAttributes = new Dictionary<NumericType, float>();

        public override ALSF_Cmd Init(long unitId)
        {
            this.LockStepStateFrameSyncDataType = CmdType;
            this.UnitId = unitId;

            return this;
        }
    }
}