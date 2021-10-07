namespace ET
{
    [NumericWatcher(NumericType.Attack)]
    public class NumericWatcher_AttackValue: INumericWatcher
    {
        public void Run(NumericComponent numericComponent, NumericType numericType, float value)
        {
#if SERVER
            Unit unit = numericComponent.GetParent<Unit>();

            if (!(unit is null))
            {
                MessageHelper.BroadcastToRoom(unit.BelongToRoom,
                    new M2C_ChangeProperty() {UnitId = unit.Id, FinalValue = value, NumicType = (int) numericType});
            }
#else
            Game.EventSystem.Publish(new EventType.UnitChangeProperty()
                {FinalValue = value, Sprite = numericComponent.GetParent<Unit>(), NumericType = NumericType.MaxHp});
#endif
        }
    }
}