using System;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    public class LSF_PlaySkillInputCmd : ALSF_Cmd
    {
        public const uint CmdType = LSF_CmdType.PlayerSkillInput;

        /// <summary>
        /// 输入的Tag标识，比如PlayerInput，E长按
        /// </summary>
        [ProtoMember(1)] public string InputTag;

        /// <summary>
        /// 输入的具体指令，比如E，EHold
        /// </summary>
        [ProtoMember(2)] public string InputKey;

        public override ALSF_Cmd Init(long unitId)
        {
            this.LockStepStateFrameSyncDataType = CmdType;
            this.UnitId = unitId;
            return this;
        }

        public override void Clear()
        {
            InputKey = String.Empty;
        }
    }
}