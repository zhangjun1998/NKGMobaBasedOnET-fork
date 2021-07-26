namespace ETModel
{
    /// <summary>
    /// 房间的实体.
    /// </summary>
    public class RoomEntity : Entity
    {
        /// <summary>
        /// 只有在非战斗状态的房间才可以修改玩家状态
        /// </summary>
        public bool CanUnitChangeState =>
            GetComponent<BattleLoadingComponent>() != null &&
            GetComponent<BattleEntity>() != null;

        public RoomBriefInfo BriefInfo =>
            new RoomBriefInfo() {
                CurrentMemberCount = GetComponent<RoomPlayerComponent>().Players.Count,
                IsInBattle = GetComponent<BattleLoadingComponent>() == null,
                RoomId = InstanceId,
                RoomName = GetComponent<RoomConfigComponent>().RoomName,
                MaxMemberCount = GetComponent<RoomConfigComponent>().MaxMemberCount,
            };

        public RoomInfo Roominfo =>
            new RoomInfo { 
            MaxMemberCount= GetComponent<RoomConfigComponent>().MaxMemberCount,
            RoomId=InstanceId,
            RoomName= GetComponent<RoomConfigComponent>().RoomName,
            RoomPlayers= GetComponent<RoomPlayerComponent>().RoomPlayers()
            };
        public void Awake()
        {

        }

    }
}
