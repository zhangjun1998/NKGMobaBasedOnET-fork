using UnityEngine;

namespace ET
{
    public static class RoomHelper
    {
        public static void JoinRoom(Scene scene, Player player, bool isRoomHolder = false)
        {
            player.camp = AutoCamp(scene);
            player.Parent = scene.GetComponent<PlayerComponent>();
            player.AddComponent<MailBoxComponent>();
            scene.GetComponent<PlayerComponent>().Add(player);
            if (isRoomHolder)
            {
                scene.GetComponent<Room>().RoomHolderPlayerId = player.Id;
            }
        }
        /// <summary>
        /// 简单的自动分配阵营
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        public static int AutoCamp(Scene scene)
        {
            return scene.GetComponent<PlayerComponent>().Count % 2;
        }
        /// <summary>
        /// 获取对立阵营
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        public static int GetOffsetCamp(int camp)
        {
            return camp % 2;
        }
        public static RoomInfoProto GetRoomInfoProto(Scene scene)
        {
            return new RoomInfoProto
            {
                IsGameing = scene.GetComponent<RoomPreparedToEnterBattleComponent>() != null || scene.GetComponent<BattleComponent>() != null,
                PlayerCount = scene.GetComponent<PlayerComponent>().Count,
                RoomConfig = scene.GetComponent<Room>().RoomConfig,
                RoomHolderPlayer = scene.GetComponent<Room>().RoomHolderPlayerId,
                RoomId = scene.InstanceId
            };
        }
        public static bool CanStartGame(Scene scene)
        {
            return scene.GetComponent<RoomPreparedToEnterBattleComponent>() != null && scene.GetComponent<BattleComponent>() != null;
        }


        /// <summary>
        /// 根据关键组件是否存在决定是否房间是锁定状态
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        public static bool IsRoomUnLock(Scene scene)
        {
            return scene.GetComponent<RoomPreparedToEnterBattleComponent>() == null && scene.GetComponent<BattleComponent>() == null;
        }
        /// <summary>
        /// 初始化战斗相关组件
        /// </summary>
        /// <param name="scene"></param>
        public static void InitBattleComponent(Scene scene)
        {
            //BattleComponent在准备完成时添加
            //scene.AddComponent<BattleComponent>().BattleStartTime = TimeHelper.ServerNow();
            scene.AddComponent<RoomPreparedToEnterBattleComponent,int>(scene.GetComponent<PlayerComponent>().Count);
            scene.AddComponent<NP_TreeDataRepositoryComponent>();
            scene.AddComponent<UnitAttributesDataRepositoryComponent>();
            scene.AddComponent<B2S_ColliderDataRepositoryComponent>();
            scene.AddComponent<NP_SyncComponent>();
            scene.AddComponent<RecastPathComponent>();
            scene.AddComponent<CDComponent>();
            scene.AddComponent<BattleEventSystem>();
            scene.AddComponent<B2S_WorldComponent>();
            scene.AddComponent<B2S_WorldColliderManagerComponent>();
            scene.AddComponent<B2S_CollisionListenerComponent>();
            scene.AddComponent<UnitComponent>();
            foreach (Player player in scene.GetComponent<PlayerComponent>().GetAll())
            {
                Vector3 unitPos = new Vector3(-10, 0, -10);

                RoleCamp heroCamp = RoleCamp.HuiYue;
                if (player.camp % 2 == 0)
                {
                    heroCamp = RoleCamp.TianZai;
                    unitPos = new Vector3(-12, 0,-10);
                }
                Unit unit = UnitFactory.CreateHeroUnit(scene, 10001, heroCamp, unitPos, Quaternion.identity);
                unit.BelongToPlayer = player;
                unit.AddComponent<UnitGateComponent, long>(player.GateSessionId);
                player.UnitId = unit.Id;
            }
        }
        /// <summary>
        /// 战斗结束移除战斗相关组件,如果需要回房间就做清理.不然直接移除整个房间scene
        /// </summary>
        /// <param name="scene"></param>
        public static void DestroyBattleComponent(Scene scene)
        {
            //scene.RemoveComponent<B2S_CollisionListenerComponent>();
        }
        /// <summary>
        /// 更新信息到房间管理
        /// </summary>
        /// <param name="scene"></param>
        public static void UpdateRoomToRoomManager(Scene scene)
        {
            MessageHelper.SendActor(StartSceneConfigCategory.Instance.GetBySceneName(1, "RoomManager").InstanceId, new RA2RM_UpdateRoomInfo() { RoomInfo = GetRoomInfoProto(scene) });
        }
    }
}