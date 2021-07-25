using ETModel;

namespace ETHotfix
{
    public static class RoomEntityEx
    {
        /// <summary>
        /// 玩家进入房间
        /// </summary>
        /// <param name="userInfo"></param>
        public static async ETTask AddUnit(this RoomEntity self, long gateSessionId, UserInfo userInfo)
        {
            var unit = ComponentFactory.CreateWithId<Unit>(userInfo.Id);
            //添加同gate服务器通信基础组件，记录GateSeesion的Id为ActorId
            unit.AddComponent<UnitGateComponent, long>(gateSessionId);
            self.GetComponent<RoomPlayerComponent>().Players.Add(unit.Id, unit);
            unit.Parent = self;
            UnitComponent.Instance.Add(unit);
            await unit.AddComponent<MailBoxComponent>().AddLocation();
        }
        /// <summary>
        /// 玩家退出房间
        /// </summary>
        /// <param name="id"></param>
        public static void RemoveUnit(this RoomEntity self,Unit unit)
        {
            self.GetComponent<RoomPlayerComponent>().Players.Remove(unit.Id);
            UnitComponent.Instance.Remove(unit.Id);
        }
    }
}
