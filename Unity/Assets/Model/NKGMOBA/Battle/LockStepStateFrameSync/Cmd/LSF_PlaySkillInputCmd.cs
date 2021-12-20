using System;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    public class LSF_PlaySkillInputCmd : ALSF_Cmd
    {
        public const uint CmdType = LSF_CmdType.PlayerSkillInput;

        /// <summary>
        /// 目标行为树Id
        /// </summary>
        [ProtoMember(1)] public string InputKey;

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