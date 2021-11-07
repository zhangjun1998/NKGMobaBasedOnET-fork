namespace ET
{
    public abstract class ALSF_TickHandler<T> : ILSF_TickHandler where T : Entity
    {
        public abstract void OnLSF_Tick(T entity, long deltaTime);

        public void LSF_Tick(Entity entity, long deltaTime)
        {
            OnLSF_Tick(entity as T, deltaTime);
        }

#if !SERVER
        public virtual bool OnLSF_CheckConsistency(T entity, uint frame, ALSF_Cmd stateToCompare)
        {
            return true;
        }

        public virtual void OnLSF_PredictTick(T entity, long deltaTime)
        {
        }

        public virtual void OnLSF_RollBackTick(T entity, uint frame, ALSF_Cmd stateToCompare)
        {
        }

        public bool LSF_CheckConsistency(Entity entity, uint frame, ALSF_Cmd stateToCompare)
        {
            return OnLSF_CheckConsistency(entity as T, frame, stateToCompare);
        }

        public void LSF_PredictTick(Entity entity, long deltaTime)
        {
            OnLSF_PredictTick(entity as T, deltaTime);
        }



        public void LSF_RollBackTick(Entity entity, uint frame, ALSF_Cmd stateToCompare)
        {
            OnLSF_RollBackTick(entity as T, frame, stateToCompare);
        }
#endif
    }
}