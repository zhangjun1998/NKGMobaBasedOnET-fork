using UnityEngine;

namespace ET
{
    public class LSF_MoveCmd: ILSF_Cmd
    {
        private static uint s_LSF_CmdType = LSF_CmdType.Move;

        public uint GetCmdFrame => Frame;

        public uint GetLockStepStateFrameSyncDataType => s_LSF_CmdType;
        
        public ILSF_Cmd ParseFromMessage(Object message)
        {
            throw new System.NotImplementedException();
        }

        public uint Frame;
        
        public Vector3 Pos;
        public Quaternion Rot;
        public float Speed;
        public bool IsStopped;

        public void Clear()
        {
            Pos = Vector3.zero;
            Rot = Quaternion.identity;
            Speed = 0;
            IsStopped = false;
            Frame = 0;
        }
    }
}