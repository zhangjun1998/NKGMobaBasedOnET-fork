using ETModel;
namespace ETModel
{
	[Message(OuterOpcode.C2M_TestRequest)]
	public partial class C2M_TestRequest : IActorLocationRequest {}

	[Message(OuterOpcode.M2C_TestResponse)]
	public partial class M2C_TestResponse : IActorLocationResponse {}

	[Message(OuterOpcode.Actor_TransferRequest)]
	public partial class Actor_TransferRequest : IActorLocationRequest {}

	[Message(OuterOpcode.Actor_TransferResponse)]
	public partial class Actor_TransferResponse : IActorLocationResponse {}

	[Message(OuterOpcode.C2G_EnterMap)]
	public partial class C2G_EnterMap : IRequest {}

	[Message(OuterOpcode.G2C_EnterMap)]
	public partial class G2C_EnterMap : IResponse {}

// 自己的unit id
// 所有的unit
	[Message(OuterOpcode.UnitInfo)]
	public partial class UnitInfo {}

	[Message(OuterOpcode.M2C_CreateUnits)]
	public partial class M2C_CreateUnits : IActorMessage {}

	[Message(OuterOpcode.SpilingInfo)]
	public partial class SpilingInfo {}

//创建木桩
	[Message(OuterOpcode.M2C_CreateSpilings)]
	public partial class M2C_CreateSpilings : IActorMessage {}

	[Message(OuterOpcode.Frame_ClickMap)]
	public partial class Frame_ClickMap : IActorLocationMessage {}

	[Message(OuterOpcode.M2C_PathfindingResult)]
	public partial class M2C_PathfindingResult : IActorMessage {}

	[Message(OuterOpcode.C2R_Ping)]
	public partial class C2R_Ping : IRequest {}

	[Message(OuterOpcode.R2C_Ping)]
	public partial class R2C_Ping : IResponse {}

	[Message(OuterOpcode.G2C_Test)]
	public partial class G2C_Test : IMessage {}

	[Message(OuterOpcode.C2M_Reload)]
	public partial class C2M_Reload : IRequest {}

	[Message(OuterOpcode.M2C_Reload)]
	public partial class M2C_Reload : IResponse {}

	[Message(OuterOpcode.C2G_HeartBeat)]
	public partial class C2G_HeartBeat : IRequest {}

	[Message(OuterOpcode.G2C_HeartBeat)]
	public partial class G2C_HeartBeat : IResponse {}

	[Message(OuterOpcode.M2C_BuffInfo)]
	public partial class M2C_BuffInfo : IActorMessage {}

//请求攻击
	[Message(OuterOpcode.C2M_CommonAttack)]
	public partial class C2M_CommonAttack : IActorLocationMessage {}

//服务器返回攻击指令，开始播放动画
	[Message(OuterOpcode.M2C_CommonAttack)]
	public partial class M2C_CommonAttack : IActorMessage {}

//房间简略信息.用于给客户端列表展示.
	[Message(OuterOpcode.RoomBriefInfo)]
	public partial class RoomBriefInfo {}

//房间内玩家
	[Message(OuterOpcode.RoomPlayer)]
	public partial class RoomPlayer {}

//房间完整信息.用于客户端构建房间界面.
	[Message(OuterOpcode.RoomInfo)]
	public partial class RoomInfo {}

	[Message(OuterOpcode.C2G_AllRoomList)]
	public partial class C2G_AllRoomList : IRequest {}

	[Message(OuterOpcode.G2C_AllRoomList)]
	public partial class G2C_AllRoomList : IResponse {}

//创建房间
	[Message(OuterOpcode.C2RM_CreateRoom)]
	public partial class C2RM_CreateRoom : IRequest {}

	[Message(OuterOpcode.RM2C_CreateRoom)]
	public partial class RM2C_CreateRoom : IResponse {}

// 加入指定房间
	[Message(OuterOpcode.C2RM_JoinRoom)]
	public partial class C2RM_JoinRoom : IRequest {}

	[Message(OuterOpcode.RM2C_JoinRoom)]
	public partial class RM2C_JoinRoom : IResponse {}

// 房主踢人
	[Message(OuterOpcode.C2RM_KickRoomPlayer)]
	public partial class C2RM_KickRoomPlayer : IActorLocationRequest {}

