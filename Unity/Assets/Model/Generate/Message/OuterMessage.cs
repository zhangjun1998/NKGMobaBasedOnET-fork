using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
	#if SERVER
	[ResponseType(typeof(M2C_TestResponse))]
	#else
	[ResponseType("M2C_TestResponse")]
	#endif
	[Message(OuterOpcode.C2M_TestRequest)]
	[ProtoContract]
	public partial class C2M_TestRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public string request { get; set; }

	}

	[Message(OuterOpcode.M2C_TestResponse)]
	[ProtoContract]
	public partial class M2C_TestResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string response { get; set; }

	}

	#if SERVER
	[ResponseType(typeof(Actor_TransferResponse))]
	#else
	[ResponseType("Actor_TransferResponse")]
	#endif
	[Message(OuterOpcode.Actor_TransferRequest)]
	[ProtoContract]
	public partial class Actor_TransferRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int MapIndex { get; set; }

	}

	[Message(OuterOpcode.Actor_TransferResponse)]
	[ProtoContract]
	public partial class Actor_TransferResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	#if SERVER
	[ResponseType(typeof(G2C_EnterMap))]
	#else
	[ResponseType("G2C_EnterMap")]
	#endif
	[Message(OuterOpcode.C2G_EnterMap)]
	[ProtoContract]
	public partial class C2G_EnterMap: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.G2C_EnterMap)]
	[ProtoContract]
	public partial class G2C_EnterMap: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

// 自己的unit id
// 自己的unit id
		[ProtoMember(1)]
		public long UnitId { get; set; }

// 所有的unit
// 所有的unit
		[ProtoMember(2)]
		public List<UnitInfo> Units = new List<UnitInfo>();

	}

	#if SERVER
	[ResponseType(typeof(G2C_Ping))]
	#else
	[ResponseType("G2C_Ping")]
	#endif
	[Message(OuterOpcode.C2G_Ping)]
	[ProtoContract]
	public partial class C2G_Ping: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.G2C_Ping)]
	[ProtoContract]
	public partial class G2C_Ping: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long Time { get; set; }

	}

	[Message(OuterOpcode.G2C_Test)]
	[ProtoContract]
	public partial class G2C_Test: Object, IMessage
	{
	}

	#if SERVER
	[ResponseType(typeof(M2C_Reload))]
	#else
	[ResponseType("M2C_Reload")]
	#endif
	[Message(OuterOpcode.C2M_Reload)]
	[ProtoContract]
	public partial class C2M_Reload: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

	}

	[Message(OuterOpcode.M2C_Reload)]
	[ProtoContract]
	public partial class M2C_Reload: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	#if SERVER
	[ResponseType(typeof(R2C_Login))]
	#else
	[ResponseType("R2C_Login")]
	#endif
	[Message(OuterOpcode.C2R_Login)]
	[ProtoContract]
	public partial class C2R_Login: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

	}

	[Message(OuterOpcode.R2C_Login)]
	[ProtoContract]
	public partial class R2C_Login: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string GateAddress { get; set; }

		[ProtoMember(2)]
		public long Key { get; set; }

		[ProtoMember(3)]
		public long GateId { get; set; }

	}

	#if SERVER
	[ResponseType(typeof(R2C_Registe))]
	#else
	[ResponseType("R2C_Registe")]
	#endif
	[Message(OuterOpcode.C2R_Registe)]
	[ProtoContract]
	public partial class C2R_Registe: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

	}

	[Message(OuterOpcode.R2C_Registe)]
	[ProtoContract]
	public partial class R2C_Registe: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	#if SERVER
	[ResponseType(typeof(G2C_LoginGate))]
	#else
	[ResponseType("G2C_LoginGate")]
	#endif
	[Message(OuterOpcode.C2G_LoginGate)]
	[ProtoContract]
	public partial class C2G_LoginGate: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Key { get; set; }

		[ProtoMember(2)]
		public long GateId { get; set; }

	}

	[Message(OuterOpcode.G2C_LoginGate)]
	[ProtoContract]
	public partial class G2C_LoginGate: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long PlayerId { get; set; }

		[ProtoMember(93)]
		public string LobbyAddress { get; set; }

	}

	#if SERVER
	[ResponseType(typeof(L2C_LoginLobby))]
	#else
	[ResponseType("L2C_LoginLobby")]
	#endif
	[Message(OuterOpcode.C2L_LoginLobby)]
	[ProtoContract]
	public partial class C2L_LoginLobby: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long PlayerId { get; set; }

	}

	[Message(OuterOpcode.L2C_LoginLobby)]
	[ProtoContract]
	public partial class L2C_LoginLobby: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(5)]
		public List<long> RoomHolderPlayerList = new List<long>();

		[ProtoMember(1)]
		public List<long> RoomIdList = new List<long>();

		[ProtoMember(2)]
		public List<string> RoomNameList = new List<string>();

		[ProtoMember(3)]
		public List<int> RoomPlayerNum = new List<int>();

	}

	[Message(OuterOpcode.G2C_TestHotfixMessage)]
	[ProtoContract]
	public partial class G2C_TestHotfixMessage: Object, IMessage
	{
		[ProtoMember(1)]
		public string Info { get; set; }

	}

	#if SERVER
	[ResponseType(typeof(M2C_TestRobotCase))]
	#else
	[ResponseType("M2C_TestRobotCase")]
	#endif
	[Message(OuterOpcode.C2M_TestRobotCase)]
	[ProtoContract]
	public partial class C2M_TestRobotCase: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int N { get; set; }

	}

	[Message(OuterOpcode.M2C_TestRobotCase)]
	[ProtoContract]
	public partial class M2C_TestRobotCase: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int N { get; set; }

	}

