using ETModel;

namespace ETHotfix
{

    [ObjectSystem]
    public class BattleLoadingComponentAwakeSystem : AwakeSystem<BattleLoadingComponent>
    {
        public override void Awake(BattleLoadingComponent self)
        {
            self.Awake();
            var keys=self.GetParent<RoomEntity>().Players.Keys;
            MessageHelper.Broadcast()

        }
    }
}
