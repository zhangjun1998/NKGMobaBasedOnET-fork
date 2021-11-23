using System;
using System.Collections.Generic;

namespace ET
{
    //房主点击开始游戏向客户端返回房内玩家列表
    public class C2L_StartGameLobbyHandler : AMActorRpcHandler<Player, C2L_StartGameLobby, L2C_StartGameLobby>
    {
        protected override async ETTask Run(Player player, C2L_StartGameLobby request, L2C_StartGameLobby response,
            Action reply)
        {
            Scene scene = player.DomainScene();
            if (scene.GetComponent<Room>().RoomHolderPlayerId != player.Id)
            {
                response.Error = ErrorCode.ERR_RoomNeedHolder;
                reply();
                return;
            }
            //if (!RoomHelper.CanStartGame(scene))
            //{
            //    response.Error = ErrorCode.ERR_StartGameFail;
            //    reply();
            //    return;
            //}
            reply();
            //如果正常.加载战斗需要的组件
            RoomHelper.InitBattleComponent(scene);
            // 广播给客户端进入战斗
            MessageHelper.BroadcastToRoom(scene, new L2C_PrepareToEnterBattle() { Units = UnitHelper.CreateUnitInfo(scene) });
            RoomHelper.UpdateRoomToRoomManager(scene);
            await ETTask.CompletedTask;
        }
    }
}