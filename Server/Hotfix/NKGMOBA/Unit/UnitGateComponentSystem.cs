namespace ET
{
    public class UnitGateComponentAwakeSystem : AwakeSystem<UnitGateComponent, long>
	{
		public override void Awake(UnitGateComponent self, long gateSessionId)
		{
			self.Awake(gateSessionId);
			BroadcastMsgComponent comp = self.DomainScene().GetComponent<BroadcastMsgComponent>();
            if (comp!=null)
            {
				comp.AddProcessId(gateSessionId);
			}
		}
	}
	public class UnitGateComponentDestroySystem : DestroySystem<UnitGateComponent>
	{
		public override void Destroy(UnitGateComponent self)
		{
			BroadcastMsgComponent comp = self.DomainScene().GetComponent<BroadcastMsgComponent>();
			if (comp != null)
			{
				comp.ReduceProcessId(self.GateSessionActorId);
			}
			self.GateSessionActorId = 0;
		}
	}
}