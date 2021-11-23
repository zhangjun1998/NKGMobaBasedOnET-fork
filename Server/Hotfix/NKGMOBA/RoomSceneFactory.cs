namespace ET
{
    public static class RoomSceneFactory
    {
        public static async ETTask<Scene> Create(Entity parent, RoomConfigProto RoomConfig)
        {
            await ETTask.CompletedTask;
            long instanceId = IdGenerater.Instance.GenerateInstanceId();
            Scene scene = EntitySceneFactory.CreateScene(instanceId, parent.DomainZone(), SceneType.Room, "", parent);
            scene.AddComponent<Room, RoomConfigProto>(RoomConfig);
            scene.AddComponent<BroadcastMsgComponent>();
            var playercomponent = scene.AddComponent<PlayerComponent>();
            scene.AddComponent<MailBoxComponent, MailboxType>(MailboxType.UnOrderMessageDispatcher);
            return scene;
        }

    }
}