namespace ET
{
    [LSF_Tickable(EntityType = typeof(Unit))]
    public class UnitTicker : ALSF_TickHandler<Unit>
    {
        public override void OnLSF_Tick(Unit entity)
        {
            entity.GetComponent<LSF_TickComponent>()?.Tick();
        }
    }
}