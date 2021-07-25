﻿namespace ETModel
{
    public static class EventIdType
    {
        public const string NumbericChange = "NumbericChange";
        public const string TestBehavior = "TestBehavior";

        public const string CreateCollider = "CreateCollider";

        public const string SendBuffInfoToClient = "SendBuffInfoToClient";
        public const string SendNPBBValue_BoolToClient = "SendNPBBValue_BoolToClient";

        public const string SendCDInfoToClient = "SendCDInfoToClient";
        //移动到随机位置
        public const string MoveToRandomPos = "MoveToRandomPos";

        //移除碰撞体
        public const string RemoveCollider = "RemoveCollider";

        /// <summary>
        /// 需要监听伤害的buff（比如吸血buff）需要监听此事件
        /// </summary>
        public const string ExcuteDamage = "ExcuteDamage";

        //治疗触发
        public const string ExcuteTreate = "ExcuteTreate";

        /// <summary>
        /// 需要监听受伤的Buff（例如反甲）需要监听此事件
        /// </summary>
        public const string TakeDamage = "TakeDamage";

        //治疗承受
        public const string TakeTreate = "TakeTreate";

        //Numeric执行修改
        public const string NumericApplyChangeValue = "NumericApplyChangeValue";

        /// <summary>
        /// 取消移动
        /// </summary>
        public const string CancelMove = "CancelMove";

        /// <summary>
        /// 取消攻击
        /// </summary>
        public const string CancelAttack = "CancelAttack";

        /// <summary>
        /// 取消攻击但不重置攻击对象
        /// </summary>
        public const string CancelAttackWithOutResetAttackTarget = "CancelAttackWithOutResetAttackTarget";
        /// <summary>
        /// 广播消息
        /// </summary>
        public const string BroadcastMsg = "BroadcastMsg";

    }
}