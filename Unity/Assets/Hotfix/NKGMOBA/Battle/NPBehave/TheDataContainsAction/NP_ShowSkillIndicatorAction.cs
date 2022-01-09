//此文件格式由工具自动生成
using System;
using Sirenix.OdinInspector;

namespace ET
{
    [Title("显示技能指示器",TitleAlignment = TitleAlignments.Centered)]
    public class NP_ShowSkillIndicatorAction:NP_ClassForStoreAction
    {        
        public override Action GetActionToBeDone()
        {
            this.Action = this.ShowSkillIndicatorAction;
            return this.Action;
        }

        public void ShowSkillIndicatorAction()
        {

        }
    }
}
