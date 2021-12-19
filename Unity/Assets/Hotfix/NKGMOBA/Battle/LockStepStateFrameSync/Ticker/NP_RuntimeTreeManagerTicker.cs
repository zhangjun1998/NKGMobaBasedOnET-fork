using System.Collections.Generic;

namespace ET
{
    [LSF_Tickable(EntityType = typeof(NP_RuntimeTreeManager))]
    public class NP_RuntimeTreeManagerTicker : ALSF_TickHandler<NP_RuntimeTreeManager>
    {
#if !SERVER
        /// <summary>
        /// 这种可变内容的状态数据一致性检查比较特殊，需要使用发过来的脏数据和本地已经经过验证的数据做merge之后再检查才行（当然只需要客户端去做，因为服务端只需要进行脏数据构建就行了）
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="frame"></param>
        /// <param name="stateToCompare"></param>
        /// <returns></returns>
        public override bool OnLSF_CheckConsistency(NP_RuntimeTreeManager entity, uint frame, ALSF_Cmd stateToCompare)
        {
            LSF_ChangeBBValue changeBbValue = stateToCompare as LSF_ChangeBBValue;

            if (changeBbValue == null)
            {
                return true;
            }

            if (entity.FrameSnaps_DeltaOnly.TryGetValue(frame - 1, out var previousSnaps))
            {
                if (previousSnaps.TryGetValue(changeBbValue.TargetNPBehaveTreeId, out var previousSnap))
                {
                    previousSnap.NP_RuntimeTreeBBSnap.Merge(changeBbValue.NP_RuntimeTreeBBSnap);

                    if (entity.FrameSnaps_DeltaOnly.TryGetValue(frame, out var targetFrameSnaps))
                    {
                        if (targetFrameSnaps.TryGetValue(changeBbValue.TargetNPBehaveTreeId, out var targetFrameSnap))
                        {
                            return previousSnap.NP_RuntimeTreeBBSnap.Check(targetFrameSnap.NP_RuntimeTreeBBSnap);
                        }
                    }
                }
            }

            return false;
        }
#endif

        public override void OnLSF_Tick(NP_RuntimeTreeManager entity, uint currentFrame, long deltaTime)
        {
        }

        /// <summary>
        /// 我们在每一帧的Tick结尾都自动检测脏数据（对于服务端来说），这样就不需要我们手动去维护黑板值改变时需要做的数据收集工作
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="frame"></param>
        /// <param name="deltaTime"></param>
        public override void OnLSF_TickEnd(NP_RuntimeTreeManager entity, uint frame, long deltaTime)
        {
            Unit unit = entity.GetParent<Unit>();
            entity.FrameSnaps_DeltaOnly[frame] = new Dictionary<long, LSF_ChangeBBValue>();
            entity.FrameSnaps_Whole[frame] = new Dictionary<long, NP_RuntimeTreeBBSnap>();

            foreach (var runtimeTree in entity.RuntimeTrees)
            {
                NP_RuntimeTreeBBSnap npRuntimeTreeBbSnap = runtimeTree.Value.AcquireCurrentFrameBBValueSnap();
                entity.FrameSnaps_Whole[frame].Add(runtimeTree.Key, npRuntimeTreeBbSnap);

                LSF_ChangeBBValue changeBbValue =
                    ReferencePool.Acquire<LSF_ChangeBBValue>().Init(unit.Id) as LSF_ChangeBBValue;
                changeBbValue.TargetNPBehaveTreeId = runtimeTree.Key;
                changeBbValue.Frame = frame;

                bool hasPreviousSnapValue = entity.FrameSnaps_Whole.ContainsKey(frame - 1) &&
                                            entity.FrameSnaps_Whole[frame - 1].ContainsKey(runtimeTree.Key);

                // 如果前一帧有他的数据，就对比脏数据，如果没有，就直接把当前帧所有数据作为脏数据
                if (hasPreviousSnapValue)
                {
                    // 与前一帧快照对比得出脏数据
                    changeBbValue.NP_RuntimeTreeBBSnap =
                        npRuntimeTreeBbSnap.GetDifference(entity.FrameSnaps_Whole[frame - 1][runtimeTree.Key]);
                }
                else
                {
                    changeBbValue.NP_RuntimeTreeBBSnap = npRuntimeTreeBbSnap;
                }

                // 如果没有脏数据，就直接返回
                if (changeBbValue.NP_RuntimeTreeBBSnap.NP_FrameBBValues.Count == 0 &&
                    changeBbValue.NP_RuntimeTreeBBSnap.NP_FrameBBValueOperations.Count == 0)
                {
                    return;
                }

#if SERVER
                unit.BelongToRoom.GetComponent<LSF_Component>().AddCmdToSendQueue(changeBbValue);
#else
                entity.FrameSnaps_DeltaOnly[frame][runtimeTree.Key] = changeBbValue;
#endif
            }
        }
    }
}