//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年12月4日 17:06:02
//------------------------------------------------------------

using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    [Title("移动到随机点", TitleAlignment = TitleAlignments.Centered)]
    public class NP_MoveToRandomPosAction: NP_ClassForStoreAction
    {
        [BoxGroup("范围")]
        public int XMin;

        [BoxGroup("范围")]
        public int YMin;

        [BoxGroup("范围")]
        public int XMax;

        [BoxGroup("范围")]
        public int YMax;

        public override Action GetActionToBeDone()
        {
            this.Action = this.MoveToRandomPos;
            return this.Action;
        }

        public void MoveToRandomPos()
        {
#if SERVER 
            Vector3 randomTarget = new Vector3(RandomHelper.RandomNumber(this.XMin, this.XMax), 0, RandomHelper.RandomNumber(this.YMin, this.YMax));
            LSF_MoveCmd lsfPathFindCmd = ReferencePool.Acquire<LSF_MoveCmd>().Init(this.BelongToUnit.Id) as LSF_MoveCmd;
            lsfPathFindCmd.IsMoveStartCmd = true;
            lsfPathFindCmd.PosX = randomTarget.x;
            lsfPathFindCmd.PosY = randomTarget.y;
            lsfPathFindCmd.PosZ = randomTarget.z;
            lsfPathFindCmd.Speed = this.BelongToUnit.GetComponent<NumericComponent>()[NumericType.Speed] / 100f;

            LSF_Component lsfComponent = this.BelongToUnit.BelongToRoom.GetComponent<LSF_Component>();
            
            Vector3 target = new Vector3(lsfPathFindCmd.PosX, lsfPathFindCmd.PosY, lsfPathFindCmd.PosZ);

            IdleState idleState = ReferencePool.Acquire<IdleState>();
            idleState.SetData(StateTypes.Idle, "Idle", 1);
            this.BelongToUnit.NavigateTodoSomething(target, 0, idleState).Coroutine();
            
            lsfComponent.AddCmdToSendQueue(lsfPathFindCmd);
#endif
        }
    }
}