using System;


namespace ET
{
	[MessageHandler]
	public class C2G_LoginGateHandler : AMRpcHandler<C2G_LoginGate, G2C_LoginGate>
	{
		protected override async ETTask Run(Session session, C2G_LoginGate request, G2C_LoginGate response, Action reply)
		{
			Scene scene = session.DomainScene();
			PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();

			long playerid = scene.GetComponent<GateSessionKeyComponent>().Get(request.Key);
			if (playerid == 0)
			{
				response.Error = ErrorCode.ERR_ConnectGateKeyError;
				response.Message = "Gate key验证失败!";
				reply();
				return;
			}
			var existPlayer = playerComponent.Get(playerid);
			//已经有.触发顶号逻辑
			if (existPlayer != null)
            {
				Game.EventSystem.Get(existPlayer.GateSessionId)?.Dispose();
				session.AddComponent<SessionPlayerComponent>().Player = existPlayer;
				existPlayer.GateSessionId = session.InstanceId;
				session.AddComponent<MailBoxComponent, MailboxType>(MailboxType.GateSession);
				response.PlayerId = existPlayer.Id;
				reply();
				return;
            }
			PlayerInfo playerInfo = await DBComponent.Instance.Query<PlayerInfo>(playerid);
            if (playerInfo==default)
            {
				response.Error = ErrorCode.ERR_ConnectGateKeyError;
				response.Message = "玩家数据未找到!";
				reply();
				return;
			}
			// long mapInstanceId = StartSceneConfigCategory.Instance.GetBySceneName(session.DomainZone(), "Map").InstanceId;
			// long lobbyInstanceId=StartSceneConfigCategory.Instance.GetBySceneName(session.DomainZone(), "Lobby").InstanceId;
			// Log.Debug("test-------------------"+mapInstanceId.ToString());
			// Log.Debug("test-------------------"+lobbyInstanceId.ToString());

			Player player = playerComponent.AddChildWithId<Player>(playerid);
			player.AddComponent(playerInfo);
			playerComponent.Add(player);
			
			session.AddComponent<SessionPlayerComponent>().Player = player;
			player.GateSessionId = session.InstanceId;
			
			session.AddComponent<MailBoxComponent, MailboxType>(MailboxType.GateSession);

			//// 随机分配一个Lobby
			//StartSceneConfig lobbyConfig = AddressHelper.GetLobby(session.DomainZone());
			
			//response.LobbyAddress = lobbyConfig.OuterIPPortForClient.ToString();
			response.PlayerId = player.Id;

			reply();
			await ETTask.CompletedTask;
		}
	}
}