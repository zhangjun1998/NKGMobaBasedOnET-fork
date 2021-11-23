using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
	[ResponseType(typeof(M2C_TestResponse))]
	[Message(OuterOpcode_Map.C2M_TestRequest)]
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

	[Message(OuterOpcode_Map.M2C_TestResponse)]
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

	[ResponseType(typeof(Actor_TransferResponse))]
	[Message(OuterOpcode_Map.Actor_TransferRequest)]
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

	[Message(OuterOpcode_Map.Actor_TransferResponse)]
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

	[ResponseType(typeof(M2C_Reload))]
	[Message(OuterOpcode_Map.C2M_Reload)]
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

	[Message(OuterOpcode_Map.M2C_Reload)]
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

	[ResponseType(typeof(M2C_TestRobotCase))]
	[Message(OuterOpcode_Map.C2M_TestRobotCase)]
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

	[Message(OuterOpcode_Map.M2C_TestRobotCase)]
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

	[Message(OuterOpcode_Map.M2C_EndBattleSettle)]
	[ProtoContract]
	public partial class M2C_EndBattleSettle: Object, IActorMessage
	{
		[ProtoMember(1)]
		public List<PlayerBattlePoint> settleAccount = new List<PlayerBattlePoint>();

	}

	[Message(OuterOpcode_Map.M2C_KillEvent)]
	[ProtoContract]
	public partial class M2C_KillEvent: Object, IActorMessage
	{
		[ProtoMember(1)]
		public PlayerBattlePoint killer { get; set; }

		[ProtoMember(2)]
		public PlayerBattlePoint deadPlayer { get; set; }

	}

	[Message(OuterOpcode_Map.UnitInfo)]
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

	[Message(OuterOpcode_Map.M2C_CreateUnits)]
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

	[Message(OuterOpcode_Map.M2C_UnitDestoryed)]
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

	[Message(OuterOpcode_Map.C2M_PathfindingResult)]
	[ProtoContract]
	public partial class C2M_PathfindingResult: Object, IRoomMessage
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

	[Message(OuterOpcode_Map.C2M_Stop)]
	[ProtoContract]
	public partial class C2M_Stop: Object, IRoomMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode_Map.M2C_PathfindingResult)]
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

	[Message(OuterOpcode_Map.M2C_Stop)]
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

	[Message(OuterOpcode_Map.M2C_ReceiveDamage)]
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

	[Message(OuterOpcode_Map.M2C_ChangeProperty)]
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

	[Message(OuterOpcode_Map.C2M_CastHeroSkill)]
	[ProtoContract]
	public partial class C2M_CastHeroSkill: Object, IRoomMessage
	{
	}

	[Message(OuterOpcode_Map.M2C_CastHeroSkill)]
	[ProtoContract]
	public partial class M2C_CastHeroSkill: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long UnitId { get; set; }

	}

	[Message(OuterOpcode_Map.M2C_RecoverHP)]
	[ProtoContract]
	public partial class M2C_RecoverHP: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long SpriteUnitId { get; set; }

		[ProtoMember(2)]
		public float RecoverHPValue { get; set; }

	}

//请求攻击
	[Message(OuterOpcode_Map.C2M_CommonAttack)]
	[ProtoContract]
	public partial class C2M_CommonAttack: Object, IRoomMessage
	{
		[ProtoMember(1)]
		public long TargetUnitId { get; set; }

	}

//请求攻击
	[Message(OuterOpcode_Map.M2C_CancelCommonAttack)]
	[ProtoContract]
	public partial class M2C_CancelCommonAttack: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long TargetUnitId { get; set; }

	}

//服务器返回攻击指令，开始播放动画
	[Message(OuterOpcode_Map.M2C_CommonAttack)]
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

	[Message(OuterOpcode_Map.M2C_BuffInfo)]
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

	[Message(OuterOpcode_Map.C2M_UserInputSkillCmd)]
	[ProtoContract]
	public partial class C2M_UserInputSkillCmd: Object, IRoomMessage
	{
		[ProtoMember(64)]
		public string VK { get; set; }

	}

//同步行为树bool黑板变量
	[Message(OuterOpcode_Map.M2C_SyncNPBehaveBoolData)]
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
	[Message(OuterOpcode_Map.M2C_SyncCDData)]
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

	[Message(OuterOpcode_Map.C2M_CreateSpiling)]
	[ProtoContract]
	public partial class C2M_CreateSpiling: Object, IRoomMessage
	{
		[ProtoMember(2)]
		public float X { get; set; }

		[ProtoMember(3)]
		public float Y { get; set; }

		[ProtoMember(4)]
		public float Z { get; set; }

	}

	[Message(OuterOpcode_Map.M2C_CreateSpilings)]
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

	[Message(OuterOpcode_Map.M2C_SyncUnitAttribute)]
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

	[Message(OuterOpcode_Map.M2C_ChangeUnitAttribute)]
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
