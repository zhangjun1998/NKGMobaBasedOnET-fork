namespace ET
{
	public class PlayerComponent: Entity
	{
		public Session GateSession;

		/// <summary>
		/// 已经完成的加载次数
		/// </summary>
		public int HasCompletedLoadCount;
		
		public long PlayerId;
		public string PlayerAccount;
	}
}
