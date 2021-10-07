using System;
using System.Collections.Generic;

namespace ET
{
    public class C2R_RegisteHandler : AMRpcHandler<C2R_Registe, R2C_Registe>
    {
        protected override async ETTask Run(Session session, C2R_Registe request, R2C_Registe response, Action reply)
        {
            //查询账号是否存在
            List<AccountInfo> result =
                await DBComponent.Instance.Query<AccountInfo>(_account => _account.Account == request.Account);

            if (result.Count > 0)
            {
                response.Error = ErrorCode.ERR_AccountHasExist;
                reply();
                return;
            }

            //新建账号
            AccountInfo newAccount =
                Entity.CreateWithId<AccountInfo>(session.DomainScene(), IdGenerater.Instance.GenerateId());
            newAccount.Account = request.Account;
            newAccount.Password = request.Password;

            //新建用户信息
            PlayerInfo newUser = Entity.Create<PlayerInfo>(session.DomainScene(), true);
            Entity.Create<PlayerInfo>(session.DomainScene(), true);
            newUser.Name = $"玩家{request.Account}";
            newUser.Level = 1;

            // 保存用户数据到数据库
            await DBComponent.Instance.Save(newAccount);
            await DBComponent.Instance.Save(newUser);

            reply();
        }
    }
}