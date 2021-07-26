//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年6月13日 20:20:49
//------------------------------------------------------------

using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.RoomManager)]
    public class C2G_AllRoomListHandler : AMRpcHandler<C2G_AllRoomList, G2C_AllRoomList>
    {
        protected override async ETTask Run(Session session, C2G_AllRoomList message, G2C_AllRoomList response, Action reply)
        {
            foreach (var item in Game.Scene.GetComponent<RoomManagerEntity>().AllRoomDic.Values)
            {
                response.RoomList.Add(item);
            }
            reply();
        }
    }
}