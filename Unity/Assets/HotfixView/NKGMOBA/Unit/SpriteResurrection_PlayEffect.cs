using ET.EventType;
using UnityEngine;

namespace ET
{
    /// <summary>
    /// 精灵如果复活了，那么肯定说明英雄处于存活状态
    /// </summary>
    public class SpriteResurrection_PlayEffect : AEvent<EventType.SpriteResurrection>
    {
        protected override async ETTask Run(SpriteResurrection a)
        {
            GameObject gameObject =
                GameObjectPoolComponent.Instance.FetchGameObject(ResInfos.SpwanEffect, GameObjectType.Effect);

            gameObject.transform.position = a.Sprite.Position;
            //显示精灵
            a.Sprite.GetComponent<GameObjectComponent>().GameObject.SetActive(true);

            Game.Scene.GetComponent<SoundComponent>().PlayClip("Sound_Spawn", 0.3f);

            GameObject resurrectionHero = a.Sprite.GetParent<Unit>().GetComponent<GameObjectComponent>().GameObject
                .GetComponent<ReferenceCollector>()
                .Get<GameObject>("Unit_Render");
            
            resurrectionHero.GetComponent<SkinnedMeshRenderer>().material.DisableKeyword("_Dead");

            resurrectionHero.GetComponent<SkinnedMeshRenderer>().material
                .SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.One);
            resurrectionHero.GetComponent<SkinnedMeshRenderer>().material
                .SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.Zero);
            resurrectionHero.GetComponent<SkinnedMeshRenderer>().material.renderQueue = 2000;

            await TimerComponent.Instance.WaitAsync(1000);
            GameObjectPoolComponent.Instance.RecycleGameObject(ResInfos.SpwanEffect, gameObject);

            await ETTask.CompletedTask;
        }
    }
}