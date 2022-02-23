using System;
using System.Collections.Generic;
using System.Diagnostics;
using ET;
using NPBehave_Core;

namespace NPBehave
{
    public class Clock
    {
        private bool isInUpdate = false;

        public uint CurrentFrame;

        /// <summary>
        /// 将要被添加的帧事件
        /// </summary>
        private Dictionary<long, FrameAction> ToBeAddedFrameActions = new Dictionary<long, FrameAction>();

        /// <summary>
        /// 将要被移除的帧事件
        /// </summary>
        private List<long> ToBeRemovedFrameActions = new List<long>();

        /// <summary>
        /// 所有的帧事件
        /// </summary>
        private Dictionary<long, FrameAction> AllFrameActions = new Dictionary<long, FrameAction>();

        /// <summary>Register a timer function, 因为这个函数可能会在回滚的时候调用，所以要强行传递一个currentFrame</summary>
        /// <param name="intervalFrame">time in Frame</param>
        /// <param name="repeat">number of times to repeat, set to -1 to repeat until unregistered.</param>
        /// <param name="action">method to invoke</param>
        public long AddTimer(uint intervalFrame, System.Action action, int repeat = 1, uint currentFrame = 0)
        {
            FrameAction frameAction = ReferencePool.Acquire<FrameAction>();
            frameAction.Id = IdGenerater.Instance.GenerateId();
            frameAction.Action = action;
            frameAction.RepeatTime = repeat;
            frameAction.IntervalFrame = intervalFrame;

            AddTimer(frameAction, currentFrame);

            return frameAction.Id;
        }

        /// <summary>
        /// 因为这个函数可能会在回滚的时候调用，所以要强行传递一个currentFrame
        /// </summary>
        /// <param name="frameAction"></param>
        /// <param name="currentFrame"></param>
        private void AddTimer(FrameAction frameAction, uint currentFrame = 0)
        {
            CalculateTimerFrame(frameAction, currentFrame);
            if (!isInUpdate)
            {
                if (!this.AllFrameActions.ContainsKey(frameAction.Id))
                {
                    AllFrameActions.Add(frameAction.Id, frameAction);
                }
            }
            else
            {
                this.ToBeAddedFrameActions[frameAction.Id] = frameAction;
            }
        }

        public void RemoveTimer(long id)
        {
            if (!isInUpdate)
            {
                if (this.AllFrameActions.TryGetValue(id, out var frameAction))
                {
                    ReferencePool.Release(frameAction);
                    this.AllFrameActions.Remove(id);
                }
            }
            else
            {
                this.ToBeRemovedFrameActions.Add(id);
            }
        }

        public void Update(uint currentFrame)
        {
            this.isInUpdate = true;
            this.CurrentFrame = currentFrame;

            foreach (var frameActionPair in AllFrameActions)
            {
                FrameAction frameAction = frameActionPair.Value;
                if (frameAction.TargetTickFrame <= CurrentFrame)
                {
                    frameAction.Action.Invoke();

                    if (frameAction.RepeatTime != -1 && --frameAction.RepeatTime <= 0)
                    {
                        RemoveTimer(frameAction.Id);
                    }
                    else
                    {
                        CalculateTimerFrame(frameAction);
                    }
                }
            }

            this.isInUpdate = false;

            foreach (var frameActionId in this.ToBeRemovedFrameActions)
            {
                // 如果ToBeAddedFrameActions中也有这个FrameAction，说明是一帧内移除并添加的，不作真正的移除，跳过
                if (!ToBeAddedFrameActions.ContainsKey(frameActionId))
                {
                    RemoveTimer(frameActionId);
                }
            }

            foreach (var frameActionPair in this.ToBeAddedFrameActions)
            {
                AddTimer(frameActionPair.Value);
            }

            this.ToBeAddedFrameActions.Clear();
            this.ToBeRemovedFrameActions.Clear();
        }

        /// <summary>
        /// 支持传递一个自定义currentFrame进来，用于回滚时精确调用
        /// </summary>
        /// <param name="frameAction"></param>
        /// <param name="currentFrame"></param>
        private void CalculateTimerFrame(FrameAction frameAction, uint currentFrame = 0)
        {
            if (currentFrame == 0)
            {
                frameAction.TargetTickFrame = CurrentFrame + frameAction.IntervalFrame;
            }
            else
            {
                frameAction.TargetTickFrame = currentFrame + frameAction.IntervalFrame;
            }
        }
    }
}