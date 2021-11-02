using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;


namespace ET
{
    [MessageHandler]
    public class C2RLoginHandler : AMRpcHandler<C2R_Login, R2C_Login>
    {
        protected override async ETTask Run(Session session, C2R_Login request, R2C_Login response, Action reply)
        {
            //只会对account建立索引.所以先查询account然后对比密码
            List<AccountInfo> accountInfos = await DBComponent.Instance.Query<AccountInfo>(account =>
                account.Account == request.Account);

            if (accountInfos.Count == 0)
            {
                response.Error = ErrorCode.ERR_LoginError;
                reply();
                return;
            }
            AccountInfo account = accountInfos[0];
            if (account.Password!= request.Password)
            {
                response.Error = ErrorCode.ERR_LoginError;
                reply();
                return;
            }
            // 固定分配一个Gate
            StartSceneConfig gateConfig = AddressHelper.GetGate(session.DomainZone(),account.Id);
            // 向gate请求一个key,客户端可以拿着这个key连接gate
            G2R_GetLoginKey g2RGetLoginKey = (G2R_GetLoginKey) await ActorMessageSenderComponent.Instance.Call(
                gateConfig.InstanceId, new R2G_GetLoginKey() {PlayerId=account.Id});
            
            response.GateAddress = gateConfig.OuterIPPortForClient.ToString();
            response.Key = g2RGetLoginKey.Key;
            response.GateId = g2RGetLoginKey.GateId;
            reply();
        }
    }
}