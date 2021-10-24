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

        public abstract ALSF_Cmd Init(uint frame);
        
        public virtual void Clear()
        {
            Frame = 0;
            LockStepStateFrameSyncDataType = 0;
        }
    }
}