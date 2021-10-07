using ET.EventType;

namespace ET
{
    public class NP_PlayAnimationActionEvent_Init : AEvent<EventType.NP_PlayAnimInitEvent>
    {
        protected override async ETTask Run(NP_PlayAnimInitEvent a)
        {
            //进行数据的装入
            foreach (var playAnimInfo in a.NodeDataForPlayAnims)
            {
                a.Target.GetComponent<AnimationComponent>().RuntimeAnimationClips[playAnimInfo.StateTypes] =
                    playAnimInfo.AnimationClipName;
            }

            await ETTask.CompletedTask;
        }
    }

    public class NP_PlayAnimationActionEvent_Init2 : AEvent<EventType.NP_PlayAnimDoEvent>
    {
        protected override async ETTask Run(NP_PlayAnimDoEvent a)
        {
            //在播放完成后，每帧都会调用OnEnd委托，由于行为树中的FixedUpdate与Unity的Update频率不一致，所以需要作特殊处理
            a.Target.GetComponent<AnimationComponent>().PlayAnim(a.StateTypes, a.FadeDuration).OnEnd = a.AnimEnd;
            await ETTask.CompletedTask;
        }
    }

    public class NP_PlayAnimationActionEvent_Init3 : AEvent<EventType.NP_PlayAnimEndEvent>
    {
        protected override async ETTask Run(NP_PlayAnimEndEvent a)
        {
            a.Target.GetComponent<AnimationComponent>().PlayAnimByStackFsmCurrent();
            await ETTask.CompletedTask;
        }
    }
}