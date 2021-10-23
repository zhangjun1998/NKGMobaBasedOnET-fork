namespace ET
{
    public interface ILSF_Cmd: IReference
    {
        public uint GetCmdFrame
        {
            get;
        }
        public uint GetLockStepStateFrameSyncDataType
        {
            get;
        }

        public ILSF_Cmd ParseFromMessage(Object message);
    }
}