
using ETModel;
using System;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2RM_JoinRoomHandler : AMRpcHandler<C2RM_JoinRoom, RM2C_JoinRoom>
    {
        protected override async ETTask Run(Session session, C2RM_JoinRoom message, RM2C_JoinRoom response, Action reply)
        {
            Player player = session.GetComponent<SessionPlayerComponent>().Player;
            using (await CoroutineLockComponent.Instance.Wait(player.PlayerIdInDB))
            {

                //获取到了说明已经在战斗中
                var UnitId = await Game.Scene.GetComponent<LocationProxyComponent>().Get(player.PlayerIdInDB);
                if (UnitId!=0)
                {
                    response.Error = ErrorCode.ERR_AlreadyInBattle;
                    reply();
                    return;
                }
                var actorSender = Game.Scene.GetComponent<ActorMessageSenderComponent>().Get(message.RoomId);
                RM2G_JoinRoom JoinRoomResponse = (RM2G_JoinRoom)await actorSender.Call(new G2RM_JoinRoom() { UnitId = player.PlayerIdInDB, GateSessionId = session.InstanceId });
                response.Error = JoinRoomResponse.Error;
                reply();
            }
        }
    }

    [ActorMessageHandler(AppType.Room)]
    public class G2RM_JoinRoomHandler : AMActorRpcHandler<RoomEntity, G2RM_JoinRoom, RM2G_JoinRoom>
    {
        protected override async ETTask Run(RoomEntity room, G2RM_JoinRoom message, RM2G_JoinRoom response, Action reply)
        {
            if (!room.CanUnitChangeState)
            {
                response.Error = ErrorCode.ERR_RoomAlreadyInBattle;
                reply();
                return;
            }
            DBProxyComponent dbProxyComponent = Game.Scene.GetComponent<DBProxyComponent>();
            UserInfo userInfo = await dbProxyComponent.Query<UserInfo>(message.UnitId);
            await room.AddUnit(message.GateSessionId, false, userInfo);
            reply();
        }
    }
}
