//此文件格式由工具自动生成
using System;
using Sirenix.OdinInspector;

namespace ETModel
{
    [Title("往客户端发送CD信息",TitleAlignment = TitleAlignments.Centered)]
    public class NP_SendCDInfoToClient:NP_ClassForStoreAction
    {
        [LabelText("CD名")]
        public NP_BlackBoardRelationData CDName = new NP_BlackBoardRelationData();
    
        public override Action GetActionToBeDone()
        {
            this.Action = this.SendCDInfoToClient;
            return this.Action;
        }

        public void SendCDInfoToClient()
        {
            
        }
    }
}
