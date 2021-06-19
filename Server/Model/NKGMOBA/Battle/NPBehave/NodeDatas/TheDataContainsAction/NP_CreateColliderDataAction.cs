//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月27日 22:26:39
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;

namespace ETModel
{
    [Title("创建一个碰撞体", TitleAlignment = TitleAlignments.Centered)]
    public class NP_CreateColliderAction: NP_ClassForStoreAction
    {
        [LabelText("碰撞关系配置Id")]
        [Tooltip("Excel配置表中Id")]
        public int CollisionsRelationSupportIdInExcel;

        [LabelText("行为树配置表Id")]
        [Tooltip("Excel配置表中Id")]
        public int ColliderNPBehaveTreeIdInExcel;

        public override Action GetActionToBeDone()
        {
            this.Action = this.CreateColliderData;
            return this.Action;
        }

        public void CreateColliderData()
        {
            Game.EventSystem.Run(EventIdType.CreateCollider, this.Unitid, this.CollisionsRelationSupportIdInExcel,
                this.ColliderNPBehaveTreeIdInExcel);
        }
    }
}