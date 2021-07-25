//此文件格式由工具自动生成
using System.Collections.Generic;

namespace ETModel
{

    #region System


    [ObjectSystem]
    public class BattleLoadingComponentDestroySystem : DestroySystem<BattleLoadingComponent>
    {
        public override void Destroy(BattleLoadingComponent self)
        {
            self.Destroy();
        }
    }

    #endregion


    /// <summary>
    /// 同步客户端资源加载组件,所有客户端完成加载后开始战斗
    /// </summary>
    public class BattleLoadingComponent : Component
    {
        #region 私有成员



        #endregion

        #region 公有成员
        public long timerId;
        public List<long> LoadCompletedIds;
        public int NeedNum;

        #endregion

        #region 生命周期函数

        public void Awake(int num)
        {
            //此处填写Awake逻辑
            LoadCompletedIds = new List<long>();
            NeedNum = num;
        }

        public void Destroy()
        {
            //此处填写Destroy逻辑
            NeedNum = 0;
            LoadCompletedIds = null;
            timerId = 0;
        }


        #endregion

    }
}