///////////////////////////////// 房间相关  START ///////////////////////////////////
	#if SERVER
	[ResponseType(typeof(L2C_CreateNewRoomLobby))]
	#else
	[ResponseType("L2C_CreateNewRoomLobby")]
	#endif
	[Message(OuterOpcode.C2L_CreateNewRoomLobby)]
	[ProtoContract]
	public partial class C2L_CreateNewRoomLobby: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long PlayerId { get; set; }

	}

	[Message(OuterOpcode.L2C_CreateNewRoomLobby)]
	[ProtoContract]
	public partial class L2C_CreateNewRoomLobby: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int RoomId { get; set; }

		[ProtoMember(2)]
		public int mode { get; set; }

		[ProtoMember(3)]
		public int camp { get; set; }

	}

	#if SERVER
	[ResponseType(typeof(L2C_JoinRoomLobby))]
	#else
	[ResponseType("L2C_JoinRoomLobby")]
	#endif
	[Message(OuterOpcode.C2L_JoinRoomLobby)]
	[ProtoContract]
	public partial class C2L_JoinRoomLobby: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long PlayerId { get; set; }

		[ProtoMember(2)]
		public long RoomId { get; set; }

	}

	[Message(OuterOpcode.L2C_JoinRoomLobby)]
	[ProtoContract]
	public partial class L2C_JoinRoomLobby: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long RoomId { get; set; }

		[ProtoMember(2)]
		public int camp { get; set; }

		[ProtoMember(3)]
		public long RoomHolderId { get; set; }

		[ProtoMember(4)]
		public string RoomName { get; set; }

		[ProtoMember(5)]
		public List<PlayerInfoRoom> playerInfoRoom = new List<PlayerInfoRoom>();

	}

	[Message(OuterOpcode.PlayerInfoRoom)]
	[ProtoContract]
	public partial class PlayerInfoRoom: Object
	{
		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public long UnitId { get; set; }

		[ProtoMember(3)]
		public long SessionId { get; set; }

		[ProtoMember(4)]
		public long RoomId { get; set; }

		[ProtoMember(5)]
		public int camp { get; set; }

		[ProtoMember(6)]
		public long playerid { get; set; }

	}

	[Message(OuterOpcode.L2C_PlayerTriggerRoom)]
	[ProtoContract]
	public partial class L2C_PlayerTriggerRoom: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public PlayerInfoRoom playerInfoRoom { get; set; }

		[ProtoMember(2)]
		public bool JoinOrLeave { get; set; }

	}

	#if SERVER
	[ResponseType(typeof(L2C_LeaveRoomLobby))]
	#else
	[ResponseType("L2C_LeaveRoomLobby")]
	#endif
	[Message(OuterOpcode.C2L_LeaveRoomLobby)]
	[ProtoContract]
	public partial class C2L_LeaveRoomLobby: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long PlayerId { get; set; }

	}

	[Message(OuterOpcode.L2C_LeaveRoomLobby)]
	[ProtoContract]
	public partial class L2C_LeaveRoomLobby: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int camp { get; set; }

		[ProtoMember(2)]
		public long newRoomHolder { get; set; }

		[ProtoMember(4)]
		public long RoomId { get; set; }

		[ProtoMember(5)]
		public long PlayerId { get; set; }

		[ProtoMember(3)]
		public bool isDestory { get; set; }

	}

	[Message(OuterOpcode.PlayerBattleInfo)]
	[ProtoContract]
	public partial class PlayerBattleInfo: Object
	{
		[ProtoMember(1)]
		public long PlayerId { get; set; }

		[ProtoMember(2)]
		public int HeroConfig { get; set; }

		[ProtoMember(3)]
		public List<int> SpriteConfigs = new List<int>();

	}

	#if SERVER
	[ResponseType(typeof(L2C_StartGameLobby))]
	#else
	[ResponseType("L2C_StartGameLobby")]
	#endif
	[Message(OuterOpcode.C2L_StartGameLobby)]
	[ProtoContract]
	public partial class C2L_StartGameLobby: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long PlayerId { get; set; }

		[ProtoMember(2)]
		public List<PlayerBattleInfo> PlayerBattleInfos = new List<PlayerBattleInfo>();

	}

	[Message(OuterOpcode.L2C_StartGameLobby)]
	[ProtoContract]
	public partial class L2C_StartGameLobby: Object, IResponse
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

	[Message(OuterOpcode.M2C_EndBattleSettle)]
	[ProtoContract]
	public partial class M2C_EndBattleSettle: Object, IActorMessage
	{
		[ProtoMember(1)]
		public List<PlayerBattlePoint> settleAccount = new List<PlayerBattlePoint>();

	}

	[Message(OuterOpcode.M2C_KillEvent)]
	[ProtoContract]
	public partial class M2C_KillEvent: Object, IActorMessage
	{
		[ProtoMember(1)]
		public PlayerBattlePoint killer { get; set; }

		[ProtoMember(2)]
		public PlayerBattlePoint deadPlayer { get; set; }

	}

	[Message(OuterOpcode.PlayerBattlePoint)]
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

