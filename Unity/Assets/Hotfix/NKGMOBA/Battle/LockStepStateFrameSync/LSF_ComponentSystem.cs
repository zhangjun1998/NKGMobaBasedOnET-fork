using System;
using System.Collections.Generic;
using ET.EventType;

namespace ET
{
    public class LockStepStateFrameSyncComponentAwakeSystem : AwakeSystem<LSF_Component>
    {
        public override void Awake(LSF_Component self)
        {
#if !SERVER
            self.CurrentFrame = LSF_Component.AheadOfFrameMax;
#endif
        }
    }

    public class LockStepStateFrameSyncComponentUpdateSystem : UpdateSystem<LSF_Component>
    {
        public override void Update(LSF_Component self)
        {
            if (!self.StartSync)
            {
                return;
            }

#if !SERVER
            self.CurrentAheadOfFrame = self.CurrentFrame - self.ServerCurrentFrame;

            // Log.Info(
            //     $"-------------------CurrentAheadOfFrame: {self.CurrentAheadOfFrame} TargetAheadOfFrame: {self.TargetAheadOfFrame} ServerCurrentFrame: {self.ServerCurrentFrame}");

            if (self.CurrentAheadOfFrame != self.TargetAheadOfFrame)
            {
                //Log.Info("------------------进入变速状态");
                self.HasInSpeedChangeState = true;
                self.FixedUpdate.TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond /
                                                                        (GlobalDefine.FixedUpdateTargetFPS +
                                                                         self.TargetAheadOfFrame -
                                                                         self.CurrentAheadOfFrame
                                                                        ));
            }
            else if (self.HasInSpeedChangeState)
            {
                //Log.Info("------------------已经对齐");
                self.HasInSpeedChangeState = false;
                self.FixedUpdate.TargetElapsedTime =
                    TimeSpan.FromTicks(TimeSpan.TicksPerSecond / (GlobalDefine.FixedUpdateTargetFPS));
            }
#endif

            // 将FixedUpdate Tick放在此处，这样可以防止框架层FixedUpdate帧率小于帧同步FixedUpdate帧率而导致的一些问题
            self.FixedUpdate.Tick();
        }
    }

    public class LockStepStateFrameSyncComponentDestroySystem : DestroySystem<LSF_Component>
    {
        public override void Destroy(LSF_Component self)
        {
            self.FixedUpdate.UpdateCallback = null;
            self.FixedUpdate = null;
        }
    }
}