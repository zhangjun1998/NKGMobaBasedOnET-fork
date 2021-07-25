//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月21日 16:03:23
//------------------------------------------------------------

using ETHotfix.NKGMOBA.Factory;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [ActorMessageHandler(AppType.Map)]
    public class Actor_CreateSpilingHandler: AMActorLocationHandler<Unit, Actor_CreateSpiling>
    {
        protected override ETTask Run(Unit entity, Actor_CreateSpiling message)
        {
            Unit unit = UnitFactory.CreateSpiling(entity);
            //设置木桩位置
            unit.Position = new Vector3(message.X, 0, message.Z);
            // 广播创建的木桩unit
            M2C_CreateSpilings createSpilings = new M2C_CreateSpilings();

            SpilingInfo spilingInfo = new SpilingInfo();
            spilingInfo.X = unit.Position.x;
            spilingInfo.Y = unit.Position.y;
            spilingInfo.Z = unit.Position.z;
            spilingInfo.UnitId = unit.Id;
            spilingInfo.ParentUnitId = message.ParentUnitId;
            spilingInfo.RoleCamp = (int) unit.GetComponent<B2S_RoleCastComponent>().RoleCamp;
            createSpilings.Spilings = spilingInfo;

            //向所有小骷髅广播信息
            MessageHelper.Broadcast(entity.RoomPlayerArray,createSpilings);
            return ETTask.CompletedTask;
        }
    }
}