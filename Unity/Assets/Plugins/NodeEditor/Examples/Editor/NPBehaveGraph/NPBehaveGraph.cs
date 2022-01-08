//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年6月15日 11:19:33
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ET;
using GraphProcessor;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Plugins.NodeEditor
{
    public class NPBehaveGraph : BaseGraph
    {
        [BoxGroup("本Canvas所有数据整理部分")] [LabelText("保存文件名"), GUIColor(0.9f, 0.7f, 1)]
        public string Name;

        [BoxGroup("本Canvas所有数据整理部分")] [LabelText("对应的配置表"), GUIColor(0.9f, 0.7f, 1)]
        public TextAsset Config;

        [BoxGroup("本Canvas所有数据整理部分")] [LabelText("对应的配置表类型"), GUIColor(0.9f, 0.7f, 1)] [ValueDropdown("GetConfigTypes")]
        public Type ConfigType;

        [BoxGroup("本Canvas所有数据整理部分")] [LabelText("配置表中的Id"), GUIColor(0.9f, 0.7f, 1)]
        public int IdInConfig;

        [BoxGroup("本Canvas所有数据整理部分")] [LabelText("保存路径(服务端)"), GUIColor(0.1f, 0.7f, 1)] [FolderPath]
        public string SavePathServer;
        
        [BoxGroup("本Canvas所有数据整理部分")] [LabelText("保存路径(客户端)"), GUIColor(0.1f, 0.7f, 1)] [FolderPath]
        public string SavePathClient;

        [BoxGroup("此行为树数据载体")] [DisableInEditorMode]
        public NP_DataSupportorBase NpDataSupportor = new NP_DataSupportorBase();

        [BoxGroup("行为树反序列化测试")] [DisableInEditorMode]
        public NP_DataSupportorBase NpDataSupportor1 = new NP_DataSupportorBase();

        /// <summary>
        /// 黑板数据管理器
        /// </summary>
        [HideInInspector] public NP_BlackBoardDataManager NpBlackBoardDataManager = new NP_BlackBoardDataManager();

        /// <summary>
        /// 自动配置当前图所有数据（结点，黑板）
        /// </summary>
        /// <param name="npDataSupportorBase">自定义的继承于NP_DataSupportorBase的数据体</param>
        [Button("自动配置所有结点数据", 25), GUIColor(0.4f, 0.8f, 1)]
        public virtual void AutoSetCanvasDatas()
        {
            if (this.NpDataSupportor == null)
            {
                return;
            }
            
            this.OnGraphEnable();
            NP_BlackBoardHelper.SetCurrentBlackBoardDataManager(this);
            
            this.AutoSetNP_NodeData(this.NpDataSupportor);
            this.AutoSetNP_BBDatas(this.NpDataSupportor);
        }

        [Button("保存行为树信息为二进制文件", 25), GUIColor(0.4f, 0.8f, 1)]
        public void Save()
        {
            if (string.IsNullOrEmpty(SavePathServer) || string.IsNullOrEmpty(SavePathClient) || string.IsNullOrEmpty(Name))
            {
                Log.Error($"保存路径或文件名不能为空，请检查配置");
                return;
            }

            using (FileStream file = File.Create($"{SavePathServer}/{this.Name}.bytes"))
            {
                BsonSerializer.Serialize(new BsonBinaryWriter(file), NpDataSupportor);
            }

            if (File.Exists($"{SavePathClient}/{this.Name}.bytes"))
            {
                File.Delete($"{SavePathClient}/{this.Name}.bytes");
            }

            File.Copy($"{SavePathServer}/{this.Name}.bytes", $"{SavePathClient}/{this.Name}.bytes", true);
            Log.Info($"保存 {SavePathServer}/{this.Name}.bytes {SavePathClient}/{this.Name}.bytes 成功");
        }

        [Button("测试反序列化", 25), GUIColor(0.4f, 0.8f, 1)]
        public void TestDe()
        {
            try
            {
                MongoHelper.Init();
                this.NpDataSupportor1 = null;
                using (var fs = new FileStream($"{SavePathServer}/{this.Name}.bytes", FileMode.Open, FileAccess.Read,
                    FileShare.Read))
                using (var reader = new BinaryReader(fs))
                {
                    this.NpDataSupportor1 =
                        SerializationUtility.DeserializeValue<NP_DataSupportorBase>(reader.BaseStream,
                            DataFormat.Binary);
                }
            }
            catch (Exception e)
            {
                Log.Info(e.ToString());
                throw;
            }
        }

        /// <summary>
        /// 自动配置所有行为树结点
        /// </summary>
        /// <param name="npDataSupportorBase">自定义的继承于NP_DataSupportorBase的数据体</param>
        private void AutoSetNP_NodeData(NP_DataSupportorBase npDataSupportorBase)
        {
            if (npDataSupportorBase == null)
            {
                return;
            }

            npDataSupportorBase.NPBehaveTreeDataId = 0;
            npDataSupportorBase.NP_DataSupportorDic.Clear();

            //当前Canvas所有NP_Node
            List<NP_NodeBase> allNodes = new List<NP_NodeBase>();

            foreach (var node in this.nodes)
            {
                if (node is NP_NodeBase mNode)
                {
                    allNodes.Add(mNode);
                }
            }

            //排序
            allNodes.Sort((x, y) => -x.position.y.CompareTo(y.position.y));

            //配置每个节点Id
            foreach (var node in allNodes)
            {
                node.NP_GetNodeData().id = IdGenerater.Instance.GenerateId();
            }

            if (this.Config == null)
            {
                return;
            }

            object config = ProtobufHelper.FromBytes(this.ConfigType, this.Config.bytes, 0, this.Config.bytes.Length);

            //目前行为树只有三种类型，直接在这里写出
            switch (config)
            {
                case SkillCanvasConfigCategory skillCanvasConfigCategory:
                    skillCanvasConfigCategory.AfterDeserialization();
                    SkillCanvasConfig skillCanvasConfig = skillCanvasConfigCategory.Get(this.IdInConfig);
                    if (skillCanvasConfig != null)
                    {
                        npDataSupportorBase.NPBehaveTreeDataId = skillCanvasConfig.NPBehaveId;
                    }

                    break;
                case Server_AICanvasConfigCategory serverSkillCanvasConfigCategory:
                    serverSkillCanvasConfigCategory.AfterDeserialization();
                    Server_AICanvasConfig serverAICanvasConfig = serverSkillCanvasConfigCategory.Get(this.IdInConfig);
                    if (serverAICanvasConfig != null)
                    {
                        npDataSupportorBase.NPBehaveTreeDataId = serverAICanvasConfig.NPBehaveId;
                    }

                    break;
            }

            if (npDataSupportorBase.NPBehaveTreeDataId == 0)
            {
                //设置为根结点Id
                npDataSupportorBase.NPBehaveTreeDataId = allNodes[allNodes.Count - 1].NP_GetNodeData().id;
                Log.Error(
                    $"注意，名为{this.Name}的Graph首次导出，或者未在配置表中找到Id为{this.IdInConfig}的数据行，行为树Id被设置为{npDataSupportorBase.NPBehaveTreeDataId}，请前往Excel表中进行添加，然后导出Excel");
            }
            else
            {
                allNodes[allNodes.Count - 1].NP_GetNodeData().id = npDataSupportorBase.NPBehaveTreeDataId;
            }

            foreach (var node in allNodes)
            {
                //获取结点对应的NPData
                NP_NodeDataBase mNodeData = node.NP_GetNodeData();
                if (mNodeData.LinkedIds == null)
                {
                    mNodeData.LinkedIds = new List<long>();
                }

                mNodeData.LinkedIds.Clear();

                //出结点连接的Nodes
                List<NP_NodeBase> theNodesConnectedToOutNode = new List<NP_NodeBase>();

                foreach (var outputNode in node.GetOutputNodes())
                {
                    theNodesConnectedToOutNode.Add(outputNode as NP_NodeBase);
                }

                //对所连接的节点们进行排序
                theNodesConnectedToOutNode.Sort((x, y) => x.position.x.CompareTo(y.position.x));

                //配置连接的Id，运行时实时构建行为树
                foreach (var npNodeBase in theNodesConnectedToOutNode)
                {
                    mNodeData.LinkedIds.Add(npNodeBase.NP_GetNodeData().id);
                }

                //将此结点数据写入字典
                npDataSupportorBase.NP_DataSupportorDic.Add(mNodeData.id, mNodeData);
            }
        }

        /// <summary>
        /// 自动配置黑板数据
        /// </summary>
        /// <param name="npDataSupportorBase">自定义的继承于NP_DataSupportorBase的数据体</param>
        private void AutoSetNP_BBDatas(NP_DataSupportorBase npDataSupportorBase)
        {
            npDataSupportorBase.NP_BBValueManager.Clear();
            //设置黑板数据
            foreach (var bbvalues in NP_BlackBoardDataManager.CurrentEditedNP_BlackBoardDataManager.BBValues)
            {
                npDataSupportorBase.NP_BBValueManager.Add(bbvalues.Key, bbvalues.Value);
            }
        }

        public IEnumerable<Type> GetConfigTypes()
        {
            var q = typeof(ProtoObject).Assembly.GetTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => !x.IsGenericTypeDefinition)
                .Where(x => typeof(ProtoObject).IsAssignableFrom(x));

            return q;
        }
    }
}