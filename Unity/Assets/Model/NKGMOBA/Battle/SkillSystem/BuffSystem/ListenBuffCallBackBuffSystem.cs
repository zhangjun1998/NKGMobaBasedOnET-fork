//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月21日 16:04:06
//------------------------------------------------------------

namespace ETModel
{
    /// <summary>
    /// 监听Buff回调
    /// </summary>
    public class ListenBuffCallBackBuffSystem: ABuffSystemBase<ListenBuffCallBackBuffData>
    {
        public override void OnExecute()
        {
            Game.Scene.GetComponent<BattleEventSystem>().RegisterEvent($"{this.GetBuffDataWithTType.EventId}{this.TheUnitFrom.Id}", this.GetBuffDataWithTType.ListenBuffEventNormal);
        }

        public override void OnFinished()
        {
            Game.Scene.GetComponent<BattleEventSystem>().UnRegisterEvent($"{this.GetBuffDataWithTType.EventId}{this.TheUnitFrom.Id}", this.GetBuffDataWithTType.ListenBuffEventNormal);
        }
    }
}