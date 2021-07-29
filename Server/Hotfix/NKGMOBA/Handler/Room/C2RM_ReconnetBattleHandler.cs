
using ETModel;
using System;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2RM_ReconnetBattleHandler : AMHandler<C2RM_ReconnetBattle>
    {
        protected override async ETTask Run(Session session, C2RM_ReconnetBattle message)
        {
            Player player = session.GetComponent<SessionPlayerComponent>().Player;
            ActorLocationSender actorLocationSender = Game.Scene.GetComponent<ActorLocationSenderComponent>().Get(player.PlayerIdInDB);
            actorLocationSender.Send(new G2RM_ReconnectBattle() { GateSessionId = session.InstanceId });
        }
    }


    [ActorMessageHandler(AppType.Room)]
    public class G2RM_ReconnectBattleHandler : AMActorLocationHandler<Unit, G2RM_ReconnectBattle>
    {
        protected override async ETTask Run(Unit unit, G2RM_ReconnectBattle message)
        {
            unit.GetComponent<UnitGateComponent>().GateSessionActorId = message.GateSessionId;
            var units = unit.GetParent<RoomPlayerComponent>().PlayerArray;
            var msg = new RM2C_EnterBattleMessage();
            foreach (Unit oneunit in units)
            {
                msg.Units.Add(oneunit.UnitToUnitInfo());
                foreach (var childUnit in oneunit.GetComponent<ChildrenUnitComponent>().ChildrenUnit)
                {
                    msg.Units.Add(childUnit.UnitToUnitInfo());
                }
            }
            MessageHelper.SendMsgToUnit(unit, msg);

        }
    }
}
