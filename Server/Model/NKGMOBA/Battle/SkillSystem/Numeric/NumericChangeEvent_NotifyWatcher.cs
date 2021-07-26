namespace ETModel
{
	// 分发数值监听
	[Event(EventIdType.NumbericChange)]
	public class NumericChangeEvent_NotifyWatcher: AEvent<Unit, NumericType, float>
	{
		public override void Run(Unit unit, NumericType numericType, float value)
		{
			Game.Scene.GetComponent<NumericWatcherComponent>().Run(numericType, unit, value);
		}
	}
}
