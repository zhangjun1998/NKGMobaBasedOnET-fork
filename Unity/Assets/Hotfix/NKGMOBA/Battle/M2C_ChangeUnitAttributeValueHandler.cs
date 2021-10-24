//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月21日 17:41:28
//------------------------------------------------------------

namespace ET
{
    public class M2C_ChangeUnitAttributeValueHandler : AMHandler<M2C_ChangeUnitAttribute>
    {
        protected override async ETVoid Run(Session session, M2C_ChangeUnitAttribute message)
        {
            Unit unit = session.DomainScene().GetComponent<RoomManagerComponent>().GetBattleRoom().GetComponent<UnitComponent>().Get(message.UnitId);
            Game.EventSystem.Publish(new EventType.ChangeUnitAttribute()
            {
                Unit = unit,
                NumericType = (NumericType) message.NumericType,
                ChangeValue = message.ChangeValue
            }).Coroutine();
            await ETTask.CompletedTask;
        }
    }
}