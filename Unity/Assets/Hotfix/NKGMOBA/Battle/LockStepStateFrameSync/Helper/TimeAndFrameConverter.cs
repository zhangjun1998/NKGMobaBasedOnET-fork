using UnityEngine;

namespace ET
{
    /// <summary>
    /// 时间-帧数转换器
    /// 之所以转换ms到帧的时候向上取整，是因为服务端处理的特殊性
    /// 具体来说，每帧Tick为33ms，服务器跑在25帧，RTT为15ms，所以客户端跑在第 25 + 1（缓冲帧） + 1（RTT转帧数，向上取整） = 27帧，此时客户端发出寻路指令A，本地开始记录27帧的运动数据
    /// 寻路指令A将在一个RTT（15ms）后到达服务器，此时服务器还未到达26帧，但是距离26帧只剩18ms了，不满一个缓存帧时长，所以按照标准来说这个指令A将会在26帧 + 15ms 第27帧执行（虽然未满27帧，但也会放在第27帧执行，因为我们Tick频率是33ms）
    /// </summary>
    public static class TimeAndFrameConverter
    {
        public static long MS_Float2Long(float time)
        {
            return (long) (time * 1000f);
        }

        public static float Float_Long2Float(long time)
        {
            return (time / 1000f);
        }

        public static int Frame_Long2Frame(long time)
        {
            return Mathf.CeilToInt(((time * 1.0f) / GlobalDefine.FixedUpdateTargetDTTime_Long));
        }

        public static int Frame_Float2Frame(float time)
        {
            return Mathf.CeilToInt(time / GlobalDefine.FixedUpdateTargetDTTime_Float);
        }

        public static int Frame_Float2FrameWithHalfRTT(float time, long halfRTT)
        {
            return Frame_Long2Frame(((long) (time * 1000) + halfRTT));
        }
    }
}