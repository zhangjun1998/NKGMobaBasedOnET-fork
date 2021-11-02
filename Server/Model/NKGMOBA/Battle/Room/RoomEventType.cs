namespace ET
{
    namespace RoomEventType
    {
        /// <summary>
        /// 做3件事 1添加战斗组件.2通知全体玩家可以进入游戏 3 清除准备组件
        /// </summary>
        public struct AllPlayerPrepared
        {
            public Scene Scene;
        }
    }
}