namespace ET
{
    public abstract class ALSF_TickHandler<T> : ILSF_TickHandler where T : Entity
    {
        public abstract void OnLSF_Tick(T entity);
        
        public void LSF_Tick(Entity entity)
        {
            this.OnLSF_Tick(entity as T);
        }
    }
}