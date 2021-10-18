//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月19日 18:04:17
//------------------------------------------------------------

using ET;
using NPBehave;

namespace ET
{
    public class SyncComponentAwakeSystem: AwakeSystem<NP_SyncComponent>
    {
        public override void Awake(NP_SyncComponent self)
        {
            self.SyncContext = new SyncContext();
        }
    }

    public class SyncComponentFixedUpdate: FixedUpdateSystem<NP_SyncComponent>
    {
        public override void FixedUpdate(NP_SyncComponent self)
        {
            self.SyncContext.Update();
        }
    }
    
    public class SyncComponentDestroy: DestroySystem<NP_SyncComponent>
    {
        public override void Destroy(NP_SyncComponent self)
        {
            self.SyncContext = null;
        }
    }
    
    public class NP_SyncComponent: Entity
    {
        public SyncContext SyncContext;
    }
}