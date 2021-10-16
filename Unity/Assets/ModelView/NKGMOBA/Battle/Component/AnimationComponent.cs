//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月12日 16:08:44
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using Animancer;
using ET;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    public class AnimationComponentAwakeSystem : AwakeSystem<AnimationComponent>
    {
        public override void Awake(AnimationComponent self)
        {
            GameObject gameObject = self.GetParent<Unit>().GetComponent<GameObjectComponent>().GameObject;
            self.AnimancerComponent = gameObject.GetComponent<AnimancerComponent>();
            self.StackFsmComponent = self.GetParent<Unit>().GetComponent<StackFsmComponent>();
            //如果是以Anim开头的key值，说明是动画文件，需要添加引用
            foreach (var referenceCollectorData in gameObject.GetComponent<ReferenceCollector>().data)
            {
                if (referenceCollectorData.key.StartsWith("Anim"))
                {
                    self.AnimationClips.Add(referenceCollectorData.key,
                        referenceCollectorData.gameObject as AnimationClip);
                }

                if (referenceCollectorData.key.StartsWith("AnimMask"))
                {
                    self.AvatarMasks.Add(referenceCollectorData.key, referenceCollectorData.gameObject as AvatarMask);
                }
            }

            self.AnimancerComponent.Layers[(int) PlayAnimInfo.AvatarMaskType.AnimMask_UpNotAffect]
                .SetMask(self.AvatarMasks[PlayAnimInfo.AvatarMaskType.AnimMask_UpNotAffect.ToString()]);
            self.AnimancerComponent.Layers[(int) PlayAnimInfo.AvatarMaskType.AnimMask_DownNotAffect]
                .SetMask(self.AvatarMasks[PlayAnimInfo.AvatarMaskType.AnimMask_DownNotAffect.ToString()]);


            self.PlayIdelFromStart();
        }
    }

    public class AnimationComponentDestroySystem : DestroySystem<AnimationComponent>
    {
        public override void Destroy(AnimationComponent self)
        {
            self.AnimancerComponent = null;
            self.StackFsmComponent = null;
            self.AnimationClips.Clear();
            self.RuntimeAnimationClips.Clear();

            self.CommonAnimState = null;
            self.SkillAnimState = null;
        }
    }

    /// <summary>
    /// 基于Animancer插件做的动画机系统。配合可视化编辑使用效果更佳
    /// </summary>
    public class AnimationComponent : Entity
    {
        /// <summary>
        /// Animacner的组件
        /// </summary>
        public AnimancerComponent AnimancerComponent;

        /// <summary>
        /// 栈式状态机组件，用于辅助切换动画
        /// </summary>
        public StackFsmComponent StackFsmComponent;

        public Dictionary<string, AvatarMask> AvatarMasks = new Dictionary<string, AvatarMask>();

        /// <summary>
        /// 管理所有的动画文件
        /// </summary>
        public Dictionary<string, AnimationClip> AnimationClips = new Dictionary<string, AnimationClip>();

        /// <summary>
        /// 通常的动画状态，例如默认，行走，攻击等
        /// </summary>
        public AnimancerState CommonAnimState;

        /// <summary>
        /// 技能动画状态，例如诺手Q技能动画
        /// </summary>
        public AnimancerState SkillAnimState;

        /// <summary>
        /// 运行时所播放的动画文件，会动态变化
        /// 例如移动速度快到一定程度将会播放另一种跑路动画，这时候就需要动态替换RuntimeAnimationClips的Run所对应的VALUE
        /// KEY:外部调用的名称
        /// VALEU：对应AnimationClips中的KEY，可以取得相应的动画文件
        /// </summary>
        public Dictionary<string, string> RuntimeAnimationClips = new Dictionary<string, string>
        {
            {StateTypes.Run.GetStateTypeMapedString(), "Anim_Run1"},
            {StateTypes.Idle.GetStateTypeMapedString(), "Anim_Idle1"},
            {StateTypes.CommonAttack.GetStateTypeMapedString(), "Anim_Attack1"}
        };

        public AnimancerState PlaySkillAnim(string stateTypes,
            PlayAnimInfo.AvatarMaskType avatarMaskType = PlayAnimInfo.AvatarMaskType.None,
            float fadeDuration = 0.25f, float speed = 1.0f, FadeMode fadeMode = FadeMode.FixedDuration)
        {
            AnimancerState animancerState = null;

            if (avatarMaskType == PlayAnimInfo.AvatarMaskType.AnimMask_DownNotAffect &&
                this.StackFsmComponent.GetCurrentFsmState().StateTypes == StateTypes.Run)
            {
                animancerState = AnimancerComponent.Layers[(int) avatarMaskType]
                    .Play(this.AnimationClips[RuntimeAnimationClips[stateTypes]], fadeDuration, fadeMode);
            }
            else
            {
                animancerState = PlayCommonAnim_Internal(stateTypes, avatarMaskType, fadeDuration, speed, fadeMode);
            }

            this.SkillAnimState = animancerState;
            return animancerState;
        }

        /// <summary>
        /// 播放一个动画(播放完成自动回到默认动画)
        /// </summary>
        /// <param name="stateTypes"></param>
        /// <returns></returns>
        public void PlayAnimAndReturnIdelFromStart(StateTypes stateTypes, float fadeDuration = 0.25f,
            float speed = 1.0f, FadeMode fadeMode = FadeMode.FixedDuration)
        {
            PlayCommonAnim(stateTypes, SkillAnimState is {IsPlaying: true}
                ? PlayAnimInfo.AvatarMaskType.AnimMask_UpNotAffect
                : PlayAnimInfo.AvatarMaskType.None, fadeDuration, speed, fadeMode).Events.OnEnd = PlayIdelFromStart;
        }

        /// <summary>
        /// 播放默认动画如果在此期间再次播放，则会从头开始
        /// </summary>
        public void PlayIdelFromStart()
        {
            this.CommonAnimState = AnimancerComponent.Play(
                this.AnimationClips[RuntimeAnimationClips[StateTypes.Idle.GetStateTypeMapedString()]], 0.25f,
                FadeMode.FromStart);
        }

        /// <summary>
        /// 根据栈式状态机来自动播放动画
        /// </summary>
        public void PlayAnimByStackFsmCurrent(float fadeDuration = 0.25f, float speed = 1.0f,
            bool afterSkillStateStartFade = false)
        {
            //先根据StateType进行动画播放
            if (this.RuntimeAnimationClips.ContainsKey(
                this.StackFsmComponent.GetCurrentFsmState().StateTypes.ToString()))
            {
                if (SkillAnimState is {IsPlaying: true})
                {
                    if (CommonAnimState.LayerIndex == (int) PlayAnimInfo.AvatarMaskType.None &&
                        afterSkillStateStartFade)
                    {
                        return;
                    }

                    // 如果先释放技能再寻路，且技能尚未播放完成，就会保持上半身不变
                    CommonAnimState = PlayCommonAnim(this.StackFsmComponent.GetCurrentFsmState().StateTypes,
                        PlayAnimInfo.AvatarMaskType.AnimMask_UpNotAffect,
                        fadeDuration, speed);
                }
                else
                {
                    // 因为先释放技能再寻路，且技能尚未播放完成，就会保持上半身不变，走的是另一个Layer，所以要先停掉
                    if (CommonAnimState.LayerIndex == (int) PlayAnimInfo.AvatarMaskType.AnimMask_UpNotAffect)
                    {
                        CommonAnimState.Stop();
                    }

                    CommonAnimState = PlayCommonAnim(this.StackFsmComponent.GetCurrentFsmState().StateTypes,
                        PlayAnimInfo.AvatarMaskType.None,
                        fadeDuration, speed);
                }
            }
            //否则播放默认动画
            else
            {
                this.PlayIdelFromStart();
            }
        }

        /// <summary>
        /// 技能动画的Layer是否应当立即Stop，这种情况发生在先释放技能，后进行移动的时候
        /// </summary>
        /// <returns></returns>
        public bool SkillStateShouldStopImmdiately()
        {
            return CommonAnimState.LayerIndex == (int) PlayAnimInfo.AvatarMaskType.AnimMask_UpNotAffect;
        }

        #region PRIVATE

        /// <summary>
        /// 播放一个动画,默认过渡时间为0.25s
        /// </summary>
        /// <param name="stateTypes"></param>
        /// <param name="fadeDuration">动画过渡时间</param>
        /// <returns></returns>
        private AnimancerState PlayCommonAnim_Internal(string stateTypes,
            PlayAnimInfo.AvatarMaskType avatarMaskType = PlayAnimInfo.AvatarMaskType.None,
            float fadeDuration = 0.25f, float speed = 1.0f, FadeMode fadeMode = FadeMode.FixedDuration)
        {
            AnimancerState animancerState = null;

            animancerState = AnimancerComponent.Layers[(int) avatarMaskType]
                .Play(this.AnimationClips[RuntimeAnimationClips[stateTypes]], fadeDuration, fadeMode);
            animancerState.Speed = speed;

            return animancerState;
        }


        /// <summary>
        /// 播放一个动画,默认过渡时间为0.25s，如果在此期间再次播放，则会继续播放
        /// </summary>
        /// <param name="stateTypes"></param>
        /// <param name="fadeDuration">动画过渡时间</param>
        /// <returns></returns>
        private AnimancerState PlayCommonAnim(StateTypes stateTypes,
            PlayAnimInfo.AvatarMaskType avatarMaskType = PlayAnimInfo.AvatarMaskType.None,
            float fadeDuration = 0.25f, float speed = 1.0f, FadeMode fadeMode = FadeMode.FixedDuration)
        {
            return PlayCommonAnim_Internal(stateTypes.GetStateTypeMapedString(), avatarMaskType, fadeDuration, speed,
                fadeMode);
        }

        #endregion
    }
}