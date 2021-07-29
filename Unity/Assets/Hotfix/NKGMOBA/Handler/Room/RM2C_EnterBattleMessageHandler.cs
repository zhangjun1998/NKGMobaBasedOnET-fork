//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月5日 22:07:15
//------------------------------------------------------------

using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [MessageHandler]
    public class RM2C_EnterBattleMessageHandler : AMHandler<RM2C_EnterBattleMessage>
    {
        protected override async ETTask Run(ETModel.Session session, RM2C_EnterBattleMessage message)
        {
            Log.Info("收到了进入战斗指令");
            ETModel.Game.EventSystem.Run(ETModel.EventIdType.ShowLoadingUI);
            //ETModel.Game.Scene.GetComponent<FUIPackageComponent>().RemovePackage(FUIPackage.FUILogin);
            // 切换到map场景
            // 加载场景资源
            await ETModel.Game.Scene.GetComponent<ResourcesComponent>().LoadSceneAsync(ABPathUtilities.GetScenePath(SceneType.Map));
            // 创建5v5游戏
            M5V5GameFactory.CreateM5V5Game();

            // 临时引用5v5游戏
            M5V5Game m5V5Game = Game.Scene.GetComponent<M5V5GameComponent>().m_5V5Game;
            foreach (UnitInfo unitInfo in message.Units)
            {
                //TODO 暂时先忽略除英雄之外的Unit（如技能碰撞体），后期需要配表来解决这一块的逻辑，并且需要在协议里指定Unit的类型Id（注意不是运行时的Id,是Excel表中的类型Id）
                //TODO 诺手UnitTypeId暂定10001
                if (UnitComponent.Instance.Get(unitInfo.UnitId) != null || unitInfo.UnitTypeId != 10001)
                {
                    continue;
                }

                //根据不同名称和ID，创建英雄
                Unit unit = UnitFactory.CreateHero(unitInfo.UnitId, unitInfo.UnitTypeId, (RoleCamp)unitInfo.RoleCamp);
                //因为血条需要，创建热更层unit
                HotfixUnit hotfixUnit = HotfixUnitFactory.CreateHotfixUnit(unit, true);

                hotfixUnit.AddComponent<FallingFontComponent>();

                unit.Position = new Vector3(unitInfo.X, unitInfo.Y, unitInfo.Z);

                // 创建头顶Bar
                Game.EventSystem.Run(EventIdType.CreateHeadBar, unitInfo.UnitId);
            }
            PlayerComponent.Instance.MyPlayer.UnitId = PlayerComponent.Instance.MyPlayer.Id;
            // 给自己的Unit添加引用
            UnitComponent.Instance.MyUnit =
                    UnitComponent.Instance.Get(PlayerComponent.Instance.MyPlayer.UnitId);
            UnitComponent.Instance.MyUnit
                    .AddComponent<CameraComponent, Unit>(UnitComponent.Instance.MyUnit);

            UnitComponent.Instance.MyUnit.AddComponent<OutLineComponent>();
            Game.Scene.GetComponent<M5V5GameComponent>().GetHotfixUnit(PlayerComponent.Instance.MyPlayer.UnitId)
                    .AddComponent<PlayerHeroControllerComponent>();

            // 添加点击地图寻路组件
            m5V5Game.AddComponent<MapClickCompoent>();

            Game.EventSystem.Run(EventIdType.EnterMapFinish);
            //通知服务端加载完成
            ETModel.SessionComponent.Instance.Session.Send(new C2RM_LoadComplete());
        }
    }
}