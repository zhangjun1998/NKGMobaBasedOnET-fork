using System;

namespace ET
{
    public class RoomAwakeSystem : AwakeSystem<Room, RoomConfigProto>
    {
        public override void Awake(Room self, RoomConfigProto config)
        {
            self.RoomConfig = config;
            self.RoomHolderPlayerId = 0;
        }
    }

    public class RoomDestorySystem : DestroySystem<Room>
    {
        public override void Destroy(Room self)
        {
            self.RoomConfig = null;
            self.RoomHolderPlayerId = 0;
        }
    }
    /// <summary>
    /// 代表一个房间
    /// </summary>
    public class Room : Entity
    {
        /// <summary>
        /// 房主
        /// </summary>
        public Player RoomHolder => Domain.GetComponent<PlayerComponent>().Get(RoomHolderPlayerId);
        public long RoomHolderPlayerId;
        /// <summary>
        /// 房间配置.因为要用于和客户端共享所以直接用proto定义
        /// </summary>
        public RoomConfigProto RoomConfig;
    }
}