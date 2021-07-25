using System;
using System.Collections.Generic;
using System.Linq;

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
    /// 房间的实体.
    /// </summary>
    public class RoomEntity : Entity
    {
        /// <summary>
        /// 只有在非战斗状态的房间才可以修改玩家状态
        /// </summary>
        public bool CanUnitChangeState => 
            GetComponent<BattleLoadingComponent>() != null &&
            GetComponent<BattleEntity>()!=null;
        public void Awake()
        {
            AddComponent<RoomPlayerComponent>();
            AddComponent<RoomConfigComponent>();
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
        //public Dictionary<long, Unit> BattlePlayers =>GetParent<RoomEntity>().GetComponent<RoomPlayerComponent>().Players;
        public void Awake()
        {
            AddComponent<B2S_WorldColliderManagerComponent>();
            AddComponent<B2S_WorldComponent>();
            AddComponent<B2S_CollisionListenerComponent>();
            AddComponent<CampAllocManagerComponent>();
            //改造成本较高.BattleEventSystem先不移到单独战斗里
            //AddComponent<BattleEventSystem>();
        }
    }
}
