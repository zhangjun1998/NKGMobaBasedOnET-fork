//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年5月12日 18:19:55
//------------------------------------------------------------

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ETModel
{
    public class ChangeRenderAssetBuffSystem: ABuffSystemBase<ChangeRenderAssetBuffData>
    {
        /// <summary>
        /// 自身下一个时间点
        /// </summary>
        private long m_SelfNextimer;

        public override void OnExecute()
        {
            SetScriptableRendererFeatureState(true);
            //Log.Info($"作用间隔为{selfNextimer - TimeHelper.Now()},持续时间为{temp.SustainTime},持续到{this.selfNextimer}");
        }
        
        public override void OnFinished()
        {
            SetScriptableRendererFeatureState(false);
        }

        private void SetScriptableRendererFeatureState(bool state)
        {
            foreach (var renderFeatureNameToActive in this.GetBuffDataWithTType.RenderFeatureNameToActive)
            {
                ForwardRenderBridge.Instance.SetScriptableRendererFeatureState(renderFeatureNameToActive, state);
            }
        }
    }
}