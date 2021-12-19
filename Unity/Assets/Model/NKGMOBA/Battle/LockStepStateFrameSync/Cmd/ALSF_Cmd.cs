using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [ProtobufBaseTypeRegister]
    public abstract class ALSF_Cmd: IReference
    {
        [ProtoMember(1)]
        public uint Frame;

        [ProtoMember(2)]
        public uint LockStepStateFrameSyncDataType;

        [ProtoMember(3)]
        public long UnitId;

        public abstract ALSF_Cmd Init(long unitId);
        
        public virtual bool CheckConsistency(ALSF_Cmd alsfCmd)
        {
            return true;
        }
        
        public virtual void Clear()
        {
            Frame = 0;
            LockStepStateFrameSyncDataType = 0;
            UnitId = 0;
        }
    }
}