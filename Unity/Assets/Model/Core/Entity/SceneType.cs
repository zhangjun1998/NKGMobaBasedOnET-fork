namespace ET
{
	public enum SceneType
	{
		Process = 0,
		Manager = 1,
		Realm = 2,
		Gate = 3,
		Http = 4,
		Location = 5,
		Map = 6,
		/// <summary>
		/// 房间代理.用于生成真正的room
		/// </summary>
		RoomAgent = 7,
		RoomManager = 8,
		Room = 9,
		// 客户端Model层
		Client = 30,
		Zone = 31,
		Login = 32,
		Robot = 33,
	}
}