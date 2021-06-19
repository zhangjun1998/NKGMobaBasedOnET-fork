namespace ETModel
{
	[Config((int)(AppType.ClientM))]
	public partial class Client_UnitConfigCategory : ACategory<Client_UnitConfig>
	{
	}

	public class Client_UnitConfig: IConfig
	{
		public long Id { get; set; }
		public string UnitName;
		public int UnitAttributesDataId;
		public int UnitPassiveSkillId;
		public int UnitQSkillId;
		public int UnitWSkillId;
		public int UnitESkillId;
		public int UnitRSkillId;
	}
}
