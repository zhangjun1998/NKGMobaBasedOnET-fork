namespace ET
{
    public interface ILSF_TickHandler
    {
        /// <summary>
        /// 正常Tick
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="deltaTime"></param>
        void LSF_Tick(Entity entity, long deltaTime);

#if !SERVER 
        /// <summary>
        /// 检测结果一致性
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="alsfCmd"></param>
        bool LSF_CheckConsistency(Entity entity, uint frame, ALSF_Cmd stateToCompare);

        /// <summary>
        /// 预测
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="deltaTime"></param>
        void LSF_PredictTick(Entity entity, long deltaTime);
        
        /// <summary>
        /// 回滚
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="deltaTime"></param>
        void LSF_RollBackTick(Entity entity, uint frame, ALSF_Cmd stateToCompare);
#endif
    }
}