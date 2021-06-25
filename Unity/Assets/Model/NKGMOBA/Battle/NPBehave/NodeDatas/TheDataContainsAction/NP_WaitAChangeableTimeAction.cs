//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月26日 19:47:29
//------------------------------------------------------------

using System;
using NPBehave;
using Sirenix.OdinInspector;
using Action = NPBehave.Action;

namespace ETModel
{
    /// <summary>
    /// 等待一个可变化的时间，比如CD变化
    /// </summary>
    [Title("等待一个可变化的时间", TitleAlignment = TitleAlignments.Centered)]
    public class NP_WaitAChangeableTimeAction: NP_ClassForStoreAction
    {
        [LabelText("等待的时长")]
        public NP_BlackBoardRelationData TheTimeToWait = new NP_BlackBoardRelationData();

        private double lastElapsedTime;

        private Blackboard tempBlackboard;

        public override Func<bool, Action.Result> GetFunc2ToBeDone()
        {
            this.Func2 = WaitTime;
            return this.Func2;
        }

        public Action.Result WaitTime(bool hasDown)
        {
            this.lastElapsedTime = SyncContext.Instance.GetClock().ElapsedTime;
            tempBlackboard.Set(this.TheTimeToWait.BBKey,
                tempBlackboard.Get<float>(this.TheTimeToWait.BBKey) -
                (float) (SyncContext.Instance.GetClock().ElapsedTime - lastElapsedTime));
            if (tempBlackboard.Get<float>(this.TheTimeToWait.BBKey) <= 0)
            {
                lastElapsedTime = -1;
                return NPBehave.Action.Result.SUCCESS;
            }

            return NPBehave.Action.Result.PROGRESS;
        }
    }
}