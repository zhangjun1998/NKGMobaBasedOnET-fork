using System;
using UnityEngine;

namespace ET
{
    [ActorMessageHandler]
    //gate向RoomManager申请新的房间
    public class G2RM_CreateNewRoomLobbyHandler : AMActorRpcHandler<Scene, G2RM_CreateNewRoomLobby, RM2G_CreateNewRoomLobby>
    {
        protected override async ETTask Run(Scene scene, G2RM_CreateNewRoomLobby request, RM2G_CreateNewRoomLobby response, Action reply)
        {
            var roommanager = scene.GetComponent<RoomManagerComponent>();
            int n = RandomHelper.RandomNumber(0, StartSceneConfigCategory.Instance.RoomAgent.Count);
            var actorid = StartSceneConfigCategory.Instance.RoomAgent[n].InstanceId;
            var resp = (RA2RM_CreateNewRoomLobby)await MessageHelper.CallActor(actorid, new RM2RA_CreateNewRoomLobby() { RoomConfig = request.RoomConfig });
            if (resp.Error != 0)
            {
                response.Error = resp.Error;
                reply();
                return;
            }
            roommanager.Rooms.Add(resp.RoomInfo.RoomId, resp.RoomInfo);
            response.RoomInfo = resp.RoomInfo;
            reply();
        }
    }
    [ActorMessageHandler]
    //Room向RoomManager发送信息更新请求
    public class RA2RM_UpdateRoomInfoHandler : AMActorHandler<Scene, RA2RM_UpdateRoomInfo>
    {
        protected override async ETTask Run(Scene scene, RA2RM_UpdateRoomInfo message)
        {
            await ETTask.CompletedTask;
            var roommanager = scene.GetComponent<RoomManagerComponent>();
            int needCloseRoom = 0;
            if (roommanager.Rooms.TryGetValue(message.RoomInfo.RoomId, out var roomInfo))
            {
                if (roomInfo.IsGameing && message.RoomInfo.IsGameing == false)
                {
                    needCloseRoom = 1;
                }
                //判断房主退出.战斗结束关闭房间
                else if (roomInfo.RoomHolderPlayer != 0 && message.RoomInfo.RoomHolderPlayer == 0)
                {
                    needCloseRoom = 2;
                }
            }
            if (needCloseRoom != 0)
            {
                roommanager.Rooms.Remove(message.RoomInfo.RoomId);
                MessageHelper.SendActor(message.RoomInfo.RoomId, new RM2RA_RemoveRoom() { CloseCode = needCloseRoom });
                return;
            }
            roommanager.Rooms[message.RoomInfo.RoomId] = message.RoomInfo;

        }
    }
    //RoomManager通知RoomAgent创建新的房间场景
    [ActorMessageHandler]
    public class RM2RA_CreateNewRoomLobbyHandler : AMActorRpcHandler<Scene, RM2RA_CreateNewRoomLobby, RA2RM_CreateNewRoomLobby>
    {
        protected override async ETTask Run(Scene scene, RM2RA_CreateNewRoomLobby request, RA2RM_CreateNewRoomLobby response, Action reply)
        {
            var newroomScene = await RoomSceneFactory.Create(scene, request.RoomConfig);
            response.RoomInfo = RoomHelper.GetRoomInfoProto(newroomScene);
            reply();
        }
    }
    [ActorMessageHandler]
    //RoomManager通知RoomScene关闭房间
    public class RM2RA_RemoveRoomHandler : AMActorHandler<Scene, RM2RA_RemoveRoom>
    {
        protected override async ETTask Run(Scene scene, RM2RA_RemoveRoom request)
        {
            await ETTask.CompletedTask;
            //通知房间关闭.有额外处理内容所以需要内网actormessage
            MessageHelper.BroadcastToRoom(scene, new Room2G_RoomClose() { CloseCode = request.CloseCode });
            scene.Dispose();
        }
    }
    [ActorMessageHandler]
    //RoomScene通知GateSession关闭房间
    public class Room2G_RoomCloseHandler : AMActorHandler<Session, Room2G_RoomClose>
    {
        protected override async ETTask Run(Session session, Room2G_RoomClose request)
        {
            await ETTask.CompletedTask;
            session.RemoveComponent<RoomStateOnGateComponent>();
            session.Send(new L2C_RoomClose() { CloseCode = request.CloseCode });
        }
    }
    public static class RoomHelper
    {
        public static void JoinRoom(Scene scene, Player player, bool isRoomHolder = false)
        {
            player.Parent = scene.GetComponent<PlayerComponent>();
            player.AddComponent<MailBoxComponent>();
            scene.GetComponent<PlayerComponent>().Add(player);
            if (isRoomHolder)
            {
                scene.GetComponent<Room>().RoomHolderPlayerId = player.Id;
            }
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

        public static void LeaveRoom(Scene scene, long playerid)
        {
            var player = scene.GetComponent<PlayerComponent>().Get(playerid);
            scene.GetComponent<PlayerComponent>().Remove(playerid);
            player.Dispose();
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
            scene.AddComponent<BattleComponent>().BattleStartTime = TimeHelper.ServerNow();
            scene.AddComponent<RoomPreparedToEnterBattleComponent>();
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
            var unitcomponent = scene.AddComponent<UnitComponent>();
            foreach (Player player in scene.GetComponent<PlayerComponent>().GetAll())
            {
                Vector3 unitPos = new Vector3(-10, 0, -10);

                RoleCamp heroCamp = RoleCamp.HuiYue;
                if (player.camp % 2 == 0)
                {
                    heroCamp = RoleCamp.TianZai;
                    unitPos = new Vector3(-15, 0, 0);
                }
                Unit unit = UnitFactory.CreateHeroUnit(unitcomponent, 10001, heroCamp, unitPos, Quaternion.identity);
                unit.BelongToPlayer = player;
                unit.AddComponent<UnitGateComponent, long>(player.GateSessionId);
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
    public static class RoomSceneFactory
    {
        public static async ETTask<Scene> Create(Entity parent, RoomConfigProto RoomConfig)
        {
            await ETTask.CompletedTask;
            long instanceId = IdGenerater.Instance.GenerateInstanceId();
            Scene scene = EntitySceneFactory.CreateScene(instanceId, parent.DomainZone(), SceneType.Room, "", parent);
            var playercomponent = scene.AddComponent<PlayerComponent>();
            scene.AddComponent<MailBoxComponent, MailboxType>(MailboxType.UnOrderMessageDispatcher);
            return scene;
        }

    }
}