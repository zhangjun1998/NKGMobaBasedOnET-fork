using System;

namespace ET
{
    public class LSF_TickComponentAwakeSystem : AwakeSystem<LSF_TickComponent>
    {
        public override void Awake(LSF_TickComponent self)
        {
        }
    }

    public static class LSF_TickComponentUtilities
    {
        public static void Tick(this LSF_TickComponent self, long deltaTime)
        {
            Entity entity = self.GetParent<Entity>();

            using (ListComponent<Entity> componentsToTick = ListComponent<Entity>.Create())
            {
                foreach (var component1 in entity.Components)
                {
                    if (LSF_TickDispatcherComponent.Instance.HasTicker(component1.Key))
                    {
                        componentsToTick.List.Add(component1.Value);
                    }
                }

                foreach (var componentToTick in componentsToTick.List)
                {
                    Type type = componentToTick.GetType();
                    // 因为有可能Tick过程中移除了Component，所以需要做一下判断
                    if (entity.Components.ContainsKey(type))
                    {
                        LSF_TickDispatcherComponent.Instance.HandleLSF_Tick(componentToTick, deltaTime);
                    }
                }
            }
        }
    }
}