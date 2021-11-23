using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
	[ResponseType(typeof(L2C_RoomListInLobby))]
	[Message(OuterOpcode_Lobby.C2L_RoomListInLobby)]
	[ProtoContract]
	public partial class C2L_RoomListInLobby: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode_Lobby.L2C_RoomListInLobby)]
	[ProtoContract]
	public partial class L2C_RoomListInLobby: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<RoomInfoProto> RoomList = new List<RoomInfoProto>();

	}

///////////////////////////////// 房间相关  START ///////////////////////////////////
//房间配置数据.按需扩展
	[Message(OuterOpcode_Lobby.RoomConfigProto)]
	[ProtoContract]
	public partial class RoomConfigProto: Object
	{
		[ProtoMember(1)]
		public string RoomName { get; set; }

		[ProtoMember(2)]
		public int RoomPlayerNum { get; set; }

	}

	[Message(OuterOpcode_Lobby.RoomInfoProto)]
	[ProtoContract]
	public partial class RoomInfoProto: Object
	{
		[ProtoMember(1)]
		public long RoomId { get; set; }

		[ProtoMember(2)]
		public RoomConfigProto RoomConfig { get; set; }

		[ProtoMember(3)]
		public long RoomHolderPlayer { get; set; }

		[ProtoMember(4)]
		public int PlayerCount { get; set; }

		[ProtoMember(5)]
		public bool IsGameing { get; set; }

	}

	[ResponseType(typeof(L2C_CreateNewRoomLobby))]
	[Message(OuterOpcode_Lobby.C2L_CreateNewRoomLobby)]
	[ProtoContract]
	public partial class C2L_CreateNewRoomLobby: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode_Lobby.L2C_CreateNewRoomLobby)]
	[ProtoContract]
	public partial class L2C_CreateNewRoomLobby: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public RoomInfoProto RoomInfo { get; set; }

		[ProtoMember(2)]
		public List<PlayerInfoRoom> playerInfoRoom = new List<PlayerInfoRoom>();

	}

	[ResponseType(typeof(L2C_JoinRoomLobby))]
	[Message(OuterOpcode_Lobby.C2L_JoinRoomLobby)]
	[ProtoContract]
	public partial class C2L_JoinRoomLobby: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long RoomId { get; set; }

	}

	[Message(OuterOpcode_Lobby.L2C_JoinRoomLobby)]
	[ProtoContract]
	public partial class L2C_JoinRoomLobby: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public RoomInfoProto RoomInfo { get; set; }

		[ProtoMember(2)]
		public List<PlayerInfoRoom> playerInfoRoom = new List<PlayerInfoRoom>();

		[ProtoMember(3)]
		public long RoomPlayerActorId { get; set; }

	}

	[Message(OuterOpcode_Lobby.PlayerInfoRoom)]
	[ProtoContract]
	public partial class PlayerInfoRoom: Object
	{
		[ProtoMember(1)]
		public string Name { get; set; }

		[ProtoMember(2)]
		public int camp { get; set; }

		[ProtoMember(3)]
		public long playerid { get; set; }

	}

	[Message(OuterOpcode_Lobby.L2C_PlayerTriggerRoom)]
	[ProtoContract]
	public partial class L2C_PlayerTriggerRoom: Object, IActorMessage
	{
		[ProtoMember(1)]
		public PlayerInfoRoom playerInfoRoom { get; set; }

		[ProtoMember(2)]
		public bool JoinOrLeave { get; set; }

		[ProtoMember(3)]
		public long newRoomHolder { get; set; }

		[ProtoMember(4)]
		public bool IsDestory { get; set; }

	}

//房间关闭
	[Message(OuterOpcode_Lobby.L2C_RoomClose)]
	[ProtoContract]
	public partial class L2C_RoomClose: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int CloseCode { get; set; }

	}

	[ResponseType(typeof(L2C_LeaveRoomLobby))]
	[Message(OuterOpcode_Lobby.C2L_LeaveRoomLobby)]
	[ProtoContract]
	public partial class C2L_LeaveRoomLobby: Object, IRoomRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode_Lobby.L2C_LeaveRoomLobby)]
	[ProtoContract]
	public partial class L2C_LeaveRoomLobby: Object, IRoomResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode_Lobby.PlayerBattleInfo)]
	[ProtoContract]
	public partial class PlayerBattleInfo: Object
	{
		[ProtoMember(1)]
		public long PlayerId { get; set; }

		[ProtoMember(2)]
		public int HeroConfig { get; set; }

	}

	[ResponseType(typeof(L2C_StartGameLobby))]
	[Message(OuterOpcode_Lobby.C2L_StartGameLobby)]
	[ProtoContract]
	public partial class C2L_StartGameLobby: Object, IRoomRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode_Lobby.L2C_StartGameLobby)]
	[ProtoContract]
	public partial class L2C_StartGameLobby: Object, IRoomResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<long> list = new List<long>();

	}

	[Message(OuterOpcode_Lobby.PlayerBattlePoint)]
	[ProtoContract]
	public partial class PlayerBattlePoint: Object
	{
		[ProtoMember(1)]
		public long PlayerId { get; set; }

		[ProtoMember(2)]
		public int Point { get; set; }

		[ProtoMember(3)]
		public int SingleMatchCount { get; set; }

		[ProtoMember(4)]
		public string Account { get; set; }

		[ProtoMember(5)]
		public long UnitId { get; set; }

	}

	[Message(OuterOpcode_Lobby.L2C_PrepareToEnterBattle)]
	[ProtoContract]
	public partial class L2C_PrepareToEnterBattle: Object, IActorMessage
	{
		[ProtoMember(1)]
		public List<UnitInfo> Units = new List<UnitInfo>();

	}

	[Message(OuterOpcode_Lobby.C2L_PreparedToEnterBattle)]
	[ProtoContract]
	public partial class C2L_PreparedToEnterBattle: Object, IRoomMessage
	{
		[ProtoMember(1)]
		public long PlayerId { get; set; }

	}

	[Message(OuterOpcode_Lobby.L2C_AllowToEnterMap)]
	[ProtoContract]
	public partial class L2C_AllowToEnterMap: Object, IActorMessage
	{
	}

}
