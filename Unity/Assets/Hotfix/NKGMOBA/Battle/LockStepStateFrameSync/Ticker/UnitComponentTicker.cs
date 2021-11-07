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

#if !SERVER
        public override void OnLSF_PredictTick(UnitComponent entity, long deltaTime)
        {
            foreach (var allUnit in entity.idUnits)
            {
                LSF_TickDispatcherComponent.Instance.HandleLSF_PredictTick(allUnit.Value, deltaTime);
            }
        }

        public override void OnLSF_RollBackTick(UnitComponent entity, uint frame, ALSF_Cmd stateToCompare)
        {
            foreach (var allUnit in entity.idUnits)
            {
                LSF_TickDispatcherComponent.Instance.HandleLSF_RollBack(allUnit.Value, frame,
                    stateToCompare);
            }
        }

        public override bool OnLSF_CheckConsistency(UnitComponent entity, uint frame, ALSF_Cmd stateToCompare)
        {
            foreach (var allUnit in entity.idUnits)
            {
                return LSF_TickDispatcherComponent.Instance.HandleLSF_CheckConsistency(allUnit.Value, frame,
                    stateToCompare);
            }

            return true;
        }
#endif
    }
}