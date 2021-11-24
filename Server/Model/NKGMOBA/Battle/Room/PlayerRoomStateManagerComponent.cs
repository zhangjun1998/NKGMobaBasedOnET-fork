using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 玩家关联房间状态的管理组件.(重连时需要
    /// In:RoomManager
    /// Parent:Scene
    /// </summary>
    public class PlayerRoomStateManagerComponent : Entity
    {
        public Dictionary<long, long> PlayeridToRoomPlayer = new Dictionary<long, long>();
    }
}