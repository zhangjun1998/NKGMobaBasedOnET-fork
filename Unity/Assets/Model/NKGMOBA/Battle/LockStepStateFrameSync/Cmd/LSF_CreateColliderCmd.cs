using ProtoBuf;

namespace ET
{
    [ProtoContract]
    public class LSF_CreateColliderCmd: ALSF_Cmd
    {
        public const uint CmdType = LSF_CmdType.CreateCollider;
        
        [ProtoMember(1)]
        public int ColliderNPBehaveTreeIdInExcel;

        [ProtoMember(2)]
        public long BelongtoUnitId;

        [ProtoMember(3)]
        public long Id;

        public override ALSF_Cmd Init(long unitId)
        {
            this.LockStepStateFrameSyncDataType = CmdType;
            this.UnitId = unitId;

            return this;
        }

        public override void Clear()
        {
            base.Clear();
            ColliderNPBehaveTreeIdInExcel = 0;
            BelongtoUnitId = 0;
            Id = 0;
        }
    }
}