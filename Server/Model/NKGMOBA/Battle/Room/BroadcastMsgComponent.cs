using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 记录要发送的进程id,需要特殊广播的场景下挂载
    /// 目前挂载:RoomScene
    /// </summary>
    public class BroadcastMsgComponent : Entity
    {
        public Dictionary<int, int> ProcessIds = new Dictionary<int, int>();
    }
}