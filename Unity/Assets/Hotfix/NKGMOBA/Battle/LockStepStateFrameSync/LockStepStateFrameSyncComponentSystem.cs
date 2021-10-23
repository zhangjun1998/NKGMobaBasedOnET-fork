using System;
using System.Collections.Generic;
using ET.EventType;

namespace ET
{
    public class LockStepStateFrameSyncComponentAwakeSystem : AwakeSystem<LockStepStateFrameSyncComponent>
    {
        public override void Awake(LockStepStateFrameSyncComponent self)
        {
            self.FixedUpdate = new FixedUpdate() {UpdateCallback = self.LSF_Tick};
        }
    }

    public class LockStepStateFrameSyncComponentUpdateSystem : UpdateSystem<LockStepStateFrameSyncComponent>
    {
        public override void Update(LockStepStateFrameSyncComponent self)
        {
            if (!self.StartSync)
            {
                return;
            }

#if !SERVER
            self.CurrentAheadOfFrame = self.CurrentFrame - self.ServerCurrentFrame;

            if (self.CurrentAheadOfFrame != self.TargetAheadOfFrame)
            {
                self.HasInSpeedChangeState = true;
                self.FixedUpdate.TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond /
                                                                        (GlobalDefine.FixedUpdateTargetFPS +
                                                                         self.TargetAheadOfFrame -
                                                                         self.CurrentAheadOfFrame
                                                                        ));
            }
            else if (self.HasInSpeedChangeState)
            {
                self.HasInSpeedChangeState = false;
                self.FixedUpdate.TargetElapsedTime =
                    TimeSpan.FromTicks(TimeSpan.TicksPerSecond / (GlobalDefine.FixedUpdateTargetFPS));
            }
#endif

            // 将FixedUpdate Tick放在此处，这样可以防止框架层FixedUpdate帧率小于帧同步FixedUpdate帧率而导致的一些问题
            self.FixedUpdate.Tick();
        }
    }

    public class LockStepStateFrameSyncComponentDestroySystem : DestroySystem<LockStepStateFrameSyncComponent>
    {
        public override void Destroy(LockStepStateFrameSyncComponent self)
        {
            self.FixedUpdate.UpdateCallback = null;
            self.FixedUpdate = null;
        }
    }

    public static class LockStepStateFrameSyncComponentSystem
    {
        /// <summary>
        /// 正式的帧同步Tick，所有的战斗逻辑都从这里出发
        /// </summary>
        /// <param name="self"></param>
        public static void LSF_Tick(this LockStepStateFrameSyncComponent self)
        {
            self.CurrentFrame++;

            Queue<Object> currentFrameCmdToHandle = self.FrameCmdsToHandle[self.CurrentFrame];

            foreach (var cmd in currentFrameCmdToHandle)
            {
                //TODO 处理客户端/服务端cmd
            }


            //TODO Tick Unit及其相关模块
        }

        public static void AddMessageToHandle(this LockStepStateFrameSyncComponent self, Object messageToHandle)
        {
#if SERVER
            C2M_FrameCmd cmd = messageToHandle as C2M_FrameCmd;
#else

            M2C_FrameCmd cmd = messageToHandle as M2C_FrameCmd;
#endif

            if (self.FrameCmdsToHandle.TryGetValue(self.CurrentFrame, out var queue))
            {
                queue.Enqueue(cmd);
            }
            else
            {
                Queue<Object> newQueue = new Queue<Object>();
                newQueue.Enqueue(cmd);
                self.FrameCmdsToHandle[self.CurrentFrame] = newQueue;
            }
        }

        public static void StartFrameSync(this LockStepStateFrameSyncComponent self)
        {
            self.StartSync = true;
        }
    }


    /// <summary>
    /// 根据Ping值的改变来改变Tick频率
    /// </summary>
    public class RTTChanged_ChangeFixedUpdateFPS : AEvent<EventType.PingChange>
    {
        protected override async ETTask Run(PingChange a)
        {
            LockStepStateFrameSyncComponent lockStepStateFrameSyncComponent = Game.Scene.GetComponent<PlayerComponent>()
                .BelongToRoom.GetComponent<LockStepStateFrameSyncComponent>();

            // 从游戏设计的角度来看，就算理应超前的帧数不满一帧也要按照一帧去算，因为玩家的指令肯定是越早到达服务器越好
            uint targetAheadOfFrame =
                (uint) Math.Ceiling(a.C2MPing / 2.0f / GlobalDefine.FixedUpdateTargetDTTime_Long) +
                lockStepStateFrameSyncComponent.BufferFrame;

            lockStepStateFrameSyncComponent.ServerCurrentFrame = a.ServerFrame;

            lockStepStateFrameSyncComponent.TargetAheadOfFrame =
                targetAheadOfFrame > LockStepStateFrameSyncComponent.AheadOfFrameMax
                    ? LockStepStateFrameSyncComponent.AheadOfFrameMax
                    : targetAheadOfFrame;

            await ETTask.CompletedTask;
        }
    }

    /// <summary>
    /// 服务端告诉进入地图后再开始Tick
    /// </summary>
    public class FinishEnterMap_BeginFrameSync : AEvent<EventType.FinishEnterMap>
    {
        protected override async ETTask Run(FinishEnterMap a)
        {
            LockStepStateFrameSyncComponent lockStepStateFrameSyncComponent = Game.Scene.GetComponent<PlayerComponent>()
                .BelongToRoom.GetComponent<LockStepStateFrameSyncComponent>();
            lockStepStateFrameSyncComponent.StartFrameSync();
            await ETTask.CompletedTask;
        }
    }
}