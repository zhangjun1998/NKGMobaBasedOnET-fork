using System;
using System.Collections.Generic;

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
            //self.enterNum = 0;
            //self.RoomHolder = null;
            //self.startGameNum = 0;
            //self.ContainsPlayers.Clear();
            //self.PlayersCamp.Clear();
            self.RoomConfig = null;
            self.RoomHolderPlayerId = 0;
        }
    }
    /// <summary>
    /// 战斗组件.记录一些战斗需要的数据
    /// </summary>
    public class BattleComponent : Entity
    {
        public long BattleStartTime;
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
        ///// <summary>
        ///// 这个房间当前包含的玩家，包括房主
        ///// </summary>
        //public Dictionary<long, Player> ContainsPlayers = new Dictionary<long, Player>();

        /// <summary>
        /// 房间玩家对应的位置，也就是阵营
        /// </summary>
        //public Dictionary<int, long> PlayersCamp = new Dictionary<int, long>();
    }
    /// <summary>
    /// 玩家关联房间状态的管理组件.
    /// In:RoomManager
    /// Parent:Scene
    /// </summary>
    public class PlayerRoomStateManagerComponent : Entity
    {
        public Dictionary<long, long> PlayeridToRoomPlayer = new Dictionary<long, long>();
    }
    public class RoomStateOnGateComponentAwakeSystem : AwakeSystem<RoomStateOnGateComponent, long,long>
    {
        public override void Awake(RoomStateOnGateComponent self, long actorid,long roomsceneid)
        {
            self.RoomPlayerActorId = actorid;
            self.TargetRoomSceneId = roomsceneid;
            BroadcastMsgOnGateComponent.Instance.AddSessionId(self.TargetRoomSceneId, self.Parent.InstanceId);
        }
    }
    public class RoomStateOnGateComponentDestroySystem : DestroySystem<RoomStateOnGateComponent>
    {
        public override void Destroy(RoomStateOnGateComponent self)
        {
            BroadcastMsgOnGateComponent.Instance.RemoveSessionId(self.TargetRoomSceneId, self.Parent.InstanceId);
            self.RoomPlayerActorId = 0;
            self.TargetRoomSceneId = 0;
        }
    }
    /// <summary>
    /// gate上保留玩家在房间内的状态组件.
    /// In:Gate
    /// Parent:Session
    /// </summary>
    public class RoomStateOnGateComponent : Entity
    {
        /// <summary>
        /// 在roomscene上 RoomPlayer的Instanceid
        /// </summary>
        public long RoomPlayerActorId;
        /// <summary>
        /// roomsceneid.用于注册或者注销消息监听
        /// </summary>
        public long TargetRoomSceneId;
    }

    /// <summary>
    /// 管理准备进战斗的状态.全部准备或者超时进入
    /// In:Room
    /// Parent:Scene
    /// </summary>
    public class RoomPreparedToEnterBattleComponent : Entity
    {
        public void Awake()
        {

        }



        /// <summary>
        /// 需要准备的人数
        /// </summary>
        public int TargetPreparNum;

        public long TimerId;
        /// <summary>
        /// 当前准备的人数
        /// </summary>
        public int CurrPreparNum => PreparUnitId.Count;
        /// <summary>
        /// 是否所有人准备就绪
        /// </summary>
        public bool IsAllPrepar => CurrPreparNum >= TargetPreparNum;
        /// <summary>
        /// 已经准备的玩家
        /// </summary>
        public HashSet<long> PreparUnitId = new HashSet<long>();

    }
    /// <summary>
    /// 记录要发送的进程id,需要特殊广播的场景下挂载
    /// 目前挂载:RoomScene
    /// </summary>
    public class BroadcastMsgComponent : Entity
    {
        public Dictionary<int, int> ProcessIds = new Dictionary<int, int>();
    }
    /// <summary>
    /// 在Gate记录actorid对应需要广播的sessionid
    /// </summary>
    public class BroadcastMsgOnGateComponent : Entity
    {
        public static BroadcastMsgOnGateComponent Instance
        {
            get;
            set;
        }
        public Dictionary<long, List<long>> ActoridToSessionIds;
        public void AddSessionId(long actorid, long sessionid)
        {
            if (!ActoridToSessionIds.ContainsKey(actorid))
            {
                ActoridToSessionIds[actorid] = new List<long>();
            }
            ActoridToSessionIds[actorid].Add(sessionid);
        }
        public void RemoveSessionId(long actorid, long sessionid)
        {
            if (ActoridToSessionIds.TryGetValue(actorid, out var sessionList))
            {
                sessionList.Remove(sessionid);
                //全部清空时清理一下
                if (sessionList.Count == 0)
                {
                    ActoridToSessionIds.Remove(actorid);
                }
            }
        }
    }
}