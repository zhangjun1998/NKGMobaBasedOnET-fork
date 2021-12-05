using UnityEngine;

namespace ET
{
    [LSF_MessageHandler(LSF_CmdHandlerType = LSF_PathFindCmd.CmdType)]
    public class LSF_PathFindCmdHandler : ALockStepStateFrameSyncMessageHandler<LSF_PathFindCmd>
    {
        protected override async ETVoid Run(Unit unit, LSF_PathFindCmd cmd)
        {
            Vector3 target = new Vector3(cmd.PosX, cmd.PosY, cmd.PosZ);

            IdleState idleState = ReferencePool.Acquire<IdleState>();
            idleState.SetData(StateTypes.Idle, "Idle", 1);
            unit.NavigateTodoSomething(target, 0, idleState).Coroutine();

#if SERVER
            // 对于客户端发来的每一条指令，都要进行一次广播，因为多人模式需要进行同步，
            LSF_Component lsfComponent = unit.BelongToRoom.GetComponent<LSF_Component>();
            lsfComponent.AddCmdToSendQueue(cmd);
#endif
            
            await ETTask.CompletedTask;
        }
    }
}