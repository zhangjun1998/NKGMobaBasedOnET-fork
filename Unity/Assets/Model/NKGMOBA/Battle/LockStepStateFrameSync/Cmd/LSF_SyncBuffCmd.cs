using ProtoBuf;

namespace ET
{
    public class LSF_SyncBuffCmd: ALSF_Cmd
    {
        public const uint CmdType = LSF_CmdType.SyncBuff;

        [ProtoMember(1)]
        public BuffSnapInfo BuffSnapInfo;
        
        public override ALSF_Cmd Init(long unitId)
        {
            this.LockStepStateFrameSyncDataType = CmdType;
            this.UnitId = unitId;

            return this;
        }

        public override void Clear()
        {
            BuffSnapInfo.FrameBuffChangeSnap.Clear();
        }
    }
}