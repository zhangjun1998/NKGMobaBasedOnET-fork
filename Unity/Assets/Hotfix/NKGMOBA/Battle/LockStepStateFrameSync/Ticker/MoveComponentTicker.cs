using UnityEngine;

namespace ET
{
    [LSF_Tickable(EntityType = typeof(MoveComponent))]
    public class MoveComponentTicker : ALSF_TickHandler<MoveComponent>
    {
        public override void OnLSF_Tick(MoveComponent entity, long deltaTime)
        {
            Unit unit = entity.GetParent<Unit>();
#if SERVER
            LSF_MoveCmd lsfMoveCmd = ReferencePool.Acquire<LSF_MoveCmd>().Init(unit.Id) as LSF_MoveCmd;

            lsfMoveCmd.Speed = entity.Speed;
            lsfMoveCmd.PosX = unit.Position.x;
            lsfMoveCmd.PosY = unit.Position.y;
            lsfMoveCmd.PosZ = unit.Position.z;

            lsfMoveCmd.RotA = unit.Rotation.x;
            lsfMoveCmd.RotB = unit.Rotation.y;
            lsfMoveCmd.RotC = unit.Rotation.z;
            lsfMoveCmd.RotW = unit.Rotation.w;

            lsfMoveCmd.IsStopped = !entity.ShouldMove;

            unit.BelongToRoom.GetComponent<LSF_Component>().SendMessage(lsfMoveCmd);

            // if (entity.ShouldMove)
            // {
            //     entity.MoveForward(false);
            // }
            
                        
            Log.Info($"Frame: {unit.BelongToRoom.GetComponent<LSF_Component>().CurrentFrame} {entity.To.ToString()}");
#else
            Log.Info($"Frame: {unit.BelongToRoom.GetComponent<LSF_Component>().CurrentFrame} {entity.To.ToString()}");
#endif
        }
    }
}