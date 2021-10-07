//此文件格式由工具自动生成

using System;
using Sirenix.OdinInspector;

namespace ET
{
    [Title("同步黑板bool值到客户端", TitleAlignment = TitleAlignments.Centered)]
    public class NP_SyncNPBBValue_BoolAction : NP_ClassForStoreAction
    {
        [LabelText("黑板键")] public string BBKey;

        [LabelText("黑板值")] public bool BBValue;

        public override Action GetActionToBeDone()
        {
            this.Action = this.SyncNPBBValue_BoolAction;
            return this.Action;
        }

        public void SyncNPBBValue_BoolAction()
        {
#if SERVER
              MessageHelper.BroadcastToRoom(this.BelongToUnit.BelongToRoom,
                new M2C_SyncNPBehaveBoolData() {UnitId = BelongToUnit.Id, BBKey = BBKey, Value = BBValue});
#endif
        }
    }
}