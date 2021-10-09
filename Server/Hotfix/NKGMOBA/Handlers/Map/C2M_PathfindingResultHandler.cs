using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    [ActorMessageHandler]
    public class C2M_PathfindingResultHandler : AMActorLocationHandler<Unit, C2M_PathfindingResult>
    {
        protected override async ETTask Run(Unit unit, C2M_PathfindingResult message)
        {
            Vector3 target = new Vector3(message.X, message.Y, message.Z);
            
            IdleState idleState = ReferencePool.Acquire<IdleState>();
            idleState.SetData(StateTypes.Idle, "Idle", 1);
            unit.NavigateTodoSomething(target, 0, idleState).Coroutine();

            await ETTask.CompletedTask;
        }
    }
}