using System;
using ETModel;
using ETModel.NKGMOBA.Battle.State;
using Vector3 = UnityEngine.Vector3;

namespace ETHotfix
{
    [MessageHandler]
    public class M2C_CreateUnitsHandler: AMHandler<M2C_CreateUnits>
    {
        protected override async ETTask Run(ETModel.Session session, M2C_CreateUnits message)
        {
            foreach (UnitInfo unitInfo in message.Units)
            {
                //TODO 暂时先忽略除英雄之外的Unit（如技能碰撞体），后期需要配表来解决这一块的逻辑，并且需要在协议里指定Unit的类型Id（注意不是运行时的Id,是Excel表中的类型Id）
                //TODO 诺手UnitTypeId暂定10001
                if (UnitComponent.Instance.Get(unitInfo.UnitId) != null || unitInfo.UnitTypeId != 10001)
                {
                    continue;
                }

                //根据不同名称和ID，创建英雄
                Unit unit = UnitFactory.CreateHero(unitInfo.UnitId, unitInfo.UnitTypeId, (RoleCamp) unitInfo.RoleCamp);
                //因为血条需要，创建热更层unit
                HotfixUnit hotfixUnit = HotfixUnitFactory.CreateHotfixUnit(unit, true);

                hotfixUnit.AddComponent<FallingFontComponent>();

                unit.Position = new Vector3(unitInfo.X, unitInfo.Y, unitInfo.Z);

                // 创建头顶Bar
                Game.EventSystem.Run(EventIdType.CreateHeadBar, unitInfo.UnitId);
            }

            //ETModel.Log.Info($"{DateTime.UtcNow}完成一次创建Unit");
            await ETTask.CompletedTask;
        }
    }
}