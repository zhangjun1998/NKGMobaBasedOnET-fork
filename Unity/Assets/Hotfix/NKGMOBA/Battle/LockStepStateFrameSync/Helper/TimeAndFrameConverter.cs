using UnityEngine;

namespace ET
{
    /// <summary>
    /// 时间-帧数转换器
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
            // 从游戏设计的角度来看，就算理应超前的帧数不满一帧也要按照一帧去算，因为玩家的指令肯定是越早到达服务器越好
            return Mathf.CeilToInt(((time * 1.0f) / GlobalDefine.FixedUpdateTargetDTTime_Long));
        }

        public static int Frame_Float2Frame(float time)
        {
            // 从游戏设计的角度来看，就算理应超前的帧数不满一帧也要按照一帧去算，因为玩家的指令肯定是越早到达服务器越好
            return Mathf.CeilToInt(time / GlobalDefine.FixedUpdateTargetDTTime_Float);
        }

        public static int Frame_Float2FrameWithHalfRTT(float time, long halfRTT)
        {
            return Frame_Long2Frame(((long) (time * 1000) + halfRTT));
        }
    }
}