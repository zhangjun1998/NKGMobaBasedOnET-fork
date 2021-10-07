//此文件格式由工具自动生成

using System;
using System.Collections.Generic;
namespace ET
{
    #region System
    
    public class CDComponentAwakeSystem: AwakeSystem<CDComponent>
    {
        public override void Awake(CDComponent self)
        {
            self.Awake();
        }
    }
    
    public class CDComponentUpdateSystem: UpdateSystem<CDComponent>
    {
        public override void Update(CDComponent self)
        {
            self.Update();
        }
    }
    
    public class CDComponentFixedUpdateSystem: FixedUpdateSystem<CDComponent>
    {
        public override void FixedUpdate(CDComponent self)
        {
            self.FixedUpdate();
        }
    }
    
    public class CDComponentDestroySystem: DestroySystem<CDComponent>
    {
        public override void Destroy(CDComponent self)
        {
            self.Destroy();
        }
    }

    #endregion

    public class CDInfo: IReference
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 时间间隔（CD）
        /// </summary>
        public long Interval { get; set; }

        /// <summary>
        /// 剩余CD时长
        /// </summary>
        public long RemainCDLength { get; set; }

        /// <summary>
        /// CD是否转好了
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// CD信息变化时的回调
        /// </summary>
        public Action<CDInfo> CDChangedCallBack;

        public void Init(string name, long cdLength, Action<CDInfo> cDChangedCallBack = null)
        {
            this.Name = name;
            this.Interval = cdLength;
            this.RemainCDLength = cdLength;
            this.Result = true;
            this.CDChangedCallBack = cDChangedCallBack;
        }

        public void Clear()
        {
            this.Name = null;
            this.Interval = 0;
            this.RemainCDLength = 0;
            this.Result = false;
            this.CDChangedCallBack = null;
        }
    }

    /// <summary>
    /// CD组件，用于统一管理所有的CD类型的数据，比如攻速CD，服务器上因试图攻击导致的循环MoveTo CD
    /// </summary>
    public class CDComponent: Entity
    {
        #region 私有成员

        /// <summary>
        /// 包含所有CD信息的字典
        /// 键为id，值为对应所有CD信息
        /// </summary>
        private Dictionary<long, Dictionary<string, CDInfo>> CDInfos = new Dictionary<long, Dictionary<string, CDInfo>>();

        #endregion

        #region 公有成员

        /// <summary>
        /// 新增一条CD数据
        /// </summary>
        public CDInfo AddCDData(long id, string name, long cDLength, Action<CDInfo> onCDChangedCallback = null)
        {
            if (this.GetCDData(id, name) != null)
            {
                Log.Error($"已注册id为：{id}，Name为：{name}的CD信息，请勿重复注册");
                return null;
            }

            CDInfo cdInfo = ReferencePool.Acquire<CDInfo>();
            cdInfo.Init(name, cDLength, onCDChangedCallback);
            AddCDData(id, cdInfo);
            return cdInfo;
        }

        /// <summary>
        /// 新增一条CD数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cdInfo"></param>
        public CDInfo AddCDData(long id, CDInfo cdInfo)
        {
            if (this.CDInfos.TryGetValue(id, out var cdInfoDic))
            {
                cdInfoDic.Add(cdInfo.Name, cdInfo);
            }
            else
            {
                CDInfos.Add(id, new Dictionary<string, CDInfo>() { { cdInfo.Name, cdInfo } });
            }

            return cdInfo;
        }

        /// <summary>
        /// 触发某个CD，使其进入CD状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="cdLength">CD长度</param>
        public void TriggerCD(long id, string name, long cdLength = -1)
        {
            CDInfo cdInfo = GetCDData(id, name);
            cdInfo.Result = false;
            cdInfo.Interval = cdLength == -1? cdInfo.Interval : cdLength;
            cdInfo.RemainCDLength = cdInfo.Interval;
        }

        /// <summary>
        /// 获取CD数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public CDInfo GetCDData(long id, string name)
        {
            if (this.CDInfos.TryGetValue(id, out var cdInfoDic))
            {
                if (cdInfoDic.TryGetValue(name, out var cdInfo))
                {
                    return cdInfo;
                }
            }

            return null;
        }

        /// <summary>
        /// 增加CD时间到指定CD
        /// </summary>
        public void AddCD(long id, string name, long addedCDLength)
        {
            CDInfo cdInfo = GetCDData(id, name);
            cdInfo.Interval += addedCDLength;
            cdInfo.CDChangedCallBack?.Invoke(cdInfo);
        }

        /// <summary>
        /// 减少CD时间到指定CD
        /// </summary>
        public void ReduceCD(long id, string name, long reducedCDLength)
        {
            CDInfo cdInfo = GetCDData(id, name);
            cdInfo.Interval -= reducedCDLength;
            cdInfo.CDChangedCallBack?.Invoke(cdInfo);
        }

        /// <summary>
        /// 直接设定CD时间到指定CD
        /// </summary>
        public void SetCD(long id, string name, long cDLength, long remainCDLength)
        {
            CDInfo cdInfo = GetCDData(id, name);
            cdInfo.Interval = cDLength;
            cdInfo.RemainCDLength = remainCDLength;
            cdInfo.Result = false;
            cdInfo.CDChangedCallBack?.Invoke(cdInfo);
        }

        /// <summary>
        /// 获取CD结果
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool GetCDResult(long id, string name)
        {
            if (this.CDInfos.TryGetValue(id, out var cdInfoDic))
            {
                if (cdInfoDic.TryGetValue(name, out var cdInfo))
                {
                    return cdInfo.Result;
                }
            }

            Log.Error($"尚未注册id为：{id}，Name为：{name}的CD信息");
            return false;
        }

        /// <summary>
        /// 移除一条CD数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public void RemoveCDData(long id, string name)
        {
            if (this.CDInfos.TryGetValue(id, out var cdInfoDic))
            {
                cdInfoDic.Remove(name);
            }
        }

        #endregion

        #region 生命周期函数

        public void Awake()
        {
        }

        public void Update()
        {
        }

        public void FixedUpdate()
        {
            //此处填写FixedUpdate逻辑
            foreach (var cdInfoDic in this.CDInfos)
            {
                foreach (var cdInfo in cdInfoDic.Value)
                {
                    if (!cdInfo.Value.Result)
                    {
                        //TODO  切换帧同步驱动，使用帧数而非时间
                        cdInfo.Value.RemainCDLength -= 16;
                        if (cdInfo.Value.RemainCDLength <= 0)
                        {
                            cdInfo.Value.Result = true;
                            cdInfo.Value.CDChangedCallBack?.Invoke(cdInfo.Value);
                        }
                    }
                }
            }
        }

        public void Destroy()
        {
            //此处填写Destroy逻辑
            foreach (var cdInfoList in CDInfos)
            {
                foreach (var cdInfo in cdInfoList.Value)
                {
                    ReferencePool.Release(cdInfo.Value);
                }

                cdInfoList.Value.Clear();
            }

            this.CDInfos.Clear();
        }

        public override void Dispose()
        {
            if (IsDisposed)
                return;
            base.Dispose();
            //此处填写释放逻辑,但涉及Entity的操作，请放在Destroy中
        }

        #endregion

        public void ResetCD(long belongToUnitId, string cdName)
        {
            CDInfo cdInfo = GetCDData(belongToUnitId, cdName);
            cdInfo.RemainCDLength = 0;
            cdInfo.Result = true;
        }
    }
}