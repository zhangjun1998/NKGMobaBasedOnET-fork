//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月27日 13:13:10
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using MongoDB.Bson.Serialization;
#if !SERVER
using UnityEngine;

#endif

namespace ETModel
{
    [ObjectSystem]
    public class UnitAttributesDataRepositoryComponentAwakeSystem: AwakeSystem<UnitAttributesDataRepositoryComponent>
    {
        public override void Awake(UnitAttributesDataRepositoryComponent self)
        {
            self.Awake();
        }
    }

    public class UnitAttributesDataRepositoryComponent: Component
    {
        public Dictionary<long, UnitAttributesDataSupportor> AllUnitAttributesBaseDataDic = new Dictionary<long, UnitAttributesDataSupportor>();

        public void Awake()
        {
            ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
            GameObject heroDataConfigs = resourcesComponent.LoadAsset<GameObject>(ABPathUtilities.GetNormalConfigPath("UnitAttributesDataConfigs"));
            foreach (var referenceCollectorData in heroDataConfigs.GetComponent<ReferenceCollector>().data)
            {
                TextAsset textAsset = heroDataConfigs.GetTargetObjectFromRC<TextAsset>(referenceCollectorData.key);

                if (textAsset.bytes.Length == 0) Log.Info("没有读取到文件");

                UnitAttributesDataSupportor unitAttributesDataSupportor = BsonSerializer.Deserialize<UnitAttributesDataSupportor>(textAsset.bytes);
                this.AllUnitAttributesBaseDataDic[unitAttributesDataSupportor.SupportId] = unitAttributesDataSupportor;
            }
        }

        /// <summary>
        /// 根据id来获取指定Unit属性数据(通过深拷贝的形式获得，对数据的更改不会影响到原本的数据)
        /// </summary>
        /// <param name="dataSupportId">数据载体Id</param>
        /// <param name="nodeDataId">数据载体中的节点Id</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetUnitAttributesDataById_DeepCopy<T>(long dataSupportId, long nodeDataId) where T : UnitAttributesNodeDataBase
        {
            if (this.AllUnitAttributesBaseDataDic.TryGetValue(dataSupportId, out var unitAttributesDataSupportor))
            {
                if (unitAttributesDataSupportor.UnitAttributesDataSupportorDic.TryGetValue(nodeDataId, out var unitAttributesNodeDataBase))
                {
                    return unitAttributesNodeDataBase.DeepCopy() as T;
                }
            }
            Debug.LogError($"查询Unit属性数据失败，数据载体Id为{dataSupportId}，数据载体中的节点Id为{nodeDataId}");
            return null;
        }
        
    }
}