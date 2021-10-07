//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年10月2日 17:39:06
//------------------------------------------------------------

namespace ET
{
    public class SendBuffInfoToClientBuffSystem : ABuffSystemBase<SendBuffInfoToClientBuffData>
    {
#if SERVER
        public override void OnExecute()
        {
            SendBuffInfoToClientBuffData sendBuffInfoToClientBuffData = this.GetBuffDataWithTType;

            IBuffSystem targetBuffSystem = this.TheUnitBelongto.GetComponent<BuffManagerComponent>()
                .GetBuffById(
                    (this.BelongtoRuntimeTree.BelongNP_DataSupportor.BuffNodeDataDic[
                            sendBuffInfoToClientBuffData.TargetBuffNodeId.Value] as
                        NormalBuffNodeData).BuffData.BuffId);

            MessageHelper.BroadcastToRoom(this.GetBuffTarget().BelongToRoom,
                new M2C_BuffInfo()
                {
                    UnitId = this.BelongtoRuntimeTree.BelongToUnit.Id,
                    SkillId = sendBuffInfoToClientBuffData.BelongToSkillId.Value,
                    BBKey = sendBuffInfoToClientBuffData.BBKey.BBKey,
                    TheUnitFromId = this.TheUnitFrom.Id,
                    TheUnitBelongToId = this.TheUnitBelongto.Id,
                    BuffLayers = targetBuffSystem.CurrentOverlay,
                    BuffMaxLimitTime = targetBuffSystem.MaxLimitTime,
                });
        }
#else
        public override void OnExecute()
        {
            throw new System.NotImplementedException();
        }
#endif
    }
}