///////////////////////////////// 房间相关  END ///////////////////////////////////
//////////////////////////////// 战斗相关 START ///////////////////////////////////////
	[Message(OuterOpcode.L2C_PrepareToEnterBattle)]
	[ProtoContract]
	public partial class L2C_PrepareToEnterBattle: Object, IMessage
	{
	}

	[Message(OuterOpcode.C2L_PreparedToEnterBattle)]
	[ProtoContract]
	public partial class C2L_PreparedToEnterBattle: Object, IMessage
	{
		[ProtoMember(1)]
		public long PlayerId { get; set; }

	}

	[Message(OuterOpcode.L2C_AllowToEnterMap)]
	[ProtoContract]
	public partial class L2C_AllowToEnterMap: Object, IMessage
	{
	}

	[Message(OuterOpcode.UnitInfo)]
	[ProtoContract]
	public partial class UnitInfo: Object
	{
		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public int ConfigId { get; set; }

// 所属的玩家id
// 所属的玩家id
		[ProtoMember(99)]
		public long BelongToPlayerId { get; set; }

		[ProtoMember(3)]
		public float X { get; set; }

		[ProtoMember(4)]
		public float Y { get; set; }

		[ProtoMember(5)]
		public float Z { get; set; }

		[ProtoMember(6)]
		public int RoleCamp { get; set; }

	}

	[Message(OuterOpcode.M2C_CreateUnits)]
	[ProtoContract]
	public partial class M2C_CreateUnits: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(94)]
		public long PlayerId { get; set; }

		[ProtoMember(95)]
		public long RoomId { get; set; }

		[ProtoMember(2)]
		public List<UnitInfo> Units = new List<UnitInfo>();

	}

	[Message(OuterOpcode.M2C_UnitDestoryed)]
	[ProtoContract]
	public partial class M2C_UnitDestoryed: Object, IActorMessage
	{
		[ProtoMember(93)]
		public long ActorId { get; set; }

//被破坏的UnitId
//被破坏的UnitId
		[ProtoMember(94)]
		public long DestoryedUnitId { get; set; }

	}

	[Message(OuterOpcode.C2M_PathfindingResult)]
	[ProtoContract]
	public partial class C2M_PathfindingResult: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public float X { get; set; }

		[ProtoMember(2)]
		public float Y { get; set; }

		[ProtoMember(3)]
		public float Z { get; set; }

	}

	[Message(OuterOpcode.C2M_Stop)]
	[ProtoContract]
	public partial class C2M_Stop: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.M2C_PathfindingResult)]
	[ProtoContract]
	public partial class M2C_PathfindingResult: Object, IActorMessage
	{
		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public float X { get; set; }

		[ProtoMember(3)]
		public float Y { get; set; }

		[ProtoMember(4)]
		public float Z { get; set; }

		[ProtoMember(6)]
		public float Speed { get; set; }

		[ProtoMember(7)]
		public List<float> Xs = new List<float>();

		[ProtoMember(8)]
		public List<float> Ys = new List<float>();

		[ProtoMember(9)]
		public List<float> Zs = new List<float>();

	}

	[Message(OuterOpcode.M2C_Stop)]
	[ProtoContract]
	public partial class M2C_Stop: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int Error { get; set; }

		[ProtoMember(2)]
		public long Id { get; set; }

		[ProtoMember(3)]
		public float X { get; set; }

		[ProtoMember(4)]
		public float Y { get; set; }

		[ProtoMember(5)]
		public float Z { get; set; }

		[ProtoMember(6)]
		public float A { get; set; }

		[ProtoMember(7)]
		public float B { get; set; }

		[ProtoMember(8)]
		public float C { get; set; }

		[ProtoMember(9)]
		public float W { get; set; }

	}

	[Message(OuterOpcode.M2C_ReceiveDamage)]
	[ProtoContract]
	public partial class M2C_ReceiveDamage: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int Error { get; set; }

		[ProtoMember(2)]
		public long UnitId { get; set; }

		[ProtoMember(4)]
		public float FinalValue { get; set; }

	}

	[Message(OuterOpcode.M2C_ChangeProperty)]
	[ProtoContract]
	public partial class M2C_ChangeProperty: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int Error { get; set; }

		[ProtoMember(2)]
		public long UnitId { get; set; }

		[ProtoMember(3)]
		public int NumicType { get; set; }

		[ProtoMember(4)]
		public float FinalValue { get; set; }

	}

	[Message(OuterOpcode.C2M_CastHeroSkill)]
	[ProtoContract]
	public partial class C2M_CastHeroSkill: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.M2C_CastHeroSkill)]
	[ProtoContract]
	public partial class M2C_CastHeroSkill: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long UnitId { get; set; }

	}

	[Message(OuterOpcode.M2C_RecoverHP)]
	[ProtoContract]
	public partial class M2C_RecoverHP: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long SpriteUnitId { get; set; }

		[ProtoMember(2)]
		public float RecoverHPValue { get; set; }

	}

