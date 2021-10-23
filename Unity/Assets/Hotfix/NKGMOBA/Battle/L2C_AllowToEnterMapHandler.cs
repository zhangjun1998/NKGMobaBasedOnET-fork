namespace ET
{
    public class L2C_AllowToEnterMapHandler : AMHandler<L2C_AllowToEnterMap>
    {
        protected override async ETVoid Run(Session session, L2C_AllowToEnterMap message)
        {
            //TODO 单独的加载UI处理
            FUI_LoadingComponent.HideLoadingUI();
            PlayerComponent playerComponent = Game.Scene.GetComponent<PlayerComponent>();
            playerComponent.BelongToRoom = null;
            playerComponent.HasCompletedLoadCount = 0;

            await Game.EventSystem.Publish(new EventType.PrepareEnterMap() {ZoneScene = session.DomainScene()});
            Game.EventSystem.Publish(new EventType.FinishEnterMap() {ZoneScene = session.DomainScene()}).Coroutine();

            await ETTask.CompletedTask;
        }
    }
}