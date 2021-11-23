namespace ET
{
    public class RoomPreparedToEnterBattleComponentAwake : AwakeSystem<RoomPreparedToEnterBattleComponent>
    {
        public override void Awake(RoomPreparedToEnterBattleComponent self)
        {
            self.Awake();
            self.TimerId = TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + 30 * 1000, self.OverTime);
        }
    }
    public class RoomPreparedToEnterBattleComponentDestroy : DestroySystem<RoomPreparedToEnterBattleComponent>
    {
        public override void Destroy(RoomPreparedToEnterBattleComponent self)
        {
            TimerComponent.Instance.Remove(ref self.TimerId);
        }
    }
    public static class RoomPreparedToEnterBattleComponentSystems
    {
        public static void OverTime(this RoomPreparedToEnterBattleComponent self)
        {
            Game.EventSystem.Publish(new RoomEventType.AllPlayerPrepared() { Scene = self.DomainScene() }).Coroutine();
        }
        public static void PlayerPrepare(this RoomPreparedToEnterBattleComponent self, long id)
        {
            self.PreparUnitId.Add(id);
            if (self.IsAllPrepar)
            {
                Game.EventSystem.Publish(new RoomEventType.AllPlayerPrepared() { Scene = self.DomainScene() }).Coroutine();
            }
        }
    }
    public class AllPlayerPreparedEvent : AEvent<RoomEventType.AllPlayerPrepared>
    {
        protected override async ETTask Run(RoomEventType.AllPlayerPrepared AllPrepared)
        {
            AllPrepared.Scene.AddComponent<BattleComponent>().BattleStartTime=TimeHelper.ServerNow();
            MessageHelper.BroadcastToRoom(AllPrepared.Scene,new L2C_AllowToEnterMap());
            AllPrepared.Scene.RemoveComponent<RoomPreparedToEnterBattleComponent>();
            await ETTask.CompletedTask;
        }
    }
}