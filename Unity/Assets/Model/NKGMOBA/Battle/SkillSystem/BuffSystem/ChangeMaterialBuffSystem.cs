//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年5月12日 19:22:48
//------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace ETModel
{
    public class ChangeMaterialBuffSystem: ABuffSystemBase<ChangeMaterialBuffData>
    {
        /// <summary>
        /// 自身下一个时间点
        /// </summary>
        private long m_SelfNextimer;

        public override void OnExecute()
        {
            SkinnedMeshRenderer skinnedMeshRenderer = this.GetBuffTarget().GameObject.GetRCInternalComponent<SkinnedMeshRenderer>("Materials");

            List<Material> currentMats = new List<Material>();
            skinnedMeshRenderer.GetSharedMaterials(currentMats);

            foreach (var changeMaterialName in this.GetBuffDataWithTType.TheMaterialNameWillBeAdded)
            {
                currentMats.Add(this.GetBuffTarget().GameObject.GetComponent<ReferenceCollector>().Get<Material>(changeMaterialName));
            }

            skinnedMeshRenderer.sharedMaterials = currentMats.ToArray();
        }

        public override void OnFinished()
        {
            SkinnedMeshRenderer skinnedMeshRenderer = this.GetBuffTarget().GameObject.GetRCInternalComponent<SkinnedMeshRenderer>("Materials");

            List<Material> currentMats = new List<Material>();
            skinnedMeshRenderer.GetSharedMaterials(currentMats);

            foreach (var changeMaterialName in this.GetBuffDataWithTType.TheMaterialNameWillBeAdded)
            {
                for (int i = currentMats.Count - 1; i >= 0; i--)
                {
                    if (currentMats[i].name == changeMaterialName)
                    {
                        currentMats.RemoveAt(i);
                    }
                }
            }

            skinnedMeshRenderer.sharedMaterials = currentMats.ToArray();
        }
    }
}