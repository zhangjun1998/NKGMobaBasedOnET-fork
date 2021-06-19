using ETModel;

namespace ETHotfix
{
	[Config((int)(AppType.Gate | AppType.Map))]
	public partial class Server_B2SCollisionRelationConfigCategory : ACategory<Server_B2SCollisionRelationConfig>
	{
	}

	public class Server_B2SCollisionRelationConfig: IConfig
	{
		public long Id { get; set; }
		public int B2S_ColliderConfigId;
		public string B2S_ColliderHandlerName;
		public bool FriendlyHero;
		public bool EnemyHero;
		public bool FriendlySoldier;
		public bool EnemySoldier;
		public bool FriendlyBuilds;
		public bool EnemyBuilds;
		public bool Creeps;
	}
}
