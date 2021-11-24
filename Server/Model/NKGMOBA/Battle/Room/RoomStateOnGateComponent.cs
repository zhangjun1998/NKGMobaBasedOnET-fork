namespace ET
{
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
        /// <summary>
        /// 销毁时是否需要通知Room.如果是room通知过来的.需要置为false再释放
        /// </summary>
        public bool NeedSendMessage;
        public void Awake()
        { 
        
        }
    }
}