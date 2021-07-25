using ETModel;

namespace ETHotfix
{

    [ObjectSystem]
    public class BattleLoadingComponentAwakeSystem : AwakeSystem<BattleLoadingComponent>
    {
        public override void Awake(BattleLoadingComponent self)
        {
            var units = self.GetParent<RoomEntity>().GetComponent<RoomPlayerComponent>().PlayerArray;
            self.Awake(units.Length);
            var msg=new RM2C_EnterBattleMessage();
            foreach (var unit in units)
            {
                msg.Units.Add(unit.UnitToUnitInfo());
            }
            MessageHelper.Broadcast(units, msg);
            //超时直接进入战斗
            //self.timerId=TimerComponent.Instance.OnceTimer
        }
    }
    public static class BattleLoadingComponentEx 
    {
        public static void Ready(this BattleLoadingComponent self,long uid)
        {
            if (!self.LoadCompletedIds.Contains(uid))
            {
                self.LoadCompletedIds.Add(uid);
                if (self.LoadCompletedIds.Count >= self.NeedNum)
                {
                    var units = self.GetParent<RoomEntity>().GetComponent<RoomPlayerComponent>().PlayerArray;
                    var msg = new RM2C_StartBattleMessage();
                    MessageHelper.Broadcast(units, msg);
                }
            }
        }
    }
}
