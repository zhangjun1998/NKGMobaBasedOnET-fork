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
}