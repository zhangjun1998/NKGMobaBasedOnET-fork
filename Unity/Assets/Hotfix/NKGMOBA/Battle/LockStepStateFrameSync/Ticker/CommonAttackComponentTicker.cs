namespace ET
{
    [LSF_Tickable(EntityType = typeof(CommonAttackComponent_Logic))]
    public class CommonAttackComponentTicker: ALSF_TickHandler<CommonAttackComponent_Logic>
    {
        public override void OnLSF_Tick(CommonAttackComponent_Logic entity, uint currentFrame, long deltaTime)
        {
            entity.FixedUpdate();
        }
    }
}