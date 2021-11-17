//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月21日 16:04:06
//------------------------------------------------------------

namespace ET
{
    /// <summary>
    /// 监听Buff回调
    /// </summary>
    public class ListenBuffCallBackBuffSystem: ABuffSystemBase<ListenBuffCallBackBuffData>
    {
        public ListenBuffEvent_Normal ListenBuffEventNormal;
        
        public override void OnExecute()
        {
            if (GetBuffDataWithTType.HasOverlayerJudge)
            {
                ListenBuffEventNormal = ReferencePool.Acquire<ListenBuffEvent_CheckOverlay>();
                ListenBuffEventNormal.BuffInfoWillBeAdded = GetBuffDataWithTType.BuffInfoWillBeAdded;
                var listenOverLayer = ListenBuffEventNormal as ListenBuffEvent_CheckOverlay;
                listenOverLayer.TargetOverlay = GetBuffDataWithTType.TargetOverLayer;
            }
            else
            {
                ListenBuffEventNormal = ReferencePool.Acquire<ListenBuffEvent_Normal>();
                ListenBuffEventNormal.BuffInfoWillBeAdded = GetBuffDataWithTType.BuffInfoWillBeAdded;
            }
            this.GetBuffTarget().Domain.GetComponent<BattleEventSystem>().RegisterEvent($"{this.GetBuffDataWithTType.EventId.Value}{this.TheUnitFrom.Id}", ListenBuffEventNormal);
        }

        public override void OnFinished()
        {
            this.GetBuffTarget().Domain.GetComponent<BattleEventSystem>().UnRegisterEvent($"{this.GetBuffDataWithTType.EventId.Value}{this.TheUnitFrom.Id}", ListenBuffEventNormal);
        }
    }
}