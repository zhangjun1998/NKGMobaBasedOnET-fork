using System.Collections.Generic;

namespace ET
{
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
}