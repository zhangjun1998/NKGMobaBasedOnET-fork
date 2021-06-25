//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年6月24日 10:41:12
//------------------------------------------------------------

using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class M2C_SyncCDDataHandler: AMHandler<M2C_SyncCDData>
    {
        protected override ETTask Run(ETModel.Session session, M2C_SyncCDData message)
        {
            CDComponent.Instance.SetCD(message.UnitId, message.CDName, message.CDLength, message.RemainCDLength);
            return ETTask.CompletedTask;
        }
    }
}