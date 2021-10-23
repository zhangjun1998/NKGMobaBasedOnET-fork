namespace ET
{
    public class LSF_CmdType
    {
        //----------移动模块，0~100
        public const uint Move = 0;
        
        //----------行为树模块，101 - 10000
        public const uint ChangeBlackBoardValue = 101;
        
        //----------Slate模块，10001 - 20000
        public const uint ChangeMainKey = 10001;
    }
}