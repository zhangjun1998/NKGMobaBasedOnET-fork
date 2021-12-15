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
        /// <summary>
        /// 帧同步Tick
        /// </summary>
        /// <param name="self"></param>
        /// <param name="deltaTime"></param>
        public static void Tick(this LSF_TickComponent self, uint currentFrame, long deltaTime)
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
                        LSF_TickDispatcherComponent.Instance.HandleLSF_Tick(componentToTick, currentFrame, deltaTime);
                    }
                }
            }
        }

#if !SERVER
        /// <summary>
        /// 检查一致性
        /// </summary>
        /// <param name="self"></param>
        /// <param name="frame"></param>
        /// <returns></returns>
        public static bool CheckConsistency(this LSF_TickComponent self, uint frame, ALSF_Cmd alsfCmd)
        {
            Unit unit = self.GetParent<Room>().GetComponent<UnitComponent>().MyUnit;

            using (ListComponent<Entity> componentsToTick = ListComponent<Entity>.Create())
            {
                foreach (var component1 in unit.Components)
                {
                    if (LSF_TickDispatcherComponent.Instance.HasTicker(component1.Key))
                    {
                        if (!LSF_TickDispatcherComponent.Instance.HandleLSF_CheckConsistency(component1.Value, frame,
                            alsfCmd))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        public static void TickView(this LSF_TickComponent self, long deltaTime)
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
                        LSF_TickDispatcherComponent.Instance.HandleLSF_ViewTick(componentToTick, deltaTime);
                    }
                }
            }
        }

        /// <summary>
        /// 预测
        /// </summary>
        /// <param name="self"></param>
        /// <param name="frame"></param>
        /// <returns></returns>
        public static void Predict(this LSF_TickComponent self, long deltaTime)
        {
            Unit unit = self.GetParent<Room>().GetComponent<UnitComponent>().MyUnit;

            using (ListComponent<Entity> componentsToTick = ListComponent<Entity>.Create())
            {
                foreach (var component1 in unit.Components)
                {
                    if (LSF_TickDispatcherComponent.Instance.HasTicker(component1.Key))
                    {
                        LSF_TickDispatcherComponent.Instance.HandleLSF_PredictTick(component1.Value, deltaTime);
                    }
                }
            }
        }

        /// <summary>
        /// 回滚
        /// </summary>
        /// <param name="self"></param>
        /// <param name="frame"></param>
        /// <returns></returns>
        public static bool RollBack(this LSF_TickComponent self, uint frame, ALSF_Cmd alsfCmd)
        {
            Unit unit = self.GetParent<Room>().GetComponent<UnitComponent>().MyUnit;

            using (ListComponent<Entity> componentsToTick = ListComponent<Entity>.Create())
            {
                foreach (var component1 in unit.Components)
                {
                    if (LSF_TickDispatcherComponent.Instance.HasTicker(component1.Key))
                    {
                        LSF_TickDispatcherComponent.Instance.HandleLSF_RollBack(component1.Value, frame,
                            alsfCmd);
                    }
                }

                return true;
            }
        }
#endif
    }
}