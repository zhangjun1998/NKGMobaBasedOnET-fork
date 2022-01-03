//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月27日 22:26:39
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    [Title("创建一个碰撞体", TitleAlignment = TitleAlignments.Centered)]
    public class NP_CreateColliderAction : NP_ClassForStoreAction
    {
        [LabelText("碰撞关系配置Id")] [Tooltip("Excel配置表中Id")]
        public int CollisionsRelationSupportIdInExcel;

        [LabelText("行为树配置表Id")] [Tooltip("Excel配置表中Id")]
        public int ColliderNPBehaveTreeIdInExcel;

        public override Action GetActionToBeDone()
        {
            this.Action = this.CreateColliderData;
            return this.Action;
        }

        public void CreateColliderData()
        {
#if SERVER
            Log.Info("创建碰撞");
            int colliderDataConfigId = Server_B2SCollisionRelationConfigCategory.Instance
                .Get(CollisionsRelationSupportIdInExcel)
                .B2S_ColliderConfigId;

            Unit colliderUnit = UnitFactory
                .CreateSpecialColliderUnit(BelongToUnit.BelongToRoom, BelongToUnit, colliderDataConfigId,
                    CollisionsRelationSupportIdInExcel);

            //根据传过来的行为树Id来给这个碰撞Unit加上行为树
            NP_RuntimeTreeFactory.CreateSkillNpRuntimeTree(colliderUnit,
                    SkillCanvasConfigCategory.Instance.Get(ColliderNPBehaveTreeIdInExcel).NPBehaveId,
                    SkillCanvasConfigCategory.Instance.Get(ColliderNPBehaveTreeIdInExcel).BelongToSkillId)
                .Start();
#endif
        }
    }
}