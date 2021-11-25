namespace ET
{
    public class LSF_CmdType
    {
        //----------通用模块，1~100
        public const uint Move = 1;
        public const uint PathFind = 2;
        public const uint CreateSpiling = 3;
        public const uint CommonAttack = 4;

        //----------行为树模块，101 - 10000
        public const uint ChangeBlackBoardValue = 101;
        
        //----------Slate模块，10001 - 20000
        public const uint ChangeMainKey = 10001;
    }
}