//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月21日 15:10:39
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace ET
{
    public class BattleEventDestroySystem: DestroySystem<BattleEventSystem>
    {
        public override void Destroy(BattleEventSystem self)
        {
            self.Destroy();
        }
    }

    /// <summary>
    /// 战斗系统中的事件系统组件，一场战斗挂载一个
    /// </summary>
    public class BattleEventSystem : Entity
    {
        private readonly Dictionary<string, LinkedList<ISkillSystemEvent>> m_AllEvents =
            new Dictionary<string, LinkedList<ISkillSystemEvent>>();

        /// <summary>
        /// 缓存的结点字典
        /// </summary>
        private readonly Dictionary<string, LinkedListNode<ISkillSystemEvent>> m_CachedNodes =
            new Dictionary<string, LinkedListNode<ISkillSystemEvent>>();

        /// <summary>
        /// 临时结点字典
        /// </summary>
        private readonly Dictionary<string, LinkedListNode<ISkillSystemEvent>> m_TempNodes =
            new Dictionary<string, LinkedListNode<ISkillSystemEvent>>();

        public void RegisterEvent(string eventId, ISkillSystemEvent e)
        {
            if (!this.m_AllEvents.ContainsKey(eventId))
            {
                this.m_AllEvents.Add(eventId, new LinkedList<ISkillSystemEvent>());
            }

            this.m_AllEvents[eventId].AddLast(e);
        }

        public void UnRegisterEvent(string eventId, ISkillSystemEvent e)
        {
            if (m_CachedNodes.Count > 0)
            {
                foreach (KeyValuePair<string, LinkedListNode<ISkillSystemEvent>> cachedNode in m_CachedNodes)
                {
                    //预防极端情况，比如两个不同的事件id订阅了同一个事件处理者
                    if (cachedNode.Value != null && cachedNode.Key == eventId && cachedNode.Value.Value == e)
                    {
                        //注意这里添加的Handler是下一个
                        m_TempNodes.Add(cachedNode.Key, cachedNode.Value.Next);
                    }
                }

                //把临时结点字典中的目标元素值更新到缓存结点字典
                if (m_TempNodes.Count > 0)
                {
                    foreach (KeyValuePair<string, LinkedListNode<ISkillSystemEvent>> cachedNode in m_TempNodes)
                    {
                        m_CachedNodes[cachedNode.Key] = cachedNode.Value;
                    }

                    //清除临时结点
                    m_TempNodes.Clear();
                }
            }

            if (this.m_AllEvents.ContainsKey(eventId))
            {
                this.m_AllEvents[eventId].Remove(e);
                ReferencePool.Release(e);
            }
        }

        public void Run(string type)
        {
            LinkedList<ISkillSystemEvent> iEvents;
            if (!this.m_AllEvents.TryGetValue(type, out iEvents))
            {
                return;
            }

            LinkedListNode<ISkillSystemEvent> temp = iEvents.First;

            while (temp != null)
            {
                try
                {
                    this.m_CachedNodes[type] = temp.Next;
                    temp.Value?.Handle();
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }

                temp = this.m_CachedNodes[type];
            }

            this.m_CachedNodes.Remove(type);
        }

        public void Run<A>(string type, A a)
        {
            LinkedList<ISkillSystemEvent> iEvents;
            if (!this.m_AllEvents.TryGetValue(type, out iEvents))
            {
                return;
            }

            LinkedListNode<ISkillSystemEvent> temp = iEvents.First;

            while (temp != null)
            {
                try
                {
                    this.m_CachedNodes[type] = temp.Next;
                    temp.Value?.Handle(a);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }

                temp = this.m_CachedNodes[type];
            }

            this.m_CachedNodes.Remove(type);
        }

        public void Run<A, B>(string type, A a, B b)
        {
            LinkedList<ISkillSystemEvent> iEvents;
            if (!this.m_AllEvents.TryGetValue(type, out iEvents))
            {
                return;
            }

            LinkedListNode<ISkillSystemEvent> temp = iEvents.First;

            while (temp != null)
            {
                try
                {
                    this.m_CachedNodes[type] = temp.Next;
                    temp.Value?.Handle(a, b);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }

                temp = this.m_CachedNodes[type];
            }

            this.m_CachedNodes.Remove(type);
        }

        public void Run<A, B, C>(string type, A a, B b, C c)
        {
            LinkedList<ISkillSystemEvent> iEvents;
            if (!this.m_AllEvents.TryGetValue(type, out iEvents))
            {
                return;
            }

            LinkedListNode<ISkillSystemEvent> temp = iEvents.First;

            while (temp != null)
            {
                try
                {
                    this.m_CachedNodes[type] = temp.Next;
                    temp.Value?.Handle(a, b, c);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }

                temp = this.m_CachedNodes[type];
            }

            this.m_CachedNodes.Remove(type);
        }

        public void Destroy()
        {
            this.m_AllEvents.Clear();
            this.m_CachedNodes.Clear();
            this.m_TempNodes.Clear();
        }
    }
}