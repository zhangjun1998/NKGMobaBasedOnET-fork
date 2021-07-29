//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月5日 22:07:15
//------------------------------------------------------------

using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class RM2C_StartBattleMessageHandler : AMHandler<RM2C_StartBattleMessage>
    {
        protected override async ETTask Run(ETModel.Session session, RM2C_StartBattleMessage message)
        {
            Log.Info("收到了正式开始战斗指令");
            ETModel.Game.EventSystem.Run(ETModel.EventIdType.CloseLoadingUI);
            await ETTask.CompletedTask;
        }
    }
}