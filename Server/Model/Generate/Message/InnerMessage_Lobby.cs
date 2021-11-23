using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
	[Message(InnerOpcode_Lobby.L2G_GetRoomId)]
	[ProtoContract]
	public partial class L2G_GetRoomId: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long RoomId { get; set; }

	}

	[ResponseType(typeof(M2L_CreateHeroUnit))]
	[Message(InnerOpcode_Lobby.L2M_CreateHeroUnit)]
	[ProtoContract]
	public partial class L2M_CreateHeroUnit: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long PlayerId { get; set; }

		[ProtoMember(2)]
		public long GateSessionId { get; set; }

		[ProtoMember(3)]
		public long Roomid { get; set; }

		[ProtoMember(4)]
		public PlayerBattleInfo PlayerBattleInfo { get; set; }

	}

	[ResponseType(typeof(M2L_PreparedToEnterBattle))]
	[Message(InnerOpcode_Lobby.L2M_PreparedToEnterBattle)]
	[ProtoContract]
	public partial class L2M_PreparedToEnterBattle: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public long Roomid { get; set; }

	}

//gate向RoomManager申请新的房间
	[ResponseType(typeof(RM2G_CreateNewRoomLobby))]
	[Message(InnerOpcode_Lobby.G2RM_CreateNewRoomLobby)]
	[ProtoContract]
	public partial class G2RM_CreateNewRoomLobby: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public RoomConfigProto RoomConfig { get; set; }

	}

	[Message(InnerOpcode_Lobby.RM2G_CreateNewRoomLobby)]
	[ProtoContract]
	public partial class RM2G_CreateNewRoomLobby: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public RoomInfoProto RoomInfo { get; set; }

	}

//房间管理场景向RoomAgent申请创建房间
	[ResponseType(typeof(RA2RM_CreateNewRoomLobby))]
	[Message(InnerOpcode_Lobby.RM2RA_CreateNewRoomLobby)]
	[ProtoContract]
	public partial class RM2RA_CreateNewRoomLobby: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public RoomConfigProto RoomConfig { get; set; }

	}

	[Message(InnerOpcode_Lobby.RA2RM_CreateNewRoomLobby)]
	[ProtoContract]
	public partial class RA2RM_CreateNewRoomLobby: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public RoomInfoProto RoomInfo { get; set; }

	}

//房间管理场景向RoomAgent通知关闭房间场景
	[Message(InnerOpcode_Lobby.RM2RA_RemoveRoom)]
	[ProtoContract]
	public partial class RM2RA_RemoveRoom: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int CloseCode { get; set; }

	}

//同步房间信息
	[Message(InnerOpcode_Lobby.RA2RM_UpdateRoomInfo)]
	[ProtoContract]
	public partial class RA2RM_UpdateRoomInfo: Object, IActorMessage
	{
		[ProtoMember(1)]
		public RoomInfoProto RoomInfo { get; set; }

	}

//房间关闭
	[Message(InnerOpcode_Lobby.Room2G_RoomClose)]
	[ProtoContract]
	public partial class Room2G_RoomClose: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int CloseCode { get; set; }

	}

//session断开通知房间
	[Message(InnerOpcode_Lobby.G2Room_SessionDisconnect)]
	[ProtoContract]
	public partial class G2Room_SessionDisconnect: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long sessionId { get; set; }

	}

}
