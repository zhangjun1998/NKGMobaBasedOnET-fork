//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年12月24日 21:26:46
//------------------------------------------------------------

using System.Collections.Generic;

namespace ETModel
{
    public class ReplaceAnimBuffSystem: ABuffSystemBase<ReplaceAnimBuffData>
    {
        /// <summary>
        /// 被替换下来的动画信息
        /// </summary>
        private Dictionary<string, string> m_ReplacedAnimData = new Dictionary<string, string>();

        public override void OnExecute()
        {
            AnimationComponent animationComponent = this.GetBuffTarget().GetComponent<AnimationComponent>();
            foreach (var animMapInfo in this.GetBuffDataWithTType.AnimReplaceInfo)
            {
                this.m_ReplacedAnimData[animMapInfo.StateType] = animationComponent.RuntimeAnimationClips[animMapInfo.StateType];
                animationComponent.RuntimeAnimationClips[animMapInfo.StateType] = animMapInfo.AnimName;
            }

            animationComponent.PlayAnimByStackFsmCurrent();
        }

        public override void OnFinished()
        {
            AnimationComponent animationComponent = this.GetBuffTarget().GetComponent<AnimationComponent>();
            foreach (var animMapInfo in m_ReplacedAnimData)
            {
                animationComponent.RuntimeAnimationClips[animMapInfo.Key] = animMapInfo.Value;
            }

            animationComponent.PlayAnimByStackFsmCurrent();
            m_ReplacedAnimData.Clear();
        }
    }
}