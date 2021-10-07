namespace ET
{
    public static class RoomManagerComponentSystems
    {
        public static Room CreateBattleRoom(this RoomManagerComponent self, long id)
        {
            Room room = Entity.CreateWithId<Room>(self, id, true);

            room.AddComponent<UnitComponent>();
            room.AddComponent<RecastPathComponent>();
            room.AddComponent<CDComponent>();

            room.AddComponent<BattleEventSystem>();
            room.AddComponent<B2S_WorldComponent>();
            room.AddComponent<B2S_WorldColliderManagerComponent>();
            room.AddComponent<B2S_CollisionListenerComponent>();

            self.Rooms.Add(room.Id, room);
            return room;
        }

        public static Room CreateLobbyRoom(this RoomManagerComponent self, long id, int startGameNum)
        {
            Room room = Entity.CreateWithId<Room>(self, id, true);
            room.startGameNum = startGameNum;
            room.enterNum = 0;
            room.ContainsPlayers.Clear();
            room.PlayersCamp.Clear();

            self.Rooms.Add(room.Id, room);
            return room;
        }
    }
}