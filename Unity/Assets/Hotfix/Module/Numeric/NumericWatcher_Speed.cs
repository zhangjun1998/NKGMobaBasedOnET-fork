namespace ET
{
    [NumericWatcher(NumericType.Speed)]
    public class NumericWatcher_Speed : INumericWatcher
    {
        public void Run(NumericComponent numericComponent, NumericType numericType, float value)
        {
#if !SERVER
            Unit unit = numericComponent.GetParent<Unit>();
            Game.EventSystem.Publish(new EventType.UnitChangeProperty()
                {FinalValue = value, Sprite = numericComponent.GetParent<Unit>(), NumericType = numericType});
#endif
        }
    }
}