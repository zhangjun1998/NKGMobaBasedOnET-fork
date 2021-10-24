namespace ET
{
    public class M2C_CreateSpilingsHandler: AMHandler<M2C_CreateSpilings>
    {
        protected override async ETVoid Run(Session session, M2C_CreateSpilings message)
        {
            UnitFactory.CreateHeroSpiling(session.DomainScene().GetComponent<RoomManagerComponent>().GetBattleRoom(), message.Unit);

            await ETTask.CompletedTask;
        }
    }
}