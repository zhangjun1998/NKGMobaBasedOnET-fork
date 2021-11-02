using System.Collections.Generic;
using System.IO;

namespace ET
{
    public static class BroadcastMsgComponentSystems
    {
        /// 目前和UnitGateComponent的生命周期绑定
        /// </summary>
        /// <param name="instanceId"></param>
        public static void AddProcessId(this BroadcastMsgComponent self, long instanceId)
        {
            InstanceIdStruct instanceIdStruct = new InstanceIdStruct(instanceId);
            if (!self.ProcessIds.ContainsKey(instanceIdStruct.Process))
            {
                self.ProcessIds[instanceIdStruct.Process] = 0;
            }
            self.ProcessIds[instanceIdStruct.Process] += 1;
        }
        public static void ReduceProcessId(this BroadcastMsgComponent self, long instanceId)
        {
            InstanceIdStruct instanceIdStruct = new InstanceIdStruct(instanceId);
            if (!self.ProcessIds.ContainsKey(instanceIdStruct.Process))
            {
                return;
            }
            self.ProcessIds[instanceIdStruct.Process] -= 1;
            if (self.ProcessIds[instanceIdStruct.Process] == 0)
            {
                self.ProcessIds.Remove(instanceIdStruct.Process);
            }
        }
        public static void BroadcastToAll(this BroadcastMsgComponent self, MemoryStream stream)
        {
            var actorid = self.DomainScene().InstanceId;
            foreach (KeyValuePair<int, int> keyValuePair in self.ProcessIds)
            {
                NetInnerComponent.Instance.Get(keyValuePair.Key).Send(actorid, stream);
            }
        }
    }
}