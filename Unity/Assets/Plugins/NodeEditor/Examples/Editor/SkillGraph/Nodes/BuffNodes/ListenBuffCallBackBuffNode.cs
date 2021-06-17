//------------------------------------------------------------
// 此代码由工具自动生成，请勿更改
// 此代码由工具自动生成，请勿更改
// 此代码由工具自动生成，请勿更改
//------------------------------------------------------------

using System.Collections.Generic;
using ETModel;
using GraphProcessor;
using UnityEditor;

namespace Plugins.NodeEditor
{
    [NodeMenuItem("技能数据部分/监听Buff", typeof (SkillGraph))]
    public class ListenBuffCallBackBuffNode: BuffNodeBase
    {
        public override string name => "监听Buff";

        public NormalBuffNodeData SkillBuffBases =
                new NormalBuffNodeData()
                {
                    BuffDes = "监听Buff",
                    BuffData = new ListenBuffCallBackBuffData() { BelongBuffSystemType = BuffSystemType.ListenBuffCallBackBuffSystem }
                };

        public override BuffNodeDataBase Skill_GetNodeData()
        {
            return SkillBuffBases;
        }

        public override void AutoAddLinkedBuffs()
        {
            ListenBuffCallBackBuffData listenBuffCallBackBuffData = SkillBuffBases.BuffData as ListenBuffCallBackBuffData;
            if (listenBuffCallBackBuffData.ListenBuffEventNormal == null)
            {
                listenBuffCallBackBuffData.ListenBuffEventNormal = new ListenBuffEvent_Normal();
            }

            //备份Buff Id和对应层数键值对，防止被覆写
            Dictionary<long, int> buffDataBack = new Dictionary<long, int>();

            foreach (var vtdBuffInfo in listenBuffCallBackBuffData.ListenBuffEventNormal.BuffInfoWillBeAdded)
            {
                buffDataBack[vtdBuffInfo.BuffNodeId.Value] = vtdBuffInfo.Layers;
            }

            listenBuffCallBackBuffData.ListenBuffEventNormal.BuffInfoWillBeAdded.Clear();

            foreach (var outputNode in this.GetOutputNodes())
            {
                BuffNodeBase targetNode = (outputNode as BuffNodeBase);
                if (targetNode != null)
                {
                    listenBuffCallBackBuffData.ListenBuffEventNormal.BuffInfoWillBeAdded.Add(new VTD_BuffInfo()
                    {
                        BuffNodeId = targetNode.Skill_GetNodeData().NodeId
                    });
                }
            }

            foreach (var vtdBuffInfo in listenBuffCallBackBuffData.ListenBuffEventNormal.BuffInfoWillBeAdded)
            {
                if (buffDataBack.TryGetValue(vtdBuffInfo.BuffNodeId.Value, out var buffLayer))
                {
                    vtdBuffInfo.Layers = buffLayer;
                }
            }
        }
    }
}