using ETModel;

namespace ETHotfix
{
    public static class MessageHelper
    {
        public static void Broadcast(IActorMessage message)
        {
            Unit[] units = UnitComponent.Instance.GetAll();
            ActorMessageSenderComponent actorLocationSenderComponent = Game.Scene.GetComponent<ActorMessageSenderComponent>();
            foreach (Unit unit in units)
            {
                UnitGateComponent unitGateComponent = unit.GetComponent<UnitGateComponent>();
                if (unitGateComponent == null || unitGateComponent.IsDisconnect)
                {
                    continue;
                }

                ActorMessageSender actorMessageSender = actorLocationSenderComponent.Get(unitGateComponent.GateSessionActorId);
                actorMessageSender.Send(message);
            }
        }
        /// <summary>
        /// 范围广播 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        public static void Broadcast(Unit[] units, IActorMessage message)
        {
            ActorMessageSenderComponent actorLocationSenderComponent = Game.Scene.GetComponent<ActorMessageSenderComponent>();
            foreach (Unit unit in units)
            {
                UnitGateComponent unitGateComponent = unit.GetComponent<UnitGateComponent>();
                if (unitGateComponent == null || unitGateComponent.IsDisconnect)
                {
                    continue;
                }

                ActorMessageSender actorMessageSender = actorLocationSenderComponent.Get(unitGateComponent.GateSessionActorId);
                actorMessageSender.Send(message);
            }
        }
    }
}