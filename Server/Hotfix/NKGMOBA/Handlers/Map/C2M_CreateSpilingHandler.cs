using UnityEngine;

namespace ET
{
    public class C2M_CreateSpilingHandler : AMActorHandler<Player, C2M_CreateSpiling>
    {
        protected override async ETTask Run(Player entity, C2M_CreateSpiling message)
        {
            RoleCamp roleCamp= RoleCamp.TianZai;
            if (entity.camp%2==0)
            {
                roleCamp = RoleCamp.HuiYue;
            }
            Unit spiling = UnitFactory.CreateHeroSpilingUnit(entity.DomainScene(), 10001, roleCamp,
                new Vector3(message.X, message.Y, message.Z), Quaternion.identity);

            MessageHelper.BroadcastToRoom(entity,
                new M2C_CreateSpilings()
                {
                    Unit = new UnitInfo()
                    {
                        ConfigId = 10001, X = message.X, Y = message.Y, Z = message.Z,
                        RoleCamp = (int)roleCamp, UnitId = spiling.Id
                    }
                });
            await ETTask.CompletedTask;
        }
    }
}