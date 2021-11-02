using UnityEngine;

namespace ET
{
    public class C2M_CreateSpilingHandler : AMActorLocationHandler<Unit, C2M_CreateSpiling>
    {
        protected override async ETTask Run(Unit entity, C2M_CreateSpiling message)
        {
            Unit spiling = UnitFactory.CreateHeroSpilingUnit(entity.GetParent<UnitComponent>(), 10001, RoleCamp.TianZai,
                new Vector3(message.X, message.Y, message.Z), Quaternion.identity);

            MessageHelper.BroadcastToRoom(entity,
                new M2C_CreateSpilings()
                {
                    Unit = new UnitInfo()
                    {
                        ConfigId = 10001, X = message.X, Y = message.Y, Z = message.Z,
                        RoleCamp = (int) RoleCamp.TianZai, UnitId = spiling.Id
                    }
                });
            await ETTask.CompletedTask;
        }
    }
}