using UnityEngine;

namespace ET
{
    public class C2M_CreateSpilingHandler : AMActorLocationHandler<Unit, C2M_CreateSpiling>
    {
        protected override async ETTask Run(Unit entity, C2M_CreateSpiling message)
        {
            Unit spiling = UnitFactory.CreateHeroSpilingUnit(entity.BelongToRoom, 10001, RoleCamp.TianZai,
                new Vector3(message.X, message.Y, message.Z), Quaternion.identity);

            MessageHelper.BroadcastToRoom(entity.BelongToRoom,
                new M2C_CreateSpilings()
                {
                    RoomId = entity.BelongToRoom.Id,
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