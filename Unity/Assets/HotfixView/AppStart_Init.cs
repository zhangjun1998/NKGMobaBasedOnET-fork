namespace ET
{
    public class AppStart_Init : AEvent<EventType.AppStart>
    {
        protected override async ETTask Run(EventType.AppStart args)
        {
            Game.Scene.AddComponent<TimerComponent>();
            Game.Scene.AddComponent<CoroutineLockComponent>();

            Game.Scene.AddComponent<ConfigComponent>();
            await ConfigComponent.Instance.LoadAsync();
            
            Game.Scene.AddComponent<OpcodeTypeComponent>();
            Game.Scene.AddComponent<MessageDispatcherComponent>();

            Game.Scene.AddComponent<NetThreadComponent>();

            Game.Scene.AddComponent<ZoneSceneManagerComponent>();
            Game.Scene.AddComponent<GlobalComponent>();

            Game.Scene.AddComponent<NumericWatcherComponent>();

            Game.Scene.AddComponent<GameObjectPoolComponent>();

            Game.Scene.AddComponent<SoundComponent>();

            Game.Scene.AddComponent<CameraComponent>();

            Game.Scene.AddComponent<PlayerComponent>();

            Game.Scene.AddComponent<UserInputComponent>();

            Scene zoneScene = await SceneFactory.CreateZoneScene(1, "Game", Game.Scene);
            
            LSF_PathFindCmd p = ReferencePool.Acquire<LSF_PathFindCmd>().Init(1) as LSF_PathFindCmd;
            var arr1 = ProtobufHelper.ToBytes(p);
            var s1 = ProtobufHelper.FromBytes<LSF_PathFindCmd>(arr1, 0, arr1.Length);
            var s2 = ProtobufHelper.FromBytes<ALSF_Cmd>(arr1, 0, arr1.Length);

            //显示登陆界面
            await Game.EventSystem.Publish(new EventType.AppStartInitFinish() {ZoneScene = zoneScene});
        }
    }
}