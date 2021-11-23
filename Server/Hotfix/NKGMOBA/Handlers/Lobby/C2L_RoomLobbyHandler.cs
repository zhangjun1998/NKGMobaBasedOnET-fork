using System;
using System.Collections.Generic;

namespace ET
{
    [ActorMessageHandler]
    //Gate发给RoomScene
    public class G2L_JoinRoomHandler : AMActorRpcHandler<Scene, G2L_JoinRoomLobby, L2C_JoinRoomLobby>
    {
        protected override async ETTask Run(Scene scene, G2L_JoinRoomLobby request, L2C_JoinRoomLobby response,
            Action reply)
        {
            Room room = scene.GetComponent<Room>();
            if (room == null)
            {
                response.Error = ErrorCode.ERR_RoomNotExist;
                reply();
                return;
            }
            if (scene.GetComponent<PlayerComponent>().Get(request.Player.Id) != null)
            {
                //已经加入过了
                response.Error = ErrorCode.ERR_RoomNotExist;
                reply();
                return;
            }
            if (!RoomHelper.IsRoomUnLock(scene))
            {
                response.Error = ErrorCode.ERR_RoomIsLock;
                reply();
                return;
            }
            if (room.RoomConfig.RoomPlayerNum <= scene.GetComponent<PlayerComponent>().Count)
            {
                response.Error = ErrorCode.ERR_RoomIsFull;
                reply();
                return;
            }
            L2C_PlayerTriggerRoom playerTrigger = new L2C_PlayerTriggerRoom();
            playerTrigger.playerInfoRoom = new PlayerInfoRoom
            {
                Name = request.Player.Name,
                camp = request.Player.camp,
                playerid = request.Player.Id
            };
            playerTrigger.JoinOrLeave = true;
            MessageHelper.BroadcastToRoom(scene, playerTrigger);
            //通知完之后再加入玩家.免得通知到自己
            RoomHelper.JoinRoom(scene, request.Player, request.IsRoomHolder);
            response.RoomInfo = response.RoomInfo;
            foreach (var player in scene.GetComponent<PlayerComponent>().GetAll())
            {
                response.playerInfoRoom.Add(new PlayerInfoRoom() { camp = player.camp, Name = player.Name, playerid = player.Id });
            }
            response.RoomInfo = RoomHelper.GetRoomInfoProto(scene);
            response.RoomPlayerActorId = request.Player.InstanceId;
            reply();
            RoomHelper.UpdateRoomToRoomManager(scene);
            await ETTask.CompletedTask;
        }
    }
    //玩家加入房间
    public class C2L_JoinRoomHandler : AMRpcHandler<C2L_JoinRoomLobby, L2C_JoinRoomLobby>
    {
        protected override async ETTask Run(Session session, C2L_JoinRoomLobby request, L2C_JoinRoomLobby response,
            Action reply)
        {
            if (session.GetComponent<RoomStateOnGateComponent>() != null)
            {
                response.Error = ErrorCode.ERR_AlreadyInRoom;
                reply();
                return;
            }
            var player = session.GetComponent<SessionPlayerComponent>().Player;
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.Room, player.Id))
            {
                var resp = (L2C_JoinRoomLobby)await MessageHelper.CallActor(request.RoomId, new G2L_JoinRoomLobby() { Player = player, IsRoomHolder = false });
                if (resp.Error != 0)
                {
                    response.Error = resp.Error;
                    reply();
                    return;
                }
                response.RoomInfo = resp.RoomInfo;
                response.playerInfoRoom = resp.playerInfoRoom;
                session.AddComponent<RoomStateOnGateComponent, long, long>(resp.RoomPlayerActorId, resp.RoomInfo.RoomId);

                reply();
            }
        }
    }

    //点击创建房间逻辑
    public class C2L_CreateNewRoomLobbyHandler : AMRpcHandler<C2L_CreateNewRoomLobby, L2C_CreateNewRoomLobby>
    {
        protected override async ETTask Run(Session session, C2L_CreateNewRoomLobby request,
            L2C_CreateNewRoomLobby response,
            Action reply)
        {
            if (session.GetComponent<RoomStateOnGateComponent>() != null)
            {
                response.Error = ErrorCode.ERR_AlreadyInRoom;
                reply();
                return;
            }
            var player = session.GetComponent<SessionPlayerComponent>().Player;
            var playerinfo = new PlayerInfoRoom() { camp = 1, Name = player.Name, playerid = player.Id };
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.Room, player.Id))
            {
                var resp = (RM2G_CreateNewRoomLobby)await MessageHelper.CallActor(
                    StartSceneConfigCategory.Instance.GetBySceneName(1, "RoomManager").InstanceId,
                    new G2RM_CreateNewRoomLobby()
                    {
                        RoomConfig = new RoomConfigProto() { RoomName = $"{player.Name}的房间", RoomPlayerNum = 10 },
                    });
                if (resp.Error != 0)
                {
                    response.Error = resp.Error;
                    reply();
                    return;
                }
                var joinresp = (L2C_JoinRoomLobby)await MessageHelper.CallActor(resp.RoomInfo.RoomId, new G2L_JoinRoomLobby() { Player = player, IsRoomHolder = true });
                if (joinresp.Error != 0)
                {
                    response.Error = joinresp.Error;
                    reply();
                    return;
                }
                session.AddComponent<RoomStateOnGateComponent, long,long>(joinresp.RoomPlayerActorId, joinresp.RoomInfo.RoomId);
                response.RoomInfo = joinresp.RoomInfo;
                response.playerInfoRoom.Add(playerinfo);
                reply();
            }
        }
    }

    //玩家离开房间逻辑
    public class C2L_LeaveRoomLobbyHandler : AMActorRpcHandler<Player, C2L_LeaveRoomLobby, L2C_LeaveRoomLobby>
    {
        protected override async ETTask Run(Player player, C2L_LeaveRoomLobby request, L2C_LeaveRoomLobby response,
            Action reply)
        {
            Scene scene = player.DomainScene();
            if (!RoomHelper.IsRoomUnLock(scene))
            {
                response.Error = ErrorCode.ERR_RoomIsLock;
                reply();
                return;
            }
            reply();
            //房主设置为0.说明房主退出了
            if (scene.GetComponent<Room>().RoomHolderPlayerId == player.Id)
            {
                scene.GetComponent<Room>().RoomHolderPlayerId = 0;
            }
            // 离开房间把自己广播给周围的人
            L2C_PlayerTriggerRoom leaveRoom = new L2C_PlayerTriggerRoom();
            leaveRoom.playerInfoRoom = new PlayerInfoRoom
            {
                Name = player.Name,
                camp = player.camp,
                playerid = player.Id
            };
            leaveRoom.JoinOrLeave = false;
            player.GetParent<PlayerComponent>().Remove(player.Id);
            player.Dispose();
            MessageHelper.BroadcastToRoom(scene, leaveRoom);
            RoomHelper.UpdateRoomToRoomManager(scene);
            await ETTask.CompletedTask;
        }
    }
}