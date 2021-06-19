namespace ETModel
{
	[Config((int)(AppType.Gate | AppType.Map))]
	public partial class Server_UnitConfigCategory : ACategory<Server_UnitConfig>
	{
	}

	public class Server_UnitConfig: IConfig
	{
		public long Id { get; set; }
		public int UnitAttributesDataId;
		public int UnitColliderDataConfigId;
		public int UnitPassiveSkillId;
		public int UnitQSkillId;
		public int UnitWSkillId;
		public int UnitESkillId;
		public int UnitRSkillId;
	}
}
