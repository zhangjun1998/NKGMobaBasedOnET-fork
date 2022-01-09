//此文件格式由工具自动生成
using System;
using Sirenix.OdinInspector;

namespace ET
{
    [Title("隐藏技能指示器",TitleAlignment = TitleAlignments.Centered)]
    public class NP_HideSkillIndicatorAction:NP_ClassForStoreAction
    {        
        public override Action GetActionToBeDone()
        {
            this.Action = this.HideSkillIndicatorAction;
            return this.Action;
        }

        public void HideSkillIndicatorAction()
        {

        }
    }
}
