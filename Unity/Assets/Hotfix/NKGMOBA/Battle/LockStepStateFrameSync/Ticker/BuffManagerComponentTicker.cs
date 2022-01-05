namespace ET
{
    [LSF_Tickable(EntityType = typeof(BuffManagerComponent))]
    public class BuffManagerComponentTicker: ALSF_TickHandler<BuffManagerComponent>
    {
        public override void OnLSF_Tick(BuffManagerComponent entity, uint currentFrame, long deltaTime)
        {
            entity.FixedUpdate(currentFrame);
        }
    }
}