namespace ET
{
	public static partial class InnerOpcode
	{
		 public const ushort M2M_TrasferUnitRequest = 10001;
		 public const ushort M2M_TrasferUnitResponse = 10002;
		 public const ushort M2A_Reload = 10003;
		 public const ushort A2M_Reload = 10004;
		 public const ushort G2G_LockRequest = 10005;
		 public const ushort G2G_LockResponse = 10006;
		 public const ushort G2G_LockReleaseRequest = 10007;
		 public const ushort G2G_LockReleaseResponse = 10008;
		 public const ushort ObjectAddRequest = 10009;
		 public const ushort ObjectAddResponse = 10010;
		 public const ushort ObjectLockRequest = 10011;
		 public const ushort ObjectLockResponse = 10012;
		 public const ushort ObjectUnLockRequest = 10013;
		 public const ushort ObjectUnLockResponse = 10014;
		 public const ushort ObjectRemoveRequest = 10015;
		 public const ushort ObjectRemoveResponse = 10016;
		 public const ushort ObjectGetRequest = 10017;
		 public const ushort ObjectGetResponse = 10018;
		 public const ushort R2G_GetLoginKey = 10019;
		 public const ushort G2R_GetLoginKey = 10020;
		 public const ushort G2L_GetRoomId = 10021;
		 public const ushort L2G_GetRoomId = 10022;
		 public const ushort L2M_CreateHeroUnit = 10023;
		 public const ushort M2L_CreateHeroUnit = 10024;
		 public const ushort G2M_SessionDisconnect = 10025;
		 public const ushort L2M_PreparedToEnterBattle = 10026;
		 public const ushort M2L_PreparedToEnterBattle = 10027;
	}
}
