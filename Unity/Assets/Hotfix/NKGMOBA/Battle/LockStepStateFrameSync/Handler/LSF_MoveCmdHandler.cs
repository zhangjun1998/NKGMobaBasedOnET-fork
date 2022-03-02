using UnityEngine;

namespace ET
{
    [LSF_MessageHandler(LSF_CmdHandlerType = LSF_MoveCmd.CmdType)]
    public class LSF_MoveCmdHandler : ALockStepStateFrameSyncMessageHandler<LSF_MoveCmd>
    {
        protected override async ETVoid Run(Unit unit, LSF_MoveCmd cmd)
        {
#if !SERVER
            Vector3 pos = new Vector3(cmd.PosX, cmd.PosY, cmd.PosZ);
            Quaternion rotation = new Quaternion(cmd.RotA, cmd.RotB, cmd.RotC, cmd.RotW);
            unit.Position = pos;
            unit.Rotation = rotation;
            Log.Info(
                $"Current : {unit.Position.ToString("#0.0000")} Server : {pos.ToString("#0.0000")} ServerFrame: {unit.BelongToRoom.GetComponent<LSF_Component>().ServerCurrentFrame}");
#endif
            
            if (cmd.IsMoveStartCmd)
            {
                Vector3 target = new Vector3(cmd.TargetPosX, cmd.TargetPosY, cmd.TargetPosZ);
                IdleState idleState = ReferencePool.Acquire<IdleState>();
                idleState.SetData(StateTypes.Idle, "Idle", 1);
                unit.NavigateTodoSomething(target, 0, idleState).Coroutine();
                unit.GetComponent<MoveComponent>().HistroyMoveStates[cmd.Frame] = cmd;
            }
            
            if (cmd.IsStopped)
            {
                MoveComponent moveComponent = unit.GetComponent<MoveComponent>();
                moveComponent.Stop(true);
                Game.EventSystem.Publish(new EventType.MoveStop() {Unit = unit}).Coroutine();
            }

#if SERVER
            // 对于客户端发来的每一条指令，都要进行一次广播，因为多人模式需要进行同步，
            LSF_Component lsfComponent = unit.BelongToRoom.GetComponent<LSF_Component>();
            lsfComponent.AddCmdToSendQueue(cmd);
#endif
            
            await ETTask.CompletedTask;
        }
    }
}