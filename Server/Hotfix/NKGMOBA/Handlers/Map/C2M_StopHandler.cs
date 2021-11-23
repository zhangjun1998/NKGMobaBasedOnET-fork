using System.Collections.Generic;
using UnityEngine;

namespace ET
{
	[ActorMessageHandler]
	public class C2M_StopHandler : AMActorHandler<Player, C2M_Stop>
	{
		protected override async ETTask Run(Player player, C2M_Stop message)
		{
			Unit unit = player.Domain.GetComponent<UnitComponent>().Get(player.UnitId);
			unit.Stop(0);
			await ETTask.CompletedTask;
		}
	}
}