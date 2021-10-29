using System;
using System.Collections.Generic;
using ET.EventType;
using NPBehave_Core;

namespace ET
{
    public class LockStepStateFrameSyncComponentAwakeSystem : AwakeSystem<LSF_Component>
    {
        public override void Awake(LSF_Component self)
        {
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

            // 将FixedUpdate Tick放在此处，这样可以防止框架层FixedUpdate帧率小于帧同步FixedUpdate帧率而导致的一些问题
            self.FixedUpdate.Tick();
#if !SERVER

            // 当前客户端帧数大于服务端帧数，两种情况，
            // 1.正常情况，客户端为了保证自己的消息在合适的时间点抵达服务端需要领先于服务器
            // 2.非正常情况，客户端由于网络延迟或者断开导致没有收到服务端的帧指令，导致ServerCurrentFrame长时间没有更新，会导致CurrentAheadOfFrame越来越大，当达到一个阈值的时候将会进行断线重连
            if (self.CurrentFrame > self.ServerCurrentFrame)
            {
                self.CurrentAheadOfFrame = (int)(self.CurrentFrame - self.ServerCurrentFrame);

                if (self.CurrentAheadOfFrame > LSF_Component.AheadOfFrameMax)
                {
                    //TODO 开始断线重连
                    Log.Error("长时间未收到服务端回包，开始断线重连");
                    
                    int count = self.TargetAheadOfFrame;
                    self.CurrentFrame = self.ServerCurrentFrame;
                    while (count-- > 0)
                    {
                        self.LSF_Tick();
                    }
                    return;
                }
            }
            else // 当前客户端帧数小于服务端帧数，两种情况，1，刚开局，2，玩家退游戏重连
            {
                self.CurrentAheadOfFrame = -(int)(self.ServerCurrentFrame - self.CurrentFrame);

                if (-self.CurrentAheadOfFrame > LSF_Component.BehindOfFrameMax)
                {
                    //TODO 开始断线重连
                    Log.Error("突然收到服务器回包，发现自己远落后于服务端，开始断线重连");
                    self.CurrentFrame = self.ServerCurrentFrame;
                    int count = self.TargetAheadOfFrame;
                    while (count-- > 0)
                    {
                        self.LSF_Tick();
                    }
                    return;
                }
            }

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