//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年6月15日 11:19:33
//------------------------------------------------------------

using System;
using System.IO;
using ETModel;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Plugins.NodeEditor
{
    public class SkillGraph: NPBehaveGraph
    {
        [BoxGroup("此技能树数据载体")]
        public NP_DataSupportor SkillDataSupportor = new NP_DataSupportor();
        
        [BoxGroup("技能树反序列化测试")]
        public NP_DataSupportor SkillDataSupportor1 = new NP_DataSupportor();
        
        [Button("自动配置所有结点数据", 25), GUIColor(0.4f, 0.8f, 1)]
        public void AutoSetCanvasDatas()
        {
            base.AutoSetCanvasDatas();
            SkillDataSupportor.NpDataSupportorBase = this.NpDataSupportor;
            this.AutoSetSkillData_NodeData();
        }
        
        [Button("保存行为树信息为二进制文件", 25), GUIColor(0.4f, 0.8f, 1)]
        public void Save()
        {
            if (string.IsNullOrEmpty(SavePath) || string.IsNullOrEmpty(Name))
            {
                Log.Error($"保存路径或文件名不能为空，请检查配置");
                return;
            }
        
            using (FileStream file = File.Create($"{SavePath}/{this.Name}.bytes"))
            {
                BsonSerializer.Serialize(new BsonBinaryWriter(file), SkillDataSupportor);
            }
            
            Log.Info($"保存 {SavePath}/{this.Name}.bytes 成功");
        }
        
        [Button("测试反序列化", 25), GUIColor(0.4f, 0.8f, 1)]
        public void TestDe()
        {
            byte[] mfile = File.ReadAllBytes($"{SavePath}/{this.Name}.bytes");
        
            if (mfile.Length == 0) Log.Info("没有读取到文件");
        
            try
            {
                SkillDataSupportor1 = BsonSerializer.Deserialize<NP_DataSupportor>(mfile);
                this.NpDataSupportor1 = SkillDataSupportor1.NpDataSupportorBase;
            }
            catch (Exception e)
            {
                Log.Info(e.ToString());
                throw;
            }
        }
        
        private void AutoSetSkillData_NodeData()
        {
            if (SkillDataSupportor.BuffNodeDataDic == null) return;
            SkillDataSupportor.BuffNodeDataDic.Clear();
        
            foreach (var node in this.nodes)
            {
                if (node is BuffNodeBase mNode)
                {
                    mNode.AutoAddLinkedBuffs();
                    BuffNodeDataBase buffNodeDataBase = mNode.Skill_GetNodeData();
                    if (buffNodeDataBase is NormalBuffNodeData normalBuffNodeData)
                    {
                        normalBuffNodeData.BuffData.BelongToBuffDataSupportorId = this.SkillDataSupportor.NpDataSupportorBase.NPBehaveTreeDataId;
                    }
                
                    this.SkillDataSupportor.BuffNodeDataDic.Add(buffNodeDataBase.NodeId.Value, buffNodeDataBase);
                }
            }
        }
    }
}