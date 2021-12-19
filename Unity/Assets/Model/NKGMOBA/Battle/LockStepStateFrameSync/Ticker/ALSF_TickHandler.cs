namespace ET
{
    public abstract class ALSF_TickHandler<T> : ILSF_TickHandler where T : Entity
    {
        public abstract void OnLSF_Tick(T entity, uint currentFrame, long deltaTime);
        
        public virtual void OnLSF_TickEnd(T entity, uint frame, long deltaTime)
        {
        }

        public void LSF_Tick(Entity entity, uint currentFrame, long deltaTime)
        {
            OnLSF_Tick(entity as T, currentFrame, deltaTime);
        }
        
        public void LSF_TickEnd(Entity entity, uint frame, long deltaTime)
        {
            OnLSF_TickEnd(entity as T, frame, deltaTime);
        }

        public virtual bool OnLSF_CheckConsistency(T entity, uint frame, ALSF_Cmd stateToCompare)
        {
            return true;
        }

#if !SERVER
        /// <summary>
        /// 视图层Tick，帧率可为60，90，120，144等
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="deltaTime"></param>
        public virtual void OnLSF_ViewTick(T entity, long deltaTime)
        {
        }

        public virtual void OnLSF_RollBackTick(T entity, uint frame, ALSF_Cmd stateToCompare)
        {
        }

        public bool LSF_CheckConsistency(Entity entity, uint frame, ALSF_Cmd stateToCompare)
        {
            return OnLSF_CheckConsistency(entity as T, frame, stateToCompare);
        }

        /// <summary>
        /// 视图层Tick，帧率可为60，90，120，144等
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="deltaTime"></param>
        public void LSF_ViewTick(Entity entity, long deltaTime)
        {
            OnLSF_ViewTick(entity as T, deltaTime);
        }

        public void LSF_RollBackTick(Entity entity, uint frame, ALSF_Cmd stateToCompare)
        {
            OnLSF_RollBackTick(entity as T, frame, stateToCompare);
        }
#endif
    }
}