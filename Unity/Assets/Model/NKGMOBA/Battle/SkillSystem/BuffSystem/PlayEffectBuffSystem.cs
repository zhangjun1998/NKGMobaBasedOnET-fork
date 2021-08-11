//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月24日 14:15:24
//------------------------------------------------------------

using UnityEngine;

namespace ETModel
{
    public class PlayEffectBuffSystem: ABuffSystemBase<PlayEffectBuffData>
    {
        public override void OnExecute()
        {
            PlayEffect();
            if (this.BuffData.EventIds != null)
            {
                foreach (var eventId in this.BuffData.EventIds)
                {
                    Game.Scene.GetComponent<BattleEventSystem>().Run($"{eventId}{this.TheUnitFrom.Id}", this);
                    //Log.Info($"抛出了{this.MSkillBuffDataBase.theEventID}{this.theUnitFrom.Id}");
                }
            }
        }

        public override void OnFinished()
        {
            string targetEffectName = this.GetBuffDataWithTType.EffectName;
            if (this.GetBuffDataWithTType.CanChangeNameByCurrentOverlay)
            {
                targetEffectName = $"{this.GetBuffDataWithTType.EffectName}{this.CurrentOverlay}";
            }

            this.TheUnitBelongto.GetComponent<EffectComponent>()
                    .Remove(targetEffectName);
        }

        public override void OnRefreshed()
        {
            PlayEffect();
            if (this.BuffData.EventIds != null)
            {
                foreach (var eventId in this.BuffData.EventIds)
                {
                    Game.Scene.GetComponent<BattleEventSystem>().Run($"{eventId}{this.TheUnitFrom.Id}", this);
                    //Log.Info($"抛出了{this.MSkillBuffDataBase.theEventID}{this.theUnitFrom.Id}");
                }
            }
        }

        void PlayEffect()
        {
            string targetEffectName =  this.GetBuffDataWithTType.EffectName;

            if ( this.GetBuffDataWithTType.CanChangeNameByCurrentOverlay)
            {
                targetEffectName = $"{ this.GetBuffDataWithTType.EffectName}{this.CurrentOverlay}";
                //Log.Info($"播放{targetEffectName}");
            }

            //如果想要播放的特效正在播放，就返回
            if (this.TheUnitBelongto.GetComponent<EffectComponent>().CheckState(targetEffectName)) return;

            GameObjectPool gameObjectPool = Game.Scene.GetComponent<GameObjectPool>();

            if (!gameObjectPool.HasRegisteredPrefab(targetEffectName))
            {
                gameObjectPool.Add(targetEffectName,
                    this.TheUnitFrom.GameObject.GetComponent<ReferenceCollector>()
                            .Get<GameObject>(targetEffectName));
            }

            Unit effectUnit = gameObjectPool.FetchEntity(targetEffectName);

            if ( this.GetBuffDataWithTType.FollowUnit)
            {
                effectUnit.GameObject.transform.SetParent(this.GetBuffTarget().GetComponent<HeroTransformComponent>()
                        .GetTranform( this.GetBuffDataWithTType.PosType));

                effectUnit.GameObject.transform.localPosition = Vector3.zero;
            }

            this.TheUnitBelongto.GetComponent<EffectComponent>()
                    .Play(targetEffectName, effectUnit);
        }
    }
}