//请求攻击
	[Message(OuterOpcode.C2M_CommonAttack)]
	[ProtoContract]
	public partial class C2M_CommonAttack: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long TargetUnitId { get; set; }

	}

//请求攻击
	[Message(OuterOpcode.M2C_CancelCommonAttack)]
	[ProtoContract]
	public partial class M2C_CancelCommonAttack: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long TargetUnitId { get; set; }

	}

//服务器返回攻击指令，开始播放动画
	[Message(OuterOpcode.M2C_CommonAttack)]
	[ProtoContract]
	public partial class M2C_CommonAttack: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(4)]
		public long AttackCasterId { get; set; }

		[ProtoMember(3)]
		public long TargetUnitId { get; set; }

		[ProtoMember(2)]
		public bool CanAttack { get; set; }

	}

	[Message(OuterOpcode.M2C_BuffInfo)]
	[ProtoContract]
	public partial class M2C_BuffInfo: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(96)]
		public long SkillId { get; set; }

		[ProtoMember(2)]
		public string BBKey { get; set; }

		[ProtoMember(95)]
		public long TheUnitBelongToId { get; set; }

		[ProtoMember(91)]
		public long TheUnitFromId { get; set; }

		[ProtoMember(3)]
		public int BuffLayers { get; set; }

		[ProtoMember(4)]
		public float BuffMaxLimitTime { get; set; }

	}

	[Message(OuterOpcode.C2M_UserInputSkillCmd)]
	[ProtoContract]
	public partial class C2M_UserInputSkillCmd: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(64)]
		public string VK { get; set; }

	}

