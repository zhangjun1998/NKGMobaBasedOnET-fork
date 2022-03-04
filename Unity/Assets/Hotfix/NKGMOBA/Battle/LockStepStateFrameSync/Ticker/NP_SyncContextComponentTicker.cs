namespace ET
{
    [LSF_Tickable(EntityType = typeof(NP_SyncComponent))]
    public class NP_SyncContextComponentTicker: ALSF_TickHandler<NP_SyncComponent>
    {
        public override void OnLSF_TickStart(NP_SyncComponent entity, uint frame, long deltaTime)
        {

        }

        public override void OnLSF_Tick(NP_SyncComponent entity, uint currentFrame, long deltaTime)
        {
            entity.FixedUpdate(currentFrame);
        }
    }
}