namespace ET
{
    [LSF_Tickable(EntityType = typeof(UnitComponent))]
    public class UnitComponentTicker : ALSF_TickHandler<UnitComponent>
    {
        public override void OnLSF_Tick(UnitComponent entity, long deltaTime)
        {
            using (ListComponent<Unit> unitsToTick = new ListComponent<Unit>())
            {
                foreach (var allUnit in entity.idUnits)
                {
                    unitsToTick.List.Add(allUnit.Value);
                }
                
                foreach (var unitToTick in unitsToTick.List)
                {
                    if (entity.idUnits.TryGetValue(unitToTick.Id, out var unit))
                    {
                        LSF_TickDispatcherComponent.Instance.HandleLSF_Tick(unit, deltaTime);
                    }
                }
            }
        }
    }
}