//同步行为树bool黑板变量
	[Message(OuterOpcode.M2C_SyncNPBehaveBoolData)]
	[ProtoContract]
	public partial class M2C_SyncNPBehaveBoolData: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(94)]
		public long UnitId { get; set; }

//黑板键
//黑板键
		[ProtoMember(2)]
		public string BBKey { get; set; }

		[ProtoMember(5)]
		public bool Value { get; set; }

	}

//同步CD信息
	[Message(OuterOpcode.M2C_SyncCDData)]
	[ProtoContract]
	public partial class M2C_SyncCDData: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(94)]
		public long UnitId { get; set; }

//CD名称
//CD名称
		[ProtoMember(2)]
		public string CDName { get; set; }

//CD总时长
//CD总时长
		[ProtoMember(3)]
		public long CDLength { get; set; }

//剩余CD时长
//剩余CD时长
		[ProtoMember(5)]
		public long RemainCDLength { get; set; }

	}

	[Message(OuterOpcode.C2M_CreateSpiling)]
	[ProtoContract]
	public partial class C2M_CreateSpiling: Object, IActorLocationMessage
	{
		[ProtoMember(2)]
		public float X { get; set; }

		[ProtoMember(3)]
		public float Y { get; set; }

		[ProtoMember(4)]
		public float Z { get; set; }

//所归属的父实体id
//所归属的父实体id
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.M2C_CreateSpilings)]
	[ProtoContract]
	public partial class M2C_CreateSpilings: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(95)]
		public long RoomId { get; set; }

		[ProtoMember(2)]
		public UnitInfo Unit { get; set; }

	}

	[Message(OuterOpcode.M2C_SyncUnitAttribute)]
	[ProtoContract]
	public partial class M2C_SyncUnitAttribute: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(94)]
		public long UnitId { get; set; }

		[ProtoMember(95)]
		public int NumericType { get; set; }

		[ProtoMember(3)]
		public float FinalValue { get; set; }

	}

	[Message(OuterOpcode.M2C_ChangeUnitAttribute)]
	[ProtoContract]
	public partial class M2C_ChangeUnitAttribute: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(94)]
		public long UnitId { get; set; }

		[ProtoMember(95)]
		public int NumericType { get; set; }

		[ProtoMember(2)]
		public float ChangeValue { get; set; }

	}

////////////////////////////////////////////// 战斗相关 END ///////////////////////////////////////////////////////////////
}
