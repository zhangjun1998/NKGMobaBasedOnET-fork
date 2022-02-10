using System.Collections.Generic;

namespace ET
{
    [LSF_Tickable(EntityType = typeof(BuffManagerComponent))]
    public class BuffManagerComponentTicker : ALSF_TickHandler<BuffManagerComponent>
    {
#if !SERVER
        /// <summary>
        /// 当然只需要客户端去做，因为服务端只需要进行脏数据构建就行了
        /// 只对比每帧脏数据即可
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="frame"></param>
        /// <param name="stateToCompare"></param>
        /// <returns></returns>
        public override bool OnLSF_CheckConsistency(BuffManagerComponent entity, uint frame, ALSF_Cmd stateToCompare)
        {
            LSF_SyncBuffCmd syncBuffCmd = stateToCompare as LSF_SyncBuffCmd;

            if (syncBuffCmd == null)
            {
                return true;
            }

            stateToCompare.HasHandled = true;

            if (entity.BuffSnapInfos_DeltaOnly.TryGetValue(frame, out var localDeltaSnaps))
            {
                return localDeltaSnaps.Check(syncBuffCmd.BuffSnapInfo);
            }

            return false;
        }
#endif

        /// <summary>
        /// 我们在每一帧的Tick结尾都自动检测脏数据（对于服务端来说），这样就不需要我们手动去维护黑板值改变时需要做的数据收集工作
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="frame"></param>
        /// <param name="deltaTime"></param>
        public override void OnLSF_TickEnd(BuffManagerComponent entity, uint frame, long deltaTime)
        {
            Unit unit = entity.GetParent<Unit>();

            entity.BuffSnapInfos_Whole[frame] = entity.AcquireCurrentFrameBBValueSnap();
            
            BuffSnapInfo currentFrameBuffSnapWhole = entity.BuffSnapInfos_Whole[frame];
            BuffSnapInfo deltaBuffSnapInfo = null;
            
            bool hasPreviousSnapValue = entity.BuffSnapInfos_Whole.ContainsKey(frame - 1);

            // 如果前一帧有他的数据，就对比脏数据，如果没有，就直接把当前帧所有数据作为脏数据
            if (hasPreviousSnapValue)
            {
                // 与前一帧快照对比得出脏数据
                deltaBuffSnapInfo = currentFrameBuffSnapWhole.GetDifference(entity.BuffSnapInfos_Whole[frame - 1]);
            }
            else
            {
                deltaBuffSnapInfo = currentFrameBuffSnapWhole;
                foreach (var snap in deltaBuffSnapInfo.FrameBuffChangeSnap)
                {
                    snap.Value.OperationType = BuffInfo.BuffOperationType.ADD;
                }
            }

            // 如果没有脏数据，就直接返回
            if (deltaBuffSnapInfo.FrameBuffChangeSnap.Count == 0)
            {
                return;
            }
            
            entity.BuffSnapInfos_DeltaOnly[frame] = deltaBuffSnapInfo;
            
            LSF_SyncBuffCmd syncBuffCmd =
                ReferencePool.Acquire<LSF_SyncBuffCmd>().Init(unit.Id) as LSF_SyncBuffCmd;

            syncBuffCmd.BuffSnapInfo = deltaBuffSnapInfo;
            syncBuffCmd.Frame = frame;

#if SERVER
            unit.BelongToRoom.GetComponent<LSF_Component>().AddCmdToSendQueue(syncBuffCmd);
#endif
        }


        public override void OnLSF_Tick(BuffManagerComponent entity, uint currentFrame, long deltaTime)
        {
            entity.FixedUpdate(currentFrame);
        }
    }
}