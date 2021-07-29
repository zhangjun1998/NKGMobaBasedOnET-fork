//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月6日 13:32:23
//------------------------------------------------------------

using ETModel;
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class HeartBeatSystem: UpdateSystem<HeartBeatComponent>
    {
        public override void Update(HeartBeatComponent self)
        {
            self.Update();
        }
    }

    /// <summary>
    /// Session心跳组件(需要挂载到Session上)
    /// </summary>
    public class HeartBeatComponent: Component
    {
        /// <summary>
        /// 心跳包间隔
        /// </summary>
        public int SendInterval = 10;

        /// <summary>
        /// 记录时间
        /// </summary>
        private long RecordDeltaTime = 0;

        /// <summary>
        /// 判断是否已经离线
        /// </summary>
        private bool hasOffline;

        public void Update()
        {
            if (this.hasOffline) 
                return;
            // 如果还没有建立Session直接返回、或者没有到达发包时间
            if (TimeHelper.ClientNowSeconds() - this.RecordDeltaTime < this.SendInterval) 
                return;
            // 记录当前时间
            this.RecordDeltaTime = TimeHelper.ClientNowSeconds();
            Log.Info("发送心跳包");
            // 开始发包
            try
            {
                this.GetParent<Session>().Call(new C2G_HeartBeat()).Coroutine();
            }
            catch
            {
                Log.Info("发送心跳包失败");
                if (this.hasOffline) return;
                this.hasOffline = true;
                Game.EventSystem.Run(EventIdType.ShowOfflineDialogUI_Model, 1, "提示", "很抱歉，你与服务器连接已断开");
            }
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
			
            base.Dispose();
            SendInterval = 10;
            RecordDeltaTime = 0;
            hasOffline = false;
            Game.EventSystem.Run(EventIdType.ShowOfflineDialogUI_Model, 1, "提示", "很抱歉，你与服务器连接已断开");
        }
    }
}