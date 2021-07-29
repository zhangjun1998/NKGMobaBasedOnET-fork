using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class RoomPlayerComponentDestroySystem : DestroySystem<RoomPlayerComponent>
    {
        public override void Destroy(RoomPlayerComponent self)
        {
            foreach (var unitid in self.Players.Keys)
            {
                self.RemoveUnit(unitid).Coroutine();
            }
            self.Players = null;
        }
    }
    [ObjectSystem]
    public class RoomPlayerComponentAwakeSystem : AwakeSystem<RoomPlayerComponent>
    {
        public override void Awake(RoomPlayerComponent self)
        {
            self.Players = new System.Collections.Generic.Dictionary<long, Unit>();
        }
    }
}
