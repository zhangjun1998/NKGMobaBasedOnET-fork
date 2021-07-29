namespace ETModel
{
	[ObjectSystem]
	public class UnitGateComponentAwakeSystem : AwakeSystem<UnitGateComponent, long>
	{
		public override void Awake(UnitGateComponent self, long a)
		{
			self.Awake(a);
		}
	}
	[ObjectSystem]
	public class UnitGateComponentDestroySystem : DestroySystem<UnitGateComponent>
	{
		public override void Destroy(UnitGateComponent self)
		{
			self.IsDisconnect = false;
			self.GateSessionActorId = 0;
		}
	}
	public class UnitGateComponent : Component, ISerializeToEntity
	{
		/// <summary>
		/// Gate的ActorId（自身的entity.id）
		/// </summary>
		public long GateSessionActorId;

		public bool IsDisconnect;

		public void Awake(long gateSessionId)
		{
			this.GateSessionActorId = gateSessionId;
			IsDisconnect = false;
		}
	}
}