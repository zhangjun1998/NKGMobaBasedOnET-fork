using ProtoBuf;

namespace ET
{
    [ProtoContract]
    public class LSF_PathFindCmd : ALSF_Cmd
    {
        public const uint CmdType = LSF_CmdType.PathFind;

        [ProtoMember(1)] public float PosX;
        [ProtoMember(2)] public float PosY;
        [ProtoMember(3)] public float PosZ;

        public override ALSF_Cmd Init(long unitId)
        {
            this.LockStepStateFrameSyncDataType = CmdType;
            this.UnitId = unitId;

            return this;
        }

        public override void Clear()
        {
            base.Clear();
            PosX = 0;
            PosY = 0;
            PosZ = 0;
        }
    }
}