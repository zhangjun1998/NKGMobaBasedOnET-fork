using System.Collections.Generic;

namespace ET
{
    public class LockStepStateFrameSyncComponent : Entity
    {
        /// <summary>
        /// 整局游戏的Cmd记录，用于断线重连
        /// </summary>
        public Dictionary<uint, Queue<Object>> WholeCmds = new Dictionary<uint, Queue<Object>>(8192);
        
        public Dictionary<uint, Queue<Object>> FrameCmdsToHandle = new Dictionary<uint, Queue<Object>>(64);

        /// <summary>
        /// 用于帧同步的FixedUpdate，需要注意的是，这个FixedUpdate与框架层的是不搭嘎的
        /// </summary>
        public FixedUpdate FixedUpdate;

        /// <summary>
        /// 开启模拟
        /// </summary>
        public bool StartSync;

        /// <summary>
        /// 当前帧数
        /// </summary>
        public uint CurrentFrame;
        
        /// <summary>
        /// 服务器缓冲帧时长，按帧为单位，这里锁定为1帧，也就是33ms
        /// </summary>
        public uint BufferFrame = 1;

#if !SERVER
        /// <summary>
        /// 玩家输入缓冲区，因为会有回滚操作，需要重新预测到当前帧，保存范围为上一次服务器确认的帧到当前帧
        /// </summary>
        public Dictionary<uint, Queue<Object>> FrameCmdsBuffer = new Dictionary<uint, Queue<Object>>(64);
        
        /// <summary>
        /// 服务端当前帧，用于判断客户端当前超前帧数是否合法，Ping协议和正常的帧同步协议都会有这个信息，直接赋值过来即可
        /// </summary>
        public uint ServerCurrentFrame;
        
        /// <summary>
        /// 暂定客户端最多只能超前服务端5帧
        /// </summary>
        public const uint AheadOfFrameMax = 5;
        
        /// <summary>
        /// 当前是否处于变速阶段
        /// </summary>
        public bool HasInSpeedChangeState;
        
        /// <summary>
        /// 当前客户端超前服务端的帧数
        /// </summary>
        public uint CurrentAheadOfFrame;
        
        /// <summary>
        /// 客户端应当超前服务端的帧数
        /// </summary>
        public uint TargetAheadOfFrame;
#endif
    }
}