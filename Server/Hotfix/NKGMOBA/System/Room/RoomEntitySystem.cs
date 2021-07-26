using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class RoomEntityDestroySystem : DestroySystem<RoomEntity>
    {
        public override void Destroy(RoomEntity self)
        {
            //没开战说明是战前解散的.需要给所有房间内成员发解散消息
            if (self.GetComponent<BattleEntity>() == null)
            {
                MessageHelper.Broadcast(self.GetComponent<RoomPlayerComponent>().PlayerArray, new RM2C_LeaveRoom() { LeaveReason = (int)RoomPlayerQuitTypeEnum.RoomDismiss });
            }
            Session mgrSession = Game.Scene.GetComponent<NetInnerComponent>().Get(StartConfigComponent.Instance.RoomManagerConfig.GetComponent<InnerConfig>().IPEndPoint);
            mgrSession.Send(new UnRegisterRoomToRoomManager() { Roomid = self.InstanceId });
        }
    }

    [ObjectSystem]
    public class RoomEntityAwakeSystem : AwakeSystem<RoomEntity, string>
    {
        public override void Awake(RoomEntity self, string masterName)
        {
            self.AddComponent<RoomPlayerComponent>();
            var conf = self.AddComponent<RoomConfigComponent>();
            conf.RoomName = $"{masterName}的房间";
            conf.MaxMemberCount = 10;
        }
    }
    [ObjectSystem]
    public class RoomEntityStartSystem : StartSystem<RoomEntity>
    {
        public override void Start(RoomEntity self)
        {
            Session mgrSession = Game.Scene.GetComponent<NetInnerComponent>().Get(StartConfigComponent.Instance.RoomManagerConfig.GetComponent<InnerConfig>().IPEndPoint);
            mgrSession.Send(new UpdateRoomToRoomManager() {  BriefInfo = self.BriefInfo });
        }
    }
}
