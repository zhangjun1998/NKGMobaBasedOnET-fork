using System.Collections.Generic;

namespace ET
{
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
        public Dictionary<long, List<long>> ActoridToSessionIds = new Dictionary<long, List<long>>();
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