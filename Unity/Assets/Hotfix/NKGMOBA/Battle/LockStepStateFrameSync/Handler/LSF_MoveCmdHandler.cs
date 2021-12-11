using UnityEngine;

namespace ET
{
    [LSF_MessageHandler(LSF_CmdHandlerType = LSF_MoveCmd.CmdType)]
    public class LSF_MoveCmdHandler : ALockStepStateFrameSyncMessageHandler<LSF_MoveCmd>
    {
        protected override async ETVoid Run(Unit unit, LSF_MoveCmd cmd)
        {
            Vector3 pos = new Vector3(cmd.PosX, cmd.PosY, cmd.PosZ);
#if !SERVER
            Log.Info($"Current : {unit.Position.ToString("#0.0000")} Server : {pos.ToString("#0.0000")} ServerFrame: {unit.BelongToRoom.GetComponent<LSF_Component>().ServerCurrentFrame}");
#endif

            Quaternion rotation = new Quaternion(cmd.RotA, cmd.RotB, cmd.RotC, cmd.RotW);
            unit.Position = pos;

            unit.Rotation = rotation;

            if (cmd.IsStopped)
            {
                MoveComponent moveComponent = unit.GetComponent<MoveComponent>();
                moveComponent.Stop();
                Game.EventSystem.Publish(new EventType.MoveStop() {Unit = unit}).Coroutine();
            }

            await ETTask.CompletedTask;
        }
    }
}