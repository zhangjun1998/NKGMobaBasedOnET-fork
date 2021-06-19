using ETModel;

namespace ETHotfix
{
	[Config((int)(AppType.Gate | AppType.Map))]
	public partial class Server_B2SColliderConfigCategory : ACategory<Server_B2SColliderConfig>
	{
	}

	public class Server_B2SColliderConfig: IConfig
	{
		public long Id { get; set; }
		public long B2S_ColliderId;
		public bool SyncToUnit;
	}
}
