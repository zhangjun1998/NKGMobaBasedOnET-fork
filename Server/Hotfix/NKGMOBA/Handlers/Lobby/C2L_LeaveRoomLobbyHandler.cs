using System;

namespace ET
{
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