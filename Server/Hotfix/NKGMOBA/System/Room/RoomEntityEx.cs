using ETModel;

namespace ETHotfix
{
    public static class RoomEntityEx
    {
        /// <summary>
        /// 玩家进入房间
        /// </summary>
        /// <param name="userInfo"></param>
        public static async ETTask AddUnit(this RoomEntity self, long gateSessionId, bool isMaster, UserInfo userInfo)
        {
            var unit = ComponentFactory.CreateWithId<Unit>(userInfo.Id);
            //添加同gate服务器通信基础组件，记录GateSeesion的Id为ActorId
            unit.AddComponent<UnitGateComponent, long>(gateSessionId);
            //设置阵容
            unit.AddComponent<B2S_RoleCastComponent>();
            unit.AddComponent<MailBoxComponent>();
            self.GetComponent<CampAllocManagerComponent>().AllocRoleCamp(unit);
            //设置房间内玩家信息
            var playerdata = unit.AddComponent<RoomPlayerData>();
            playerdata.NickName = userInfo.NickName;
            playerdata.IsMaster = isMaster;
            unit.Parent = self.GetComponent<RoomPlayerComponent>();
            await self.GetComponent<RoomPlayerComponent>().AddUnit(unit);
            Session mgrSession = Game.Scene.GetComponent<NetInnerComponent>().Get(StartConfigComponent.Instance.RoomManagerConfig.GetComponent<InnerConfig>().IPEndPoint);
            mgrSession.Send(new UpdateRoomToRoomManager() { BriefInfo = self.BriefInfo });
            MessageHelper.Broadcast(self.GetComponent<RoomPlayerComponent>().PlayerArray, new RM2C_RoomInfoUpdate() { Roominfo = self.Roominfo });
        }
        /// <summary>
        /// 玩家退出房间
        /// </summary>
        /// <param name="id"></param>
        public static async ETTask RemoveUnit(this RoomEntity self, long unitid, RoomPlayerQuitTypeEnum quitType)
        {
            if (self.GetComponent<RoomPlayerComponent>().Players.TryGetValue(unitid, out var unit))
            {
                bool isMaster = unit.GetComponent<RoomPlayerData>().IsMaster;
                switch (quitType)
                {
                    case RoomPlayerQuitTypeEnum.SelfQuit:
                        break;
                    case RoomPlayerQuitTypeEnum.BeKicked:
                    case RoomPlayerQuitTypeEnum.RoomDismiss:
                        MessageHelper.SendMsgToUnit(unit, new RM2C_LeaveRoom() { LeaveReason = (int)quitType });
                        break;
                    default:
                        break;
                }
                self.GetComponent<CampAllocManagerComponent>().RemoveRoleCamp(unit);
                await self.GetComponent<RoomPlayerComponent>().RemoveUnit(unitid);
                if (isMaster)
                {
                    self.Dispose();
                }
                else
                {
                    Session mgrSession = Game.Scene.GetComponent<NetInnerComponent>().Get(StartConfigComponent.Instance.RoomManagerConfig.GetComponent<InnerConfig>().IPEndPoint);
                    mgrSession.Send(new UpdateRoomToRoomManager() { BriefInfo = self.BriefInfo });
                    MessageHelper.Broadcast(self.GetComponent<RoomPlayerComponent>().PlayerArray, new RM2C_RoomInfoUpdate() { Roominfo = self.Roominfo });
                }
            }
        }
    }
}
