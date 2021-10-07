namespace ET
{
    public class M2C_ChangePropertyHandler: AMHandler<M2C_ChangeProperty>
    {
        protected override async ETVoid Run(Session session, M2C_ChangeProperty message)
        {
            UnitComponent unitComponent = session.Domain.GetComponent<UnitComponent>();
            
            Unit unit = unitComponent.Get(message.UnitId);

            unit.GetComponent<NumericComponent>()[(NumericType)message.NumicType] = message.FinalValue;
            await ETTask.CompletedTask;
        }
    }
}