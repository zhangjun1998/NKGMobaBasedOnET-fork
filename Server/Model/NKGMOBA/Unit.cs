using UnityEngine;

namespace ETModel
{
    public sealed class Unit: Entity
    {
        public Vector3 Position { get; set; }

        public Quaternion Rotation { get; set; }
        //假设RoomEntity 为Scene. unit->RoomPlayerComponent->RoomEntity
        public Entity TempScene => Parent.GetParent<Entity>();
        public Unit[] RoomPlayerArray => GetParent<RoomPlayerComponent>().PlayerArray;
    }
}