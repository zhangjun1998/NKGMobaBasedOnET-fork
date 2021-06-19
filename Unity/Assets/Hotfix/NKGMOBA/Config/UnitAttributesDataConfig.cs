using ETModel;

namespace ETHotfix
{
	[Config((int)(AppType.Gate | AppType.Map))]
	public partial class UnitAttributesDataConfigCategory : ACategory<UnitAttributesDataConfig>
	{
	}

	public class UnitAttributesDataConfig: IConfig
	{
		public long Id { get; set; }
		public long UnitAttributesDataSupportorId;
	}
}
