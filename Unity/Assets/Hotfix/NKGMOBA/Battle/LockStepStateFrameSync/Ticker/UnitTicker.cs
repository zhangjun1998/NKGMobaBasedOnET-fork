﻿namespace ET
{
    [LSF_Tickable(EntityType = typeof(Unit))]
    public class UnitTicker : ALSF_TickHandler<Unit>
    {
        public override void OnLSF_Tick(Unit entity, long deltaTime)
        {
            entity.GetComponent<LSF_TickComponent>()?.Tick(deltaTime);
        }
        
#if !SERVER
        public override void OnLSF_ViewTick(Unit entity, long deltaTime)
        {
            LSF_TickComponent lsfTickComponent = entity.GetComponent<LSF_TickComponent>();
            if (lsfTickComponent != null)
            {
                entity.GetComponent<LSF_TickComponent>().TickView(deltaTime);
            }
        }

        public override void OnLSF_PredictTick(Unit entity, long deltaTime)
        {
            LSF_TickComponent lsfTickComponent = entity.GetComponent<LSF_TickComponent>();
            if (lsfTickComponent != null)
            {
                entity.GetComponent<LSF_TickComponent>().Predict(deltaTime);
            }
        }

        public override void OnLSF_RollBackTick(Unit entity, uint frame, ALSF_Cmd stateToCompare)
        {
            LSF_TickComponent lsfTickComponent = entity.GetComponent<LSF_TickComponent>();
            if (lsfTickComponent != null)
            {
                entity.GetComponent<LSF_TickComponent>().RollBack(frame, stateToCompare);
            }
        }
        
        public override bool OnLSF_CheckConsistency(Unit entity, uint frame, ALSF_Cmd stateToCompare)
        {
            LSF_TickComponent lsfTickComponent = entity.GetComponent<LSF_TickComponent>();
            if (lsfTickComponent != null)
            {
                return entity.GetComponent<LSF_TickComponent>().CheckConsistency(frame, stateToCompare);
            }

            return true;
        }

#endif
    }
}