namespace ET
{
    public class LSF_CmdHandlerComponentAwakeSystem: AwakeSystem<LSF_CmdHandlerComponent>
    {
        public override void Awake(LSF_CmdHandlerComponent self)
        {
            self.Load();
            LSF_CmdHandlerComponent.Instance = self;
        }
    }
    
    public class LSF_CmdHandlerComponentDestroySystem: DestroySystem<LSF_CmdHandlerComponent>
    {
        public override void Destroy(LSF_CmdHandlerComponent self)
        {
            LSF_CmdHandlerComponent.Instance = null;
        }
    }
}