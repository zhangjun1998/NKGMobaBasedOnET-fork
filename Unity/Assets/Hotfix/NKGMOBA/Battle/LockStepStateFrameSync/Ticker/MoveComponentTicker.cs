

namespace ET
{
    [LSF_Tickable(EntityType = typeof(MoveComponent))]
    public class MoveComponentTicker: ALSF_TickHandler<MoveComponent>
    {
        public override void OnLSF_Tick(MoveComponent entity)
        {
            //Log.Info($"--------MoveComponent：{TimeHelper.ClientNow()}");
        }
    }
}