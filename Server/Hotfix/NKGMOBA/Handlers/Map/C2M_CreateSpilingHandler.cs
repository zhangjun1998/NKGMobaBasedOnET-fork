using UnityEngine;

namespace ET
{
    public class C2M_CreateSpilingHandler : AMActorHandler<Player, C2M_CreateSpiling>
    {
        protected override async ETTask Run(Player entity, C2M_CreateSpiling message)
        {
            Unit spiling = UnitFactory.CreateHeroSpilingUnit(entity.Domain.GetComponent<UnitComponent>(), 10001, RoleCamp.HuiYue,
                new Vector3(message.X, message.Y, message.Z), Quaternion.identity);

            MessageHelper.BroadcastToRoom(entity,
                new M2C_CreateSpilings()
                {
                    Unit = new UnitInfo()
                    {
                        ConfigId = 10001, X = message.X, Y = message.Y, Z = message.Z,
                        RoleCamp = (int) RoleCamp.HuiYue, UnitId = spiling.Id
                    }
                });
            await ETTask.CompletedTask;
        }
    }
}