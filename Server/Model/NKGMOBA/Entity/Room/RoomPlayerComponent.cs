using Google.Protobuf.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ETModel
{
    /// <summary>
    /// 管理房间内成员
    /// </summary>
    public class RoomPlayerComponent : Entity
    {
        public Dictionary<long, Unit> Players;
        public Unit[] PlayerArray => Players.Values.ToArray();
        public RepeatedField<RoomPlayer> RoomPlayers()
        {
            var list = new RepeatedField<RoomPlayer>();
            foreach (var unit in Players.Values)
            {
                list.Add(new RoomPlayer() { 
                    IsMaster=unit.GetComponent<RoomPlayerData>().IsMaster,
                    Name= unit.GetComponent<RoomPlayerData>().NickName,
                    PlayerId= unit.Id,
                    IsRed= unit.GetComponent<B2S_RoleCastComponent>().RoleCamp==RoleCamp.HuiYue
                    //IsRed=
                });
            }
            return list;
        }
    }
}
