namespace ET
{
    public class L2C_AllowToEnterMapHandler : AMHandler<L2C_AllowToEnterMap>
    {
        protected override async ETVoid Run(Session session, L2C_AllowToEnterMap message)
        {
            //TODO 单独的加载UI处理
            FUI_LoadingComponent.HideLoadingUI();
            PlayerComponent playerComponent = Game.Scene.GetComponent<PlayerComponent>();
            playerComponent.HasCompletedLoadCount = 0;
            
            Game.EventSystem.Publish(new EventType.EnterMapFinish() {ZoneScene = session.DomainScene()}).Coroutine();

            await ETTask.CompletedTask;
        }
    }
}