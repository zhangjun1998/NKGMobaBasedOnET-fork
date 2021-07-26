using UnityEngine;

namespace ETModel
{
    public sealed class Unit: Entity
    {
        public Vector3 Position { get; set; }

        public Quaternion Rotation { get; set; }
        //假设RoomEntity 为临时Scene. unit->RoomPlayerComponent->RoomEntity
        public RoomEntity TempScene => Parent.GetParent<RoomEntity>();
        public Unit[] RoomPlayerArray => GetParent<RoomPlayerComponent>().PlayerArray;
        public long SessionId => GetComponent<UnitGateComponent>().GateSessionActorId;
    }
}