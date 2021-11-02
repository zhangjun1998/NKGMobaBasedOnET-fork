namespace ET
{
    public static partial class ErrorCode
    {
        public const int ERR_Success = 0;

        // 1-11004 是SocketError请看SocketError定义
        //-----------------------------------
        // 100000-109999是Core层的错误
        // 110000 以上，避免跟SocketError冲突
        public const int ERR_MyErrorCode = 110000;
        
        public const int ERR_KcpConnectTimeout = 100205;
        public const int ERR_PeerDisconnect = 100208;
        public const int ERR_SocketCantSend = 100209;
        public const int ERR_SocketError = 100210;
        public const int ERR_KcpWaitSendSizeTooLarge = 100211;
        public const int ERR_KcpCreateError = 100212;
        public const int ERR_SendMessageNotFoundTChannel = 100213;
        public const int ERR_TChannelRecvError = 100214;
        public const int ERR_MessageSocketParserError = 100215;
        public const int ERR_KcpNotFoundChannel = 100216;

        public const int ERR_WebsocketSendError = 100217;
        public const int ERR_WebsocketPeerReset = 100218;
        public const int ERR_WebsocketMessageTooBig = 100219;
        public const int ERR_WebsocketRecvError = 100220;
        
        public const int ERR_KcpReadNotSame = 100230;
        public const int ERR_KcpSplitError = 100231;
        public const int ERR_KcpSplitCountError = 100232;

        public const int ERR_ActorNoMailBoxComponent = 110003;
        public const int ERR_ActorLocationSenderTimeout = 110004;
        public const int ERR_PacketParserError = 110005;
        public const int ERR_KcpChannelAcceptTimeout = 110206;
        public const int ERR_KcpRemoteDisconnect = 110207;
        public const int ERR_WebsocketError = 110303;
        public const int ERR_WebsocketConnectError = 110304;
        public const int ERR_RpcFail = 110307;
        public const int ERR_ReloadFail = 110308;
        public const int ERR_ConnectGateKeyError = 110309;
        public const int ERR_SessionSendOrRecvTimeout = 110311;
        public const int ERR_OuterSessionRecvInnerMessage = 110312;
        public const int ERR_NotFoundActor = 110313;
        public const int ERR_ActorTimeout = 110315;
        public const int ERR_UnverifiedSessionSendMessage = 110316;
        public const int ERR_ActorLocationSenderTimeout2 = 110317;
        public const int ERR_ActorLocationSenderTimeout3 = 110318;
        public const int ERR_ActorLocationSenderTimeout4 = 110319;
        public const int ERR_ActorLocationSenderTimeout5 = 110320;
        
        public const int ERR_KcpRouterTimeout = 110401;
        public const int ERR_KcpRouterTooManyPackets = 110402;
        public const int ERR_KcpRouterSame = 110402;

        //-----------------------------------
        // 小于这个Rpc会抛异常，大于这个异常的error需要自己判断处理，也就是说需要处理的错误应该要大于该值
        public const int ERR_Exception = 200000;

        public const int ERR_Cancel = 200001;
        
        /// <summary>
        /// 账号或密码不存在
        /// </summary>
        public const int ERR_LoginError = 200002;
        
        /// <summary>
        /// 账号已被注册
        /// </summary>
        public const int ERR_AccountHasExist = 200003;

        /// <summary>
        /// 房间不存在
        /// </summary>
        public const int ERR_RoomNotExist = 200004;
        
        /// <summary>
        /// 开局失败
        /// </summary>
        public const int ERR_StartGameFail = 200005;
        /// <summary>
        /// 已经在房间中
        /// </summary>
        public const int ERR_AlreadyInRoom = 200006;
        /// <summary>
        /// 房间锁定中
        /// </summary>
        public const int ERR_RoomIsLock = 200007;
        /// <summary>
        /// 房间已满
        /// </summary>
        public const int ERR_RoomIsFull= 200008;
        /// <summary>
        /// 需要房主权限
        /// </summary>
        public const int ERR_RoomNeedHolder = 200009;
        public static bool IsRpcNeedThrowException(int error)
        {
            if (error == 0)
            {
                return false;
            }
            // ws平台返回错误专用的值
            if (error == -1)
            {
                return false;
            }

            if (error > ERR_Exception)
            {
                return false;
            }

            return true;
        }

        public static bool IsTargetNotOnline(this int error)
        {
            if (error == ERR_NotFoundActor)
            {
                return true;
            }
            return false;
        }
    }
}