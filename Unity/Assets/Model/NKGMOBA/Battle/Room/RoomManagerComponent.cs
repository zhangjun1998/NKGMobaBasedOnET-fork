using System.Collections.Generic;

namespace ET
{
    public class RoomManagerComponentAwakeSystem : AwakeSystem<RoomManagerComponent>
    {
        public override void Awake(RoomManagerComponent self)
        {
        }
    }

    public class RoomManagerComponentDestroySystem : DestroySystem<RoomManagerComponent>
    {
        public override void Destroy(RoomManagerComponent self)
        {
            self.Rooms.Clear();
        }
    }

    public class RoomManagerComponent : Entity
    {
        public Dictionary<long, Room> Rooms = new Dictionary<long, Room>();

        public Room CreateRoom(long id)
        {
            Room room = Entity.CreateWithId<Room>(this, id);
            
            room.AddComponent<UnitComponent>();
            
            Rooms.Add(room.Id, room);
            return room;
        }

        public Room GetRoom(long id)
        {
            if (Rooms.TryGetValue(id, out var room))
            {
                return room;
            }
            else
            {
                Log.Warning($"请求的Room Id不存在 ： {id}");
                return null;
            }
        }
        
        /// <summary>
        /// 根据PlayerId获取其作为房主的房间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Room GetRoomByPlayerId(long playerId)
        {
            foreach (var room in Rooms)
            {
                if (room.Value.RoomHolderPlayerId == playerId)
                {
                    return room.Value;
                }
            }

            Log.Error($"playerId作为房主的房间不存在 ： {playerId}");
            return null;
        }

        public void RemoveRoom(long id)
        {
            if (Rooms.TryGetValue(id, out var room))
            {
                room.Dispose();
                Rooms.Remove(id);
            }
        }

        public void RemoveAllRooms()
        {
            foreach (var room in Rooms)
            {
                room.Value.Dispose();
            }
            Rooms.Clear();
        }
    }
}