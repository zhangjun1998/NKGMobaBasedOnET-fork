using System;
using System.Collections.Generic;

namespace ETModel
{

    public class RoomManagerEntity : Entity
    {
        /// <summary>
        /// 所有房间的简略信息
        /// </summary>
        public Dictionary<long, RoomBriefInfo> AllRoomDic = new Dictionary<long, RoomBriefInfo>();
        /// <summary>
        /// 各个房间场景负载
        /// </summary>
        public Dictionary<int, int> RoomCountWithSceneId = new Dictionary<int, int>();
    }

    /// <summary>
    /// 房间配置信息.
    /// </summary>
    public class RoomConfigComponent : Entity
    {
        public int MaxMemberCount;
        public string MasterName;
        public bool IsInBattle;
    }
    /// <summary>
    /// 房间的实体.
    /// </summary>
    public class RoomEntity : Entity
    {
        public Dictionary<long, RoomPlayerEntity> Players = new Dictionary<long, RoomPlayerEntity>();
    }
    public class RoomPlayerEntity : Entity
    {
        public long GateActorId;
        public string NickName;
        public string Icon;
        public bool IsMaster;
    }
    /// <summary>
    /// 战斗的实体.
    /// </summary>
    public class BattleEntity : Entity
    {
        public Dictionary<long, Unit> BattlePlayers = new Dictionary<long, Unit>();
        public void Awake()
        {
            AddComponent<B2S_WorldColliderManagerComponent>();
            AddComponent<B2S_WorldComponent>();
            AddComponent<B2S_CollisionListenerComponent>();
            AddComponent<CampAllocManagerComponent>();
            AddComponent<BattleEventSystem>();
        }
    }
}
