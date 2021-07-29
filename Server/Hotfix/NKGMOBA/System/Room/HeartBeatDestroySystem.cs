using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class HeartBeatComponentDestroySystem : DestroySystem<HeartBeatComponent>
    {
        public override void Destroy(HeartBeatComponent self)
        {

                try
                {
                    long unitId = self.Id;
                    ActorLocationSender actorLocationSender = Game.Scene.GetComponent<ActorLocationSenderComponent>().Get(unitId);
                    actorLocationSender.Send(new G2M_SessionDisconnect());
                }
                catch
                {

                }
        }
    }
}
