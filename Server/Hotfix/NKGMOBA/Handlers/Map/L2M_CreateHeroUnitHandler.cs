using System;
using System.Collections.Generic;
using System.Numerics;
using ET.EventType;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace ET
{
    [ActorMessageHandler]
    public class L2M_CreateHeroUnitHandler : AMActorRpcHandler<Scene, L2M_CreateHeroUnit, M2L_CreateHeroUnit>
    {
        protected override async ETTask Run(Scene scene, L2M_CreateHeroUnit request, M2L_CreateHeroUnit response,
            Action reply)
        {
            //单个玩家局内创建，由Gate服发送请求
            Room targetRoom = scene.GetComponent<RoomManagerComponent>().GetRoom(request.Roomid);

            if (targetRoom == null)
            {
                targetRoom = scene.GetComponent<RoomManagerComponent>().CreateBattleRoom(request.Roomid); //局内房间
            }

            targetRoom.ContainsPlayers.Add(request.PlayerId,
                Game.Scene.GetComponent<PlayerComponent>().Get(request.PlayerId));

            Vector3 unitPos = Vector3.zero;

            Player player = Game.Scene.GetComponent<PlayerComponent>().Get(request.PlayerId);
            RoleCamp heroCamp = RoleCamp.HuiYue;
            if (player.camp % 2 == 0)
            {
                heroCamp = RoleCamp.TianZai;
                unitPos = new Vector3(-15, 0, 0);
            }

            // TODO 测试代码，应当由玩家在房间中的位置分配阵营
            Unit unit = UnitFactory.CreateHeroUnit(targetRoom, 10001, heroCamp,
                new Vector3(-10, 0, -10),
                Quaternion.identity);
            unit.BelongToPlayer = player;
            // 必要，这是Actor基石，只需要添加到英雄身上即可，因为我们可以通过英雄可以索引到英雄所拥有的一切
            await unit.AddLocation();
            unit.AddComponent<UnitGateComponent, long>(request.GateSessionId);

            response.UnitId = unit.Id;

            // 把自己广播给周围的人
            M2C_CreateUnits createUnits = new M2C_CreateUnits() {RoomId = targetRoom.Id};
            createUnits.Units.Add(UnitHelper.CreateUnitInfo(unit));
            MessageHelper.BroadcastToRoom(unit.BelongToRoom, createUnits);

            // 把周围的人通知给自己
            createUnits.Units.Clear();
            List<Unit> units = targetRoom.GetComponent<UnitComponent>().GetAllUnitToSyncToClient();
            foreach (Unit u in units)
            {
                createUnits.Units.Add(UnitHelper.CreateUnitInfo(u));
            }

            MessageHelper.SendActor(unit.GetComponent<UnitGateComponent>().GateSessionActorId, createUnits);
            reply();
        }
    }
}