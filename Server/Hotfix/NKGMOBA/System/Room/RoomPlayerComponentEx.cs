using ETModel;

namespace ETHotfix
{
    public static class RoomPlayerComponentEx
    {
        public static async ETTask AddUnit(this RoomPlayerComponent self,Unit unit)
        {
            self.Players.Add(unit.Id,unit);
            UnitComponent.Instance.Add(unit);
            await unit.GetComponent<MailBoxComponent>().AddLocation();
        }
        public static async ETTask RemoveUnit(this RoomPlayerComponent self, long unitid)
        {
            if (self.Players.TryGetValue(unitid, out var unit))
            {
                self.Players.Remove(unit.Id);
                await unit.GetComponent<MailBoxComponent>().RemoveLocation();
                UnitComponent.Instance.Remove(unitid);
            }
            else
            {
                Log.Error($"unitid :{unitid} not found in RoomPlayerComponent");
            }
        }
    }
}
