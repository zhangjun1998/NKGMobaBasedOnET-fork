using System;
using System.Collections.Generic;
using System.Linq;

namespace ETModel
{
    /// <summary>
    /// gateSession上挂载的房间信息
    /// </summary>
    public class SessionRoomComponent : Component
    {
        public long RoomActorId;
    }
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
    /// 房间的实体.
    /// </summary>
    public class RoomEntity : Entity
    {
        public void Awake()
        {
            AddComponent<RoomPlayerComponent>();
            AddComponent<RoomConfigComponent>();
        }
        /// <summary>
        /// 玩家进入房间
        /// </summary>
        /// <param name="userInfo"></param>
        public void AddUser(long gateSessionId,UserInfo userInfo)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 房间配置信息.
    /// </summary>
    public class RoomConfigComponent : Component
    {
        public int MaxMemberCount;
        public string MasterName;
        public string RoomName;
        public bool IsInBattle;
    }
    /// <summary>
    /// 管理房间内成员
    /// </summary>
    public class RoomPlayerComponent : Entity
    {
        public Dictionary<long, Unit> Players = new Dictionary<long, Unit>();
        public Unit[] PlayerArray => Players.Values.ToArray();
    }
    /// <summary>
    /// 与房间逻辑有关的成员信息,挂载在unit下
    /// </summary>
    public class RoomPlayerData : Component
    {
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
