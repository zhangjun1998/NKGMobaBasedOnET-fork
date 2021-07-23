
using ETModel;
using System;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2RM_CreateRoomHandler : AMRpcHandler<C2RM_CreateRoom, RM2C_CreateRoom>
    {
        protected override async ETTask Run(Session session, C2RM_CreateRoom message, RM2C_CreateRoom response, Action reply)
        {
            Player player = session.GetComponent<SessionPlayerComponent>().Player;
            using (await CoroutineLockComponent.Instance.Wait(player.PlayerIdInDB))
            {
                if (session.GetComponent<SessionRoomComponent>() != null)
                {
                    response.Error = ErrorCode.ERR_AlreadyInRoom;
                    reply();
                    return;
                }
                try
                {
                    //获取到了说明已经在战斗中
                    var UnitId = await Game.Scene.GetComponent<LocationProxyComponent>().Get(player.PlayerIdInDB);
                    response.Error = ErrorCode.ERR_AlreadyInBattle;
                    reply();
                    return;
                }
                //说明没在战斗中.继续创建逻辑.太粗暴了
                catch
                {

                }
                //统一入口的话应该到roommanager上请求战斗
                Session mgrSession = Game.Scene.GetComponent<NetInnerComponent>().Get(StartConfigComponent.Instance.RoomConfigs[0].GetComponent<InnerConfig>().IPEndPoint);
                RM2G_CreateRoom createRoomResponse =(RM2G_CreateRoom)await mgrSession.Call(new G2RM_CreateRoom() { UnitId = player.PlayerIdInDB,GateSessionId=session.InstanceId });
                response.Error = createRoomResponse.Error;
                if (response.Error==0)
                {
                    session.AddComponent<SessionRoomComponent>().RoomActorId= createRoomResponse.RoomActorId;
                }
                reply();
            }
        }
    }

    [MessageHandler(AppType.Room)]
    public class G2RM_CreateRoomHandler : AMRpcHandler<G2RM_CreateRoom, RM2G_CreateRoom>
    {
        protected override async ETTask Run(Session session, G2RM_CreateRoom message, RM2G_CreateRoom response, Action reply)
        {
            DBProxyComponent dbProxyComponent = Game.Scene.GetComponent<DBProxyComponent>();
            UserInfo userInfo = await dbProxyComponent.Query<UserInfo>(message.UnitId);
            var room=ComponentFactory.Create<RoomEntity,string>(userInfo.NickName);
            response.RoomActorId = room.InstanceId;
            reply();
            room.AddUser(message.GateSessionId,userInfo);
        }
    }
}