	[Message(OuterOpcode.RM2C_KickRoomPlayer)]
	public partial class RM2C_KickRoomPlayer : IActorLocationResponse {}

// 退出房间
	[Message(OuterOpcode.C2RM_QuitRoom)]
	public partial class C2RM_QuitRoom : IActorLocationRequest {}

	[Message(OuterOpcode.RM2C_QuitRoom)]
	public partial class RM2C_QuitRoom : IActorLocationResponse {}

//服务器通知客户端 退出当前房间,附带原因
	[Message(OuterOpcode.RM2C_LeaveRoom)]
	public partial class RM2C_LeaveRoom : IActorMessage {}

//服务器通知客户端 房间状态变更 (收到消息要进入房间界面)
	[Message(OuterOpcode.RM2C_RoomInfoUpdate)]
	public partial class RM2C_RoomInfoUpdate : IActorMessage {}

// 房主请求开始战斗
	[Message(OuterOpcode.C2RM_RequestStartBattle)]
	public partial class C2RM_RequestStartBattle : IActorLocationRequest {}

	[Message(OuterOpcode.RM2C_RequestStartBattle)]
	public partial class RM2C_RequestStartBattle : IActorLocationResponse {}

//客户端收到这条消息开始加载战斗相关资源
	[Message(OuterOpcode.RM2C_EnterBattleMessage)]
	public partial class RM2C_EnterBattleMessage : IActorMessage {}

//客户端加载完毕,通知服务端可以开始战斗
	[Message(OuterOpcode.C2RM_LoadComplete)]
	public partial class C2RM_LoadComplete : IActorLocationMessage {}

//所有客户端均加载完毕,通知战斗正式开始(有人超时直接跳过).
	[Message(OuterOpcode.RM2C_StartBattleMessage)]
	public partial class RM2C_StartBattleMessage : IActorMessage {}

// 战斗重连
	[Message(OuterOpcode.C2RM_ReconnetBattle)]
	public partial class C2RM_ReconnetBattle : IMessage {}

}
namespace ETModel
{
	public static partial class OuterOpcode
	{
		 public const ushort C2M_TestRequest = 101;
		 public const ushort M2C_TestResponse = 102;
		 public const ushort Actor_TransferRequest = 103;
		 public const ushort Actor_TransferResponse = 104;
		 public const ushort C2G_EnterMap = 105;
		 public const ushort G2C_EnterMap = 106;
		 public const ushort UnitInfo = 107;
		 public const ushort M2C_CreateUnits = 108;
		 public const ushort SpilingInfo = 109;
		 public const ushort M2C_CreateSpilings = 110;
		 public const ushort Frame_ClickMap = 111;
		 public const ushort M2C_PathfindingResult = 112;
		 public const ushort C2R_Ping = 113;
		 public const ushort R2C_Ping = 114;
		 public const ushort G2C_Test = 115;
		 public const ushort C2M_Reload = 116;
		 public const ushort M2C_Reload = 117;
		 public const ushort C2G_HeartBeat = 118;
		 public const ushort G2C_HeartBeat = 119;
		 public const ushort M2C_BuffInfo = 120;
		 public const ushort C2M_CommonAttack = 121;
		 public const ushort M2C_CommonAttack = 122;
		 public const ushort RoomBriefInfo = 123;
		 public const ushort RoomPlayer = 124;
		 public const ushort RoomInfo = 125;
		 public const ushort C2G_AllRoomList = 126;
		 public const ushort G2C_AllRoomList = 127;
		 public const ushort C2RM_CreateRoom = 128;
		 public const ushort RM2C_CreateRoom = 129;
		 public const ushort C2RM_JoinRoom = 130;
		 public const ushort RM2C_JoinRoom = 131;
		 public const ushort C2RM_KickRoomPlayer = 132;
		 public const ushort RM2C_KickRoomPlayer = 133;
		 public const ushort C2RM_QuitRoom = 134;
		 public const ushort RM2C_QuitRoom = 135;
		 public const ushort RM2C_LeaveRoom = 136;
		 public const ushort RM2C_RoomInfoUpdate = 137;
		 public const ushort C2RM_RequestStartBattle = 138;
		 public const ushort RM2C_RequestStartBattle = 139;
		 public const ushort RM2C_EnterBattleMessage = 140;
		 public const ushort C2RM_LoadComplete = 141;
		 public const ushort RM2C_StartBattleMessage = 142;
		 public const ushort C2RM_ReconnetBattle = 143;
	}
}
