using UnityEngine;

namespace ET
{
    [LSF_MessageHandler(LSF_CmdHandlerType = LSF_MoveCmd.CmdType)]
    public class LSF_MoveCmdHandler : ALockStepStateFrameSyncMessageHandler<LSF_MoveCmd>
    {
        protected override async ETVoid Run(Unit unit, LSF_MoveCmd cmd)
        {
            Vector3 target = new Vector3(cmd.PosX, cmd.PosY, cmd.PosZ);

            if (cmd.IsMoveStartCmd)
            {
                IdleState idleState = ReferencePool.Acquire<IdleState>();
                idleState.SetData(StateTypes.Idle, "Idle", 1);
                unit.NavigateTodoSomething(target, 0, idleState).Coroutine();
            }
            else
            {
                Quaternion rotation = new Quaternion(cmd.RotA, cmd.RotB, cmd.RotC, cmd.RotW);
                unit.Position = target;
                unit.Rotation = rotation;
            }

#if !SERVER
            Log.Info($"Current : {unit.Position.ToString("#0.0000")} Server : {target.ToString("#0.0000")} ServerFrame: {unit.BelongToRoom.GetComponent<LSF_Component>().ServerCurrentFrame}");
#endif
            
            if (cmd.IsStopped)
            {
                MoveComponent moveComponent = unit.GetComponent<MoveComponent>();
                moveComponent.Stop(true);
                Game.EventSystem.Publish(new EventType.MoveStop() {Unit = unit}).Coroutine();
            }

            await ETTask.CompletedTask;
        }
    }
}