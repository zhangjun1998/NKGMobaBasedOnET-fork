using System;
using System.Collections.Generic;

namespace ETModel
{
    public enum RoomPlayerQuitTypeEnum
    {
        /// <summary>
        /// 自己退
        /// </summary>
        SelfQuit = 0,
        /// <summary>
        /// 被踢
        /// </summary>
        BeKicked = 1,
        /// <summary>
        /// 房间解散
        /// </summary>
        RoomDismiss = 2
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
        public void Awake()
        {
        }
    }
}
