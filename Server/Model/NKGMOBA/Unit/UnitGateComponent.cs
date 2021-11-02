namespace ET
{

    public class UnitGateComponent : Entity, ISerializeToEntity
	{
		public long GateSessionActorId;

		public void Awake(long gateSessionId)
		{
			this.GateSessionActorId = gateSessionId;
		}
	}
}