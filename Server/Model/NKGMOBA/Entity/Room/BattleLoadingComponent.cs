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


        #endregion

        #region 生命周期函数

        public void Awake()
        {
            //此处填写Awake逻辑
        }
        
        public void Destroy()
        {
            //此处填写Destroy逻辑
            
        }
        
    
    #endregion

    }
}
