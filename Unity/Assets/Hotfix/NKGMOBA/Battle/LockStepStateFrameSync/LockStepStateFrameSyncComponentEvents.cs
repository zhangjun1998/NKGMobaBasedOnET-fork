using System;
using System.Collections.Generic;
using ET.EventType;


namespace ET
{
#if SERVER

#else
        /// <summary>
    /// 根据Ping值的改变来改变Tick频率
    /// </summary>
    public class RTTChanged_ChangeFixedUpdateFPS : AEvent<EventType.PingChange>
    {
        protected override async ETTask Run(PingChange a)
        {
            LockStepStateFrameSyncComponent lockStepStateFrameSyncComponent = Game.Scene.GetComponent<PlayerComponent>()
                .BelongToRoom.GetComponent<LockStepStateFrameSyncComponent>();

            lockStepStateFrameSyncComponent.HalfRTT = a.C2MPing;

            // 从游戏设计的角度来看，就算理应超前的帧数不满一帧也要按照一帧去算，因为玩家的指令肯定是越早到达服务器越好
            uint targetAheadOfFrame =
                (uint) Math.Ceiling(a.C2MPing / 2.0f / GlobalDefine.FixedUpdateTargetDTTime_Long) +
                lockStepStateFrameSyncComponent.BufferFrame;

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
#endif
}