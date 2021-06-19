//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月4日 16:17:29
//------------------------------------------------------------

using System.Collections.Generic;
using Box2DSharp.Dynamics;

namespace ETModel
{
    /// <summary>
    /// 一个碰撞体Component,包含一个碰撞实例所有信息，直接挂载到碰撞体Unit上
    /// 比如诺手Q技能碰撞体UnitA，那么这个B2S_ColliderComponent的Entity就是UnitA，而其中的BelongToUnit就是诺手
    /// </summary>
    public class B2S_ColliderComponent: Component
    {
        /// <summary>
        /// 碰撞体数据表中的Id (Excel中的Id)
        /// </summary>
        public int B2S_ColliderDataConfigId;

        /// <summary>
        /// Box2D世界中的刚体
        /// </summary>
        public Body Body;

        /// <summary>
        /// 所归属的Unit，也就是产出碰撞体的Unit，
        /// 比如诺克放一个Q，那么BelongUnit就是诺克
        /// </summary>
        public Unit BelongToUnit;

        /// <summary>
        /// 是否同步归属的Unit
        /// </summary>
        public bool Sync;

        /// <summary>
        /// 碰撞体数据实例
        /// </summary>
        public B2S_ColliderDataStructureBase B2S_ColliderDataStructureBase = new B2S_ColliderDataStructureBase();

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
            Game.Scene.GetComponent<B2S_WorldComponent>().GetWorld().DestroyBody(this.Body);
            this.B2S_ColliderDataStructureBase = null;
        }
    }
}