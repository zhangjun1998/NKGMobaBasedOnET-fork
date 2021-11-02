namespace ET
{
    public class BroadcastActorSyncMessageHandler : AMHandler<BroadcastActorSyncMessage>
	{
		protected override async ETVoid Run(Session scene, BroadcastActorSyncMessage message)
		{
            switch (message.OperaType)
            {
                case true:
                    BroadcastMsgOnGateComponent.Instance.AddSessionId(message.TargetActorId, message.SessionId);
                    break;
                case false:
                    BroadcastMsgOnGateComponent.Instance.RemoveSessionId(message.TargetActorId, message.SessionId);
                    break;
            }
			await ETTask.CompletedTask;
		}
	}
}