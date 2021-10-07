using UnityEngine;

namespace ET
{
	public class EnterMapFinish_RemoveLobbyUI: AEvent<EventType.EnterMapFinish>
	{
		protected override async ETTask Run(EventType.EnterMapFinish args)
		{
			Scene scene = args.ZoneScene;
			
			scene.GetComponent<FUIManagerComponent>().Remove(FUIPackage.Lobby);

			await ETTask.CompletedTask;
		}
	}
}
