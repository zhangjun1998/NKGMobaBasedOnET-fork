namespace ET
{
    public class RoomStateOnGateComponentAwakeSystem : AwakeSystem<RoomStateOnGateComponent, long,long>
    {
        public override void Awake(RoomStateOnGateComponent self, long actorid,long roomsceneid)
        {
            self.Awake();
            self.NeedSendMessage = true;
            self.RoomPlayerActorId = actorid;
            self.TargetRoomSceneId = roomsceneid;
            BroadcastMsgOnGateComponent.Instance.AddSessionId(self.TargetRoomSceneId, self.Parent.InstanceId);
        }
    }
    public class RoomStateOnGateComponentDestroySystem : DestroySystem<RoomStateOnGateComponent>
    {
        public override void Destroy(RoomStateOnGateComponent self)
        {
            if (self.NeedSendMessage)
            {
                MessageHelper.SendActor(self.RoomPlayerActorId, new G2Room_SessionDisconnect() { sessionId=self.Parent.InstanceId});
            }
            BroadcastMsgOnGateComponent.Instance.RemoveSessionId(self.TargetRoomSceneId, self.Parent.InstanceId);
            self.RoomPlayerActorId = 0;
            self.TargetRoomSceneId = 0;
            self.NeedSendMessage = false;
        }
    }
}