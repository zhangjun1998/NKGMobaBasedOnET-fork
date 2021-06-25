//此文件格式由工具自动生成

using System;
using ETModel;

namespace ETHotfix
{
    #region System

    [ObjectSystem]
    public class FUI5V5MapComponentAwakeSystem: AwakeSystem<FUI5V5MapComponent>
    {
        public override void Awake(FUI5V5MapComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class FUI5V5MapComponentUpdateSystem: UpdateSystem<FUI5V5MapComponent>
    {
        public override void Update(FUI5V5MapComponent self)
        {
            self.Update();
        }
    }

    [ObjectSystem]
    public class FUI5V5MapComponentFixedUpdateSystem: FixedUpdateSystem<FUI5V5MapComponent>
    {
        public override void FixedUpdate(FUI5V5MapComponent self)
        {
            self.FixedUpdate();
        }
    }

    [ObjectSystem]
    public class FUI5V5MapComponentDestroySystem: DestroySystem<FUI5V5MapComponent>
    {
        public override void Destroy(FUI5V5MapComponent self)
        {
            self.Destroy();
        }
    }

    #endregion

    public class FUI5V5MapComponent: Component
    {
        #region 私有成员

        private FUI5V5Map m_Fui5V5Map;

        private CDComponent m_CDComponent;

        #endregion

        #region 公有成员

        private CDInfo m_QCDInfo;
        private CDInfo m_WCDInfo;
        private CDInfo m_ECDInfo;

        #endregion

        #region 生命周期函数

        public void Awake()
        {
            this.m_CDComponent = ETModel.Game.Scene.GetComponent<CDComponent>();
            this.m_Fui5V5Map = Game.Scene.GetComponent<FUIComponent>().Get(FUI5V5Map.UIPackageName) as FUI5V5Map;
            long playerUnitId = UnitComponent.Instance.MyUnit.Id;
            this.m_QCDInfo = m_CDComponent.AddCDData(playerUnitId, "Q", 0, info =>
            {
                if (info.Result)
                {
                    m_Fui5V5Map.SkillQ_CDInfo.visible = false;
                    m_Fui5V5Map.SkillQ_Bar.Visible = false;
                    return;
                }

                m_Fui5V5Map.SkillQ_CDInfo.text =
                        ((int) Math.Ceiling((double) (info.RemainCDLength) / 1000))
                        .ToString();
                m_Fui5V5Map.SkillQ_CDInfo.visible = true;
                m_Fui5V5Map.SkillQ_Bar.self.value = 100 * (info.RemainCDLength / info.Interval);
                m_Fui5V5Map.SkillQ_Bar.Visible = true;
            });
            this.m_WCDInfo = m_CDComponent.AddCDData(playerUnitId, "W", 0, info =>
            {
                if (info.Result)
                {
                    m_Fui5V5Map.SkillW_CDInfo.visible = false;
                    m_Fui5V5Map.SkillW_Bar.Visible = false;
                    return;
                }

                m_Fui5V5Map.SkillW_CDInfo.text =
                        ((int) Math.Ceiling((double) (info.RemainCDLength) / 1000))
                        .ToString();
                m_Fui5V5Map.SkillW_CDInfo.visible = true;
                m_Fui5V5Map.SkillW_Bar.self.value = 100 * (info.RemainCDLength / info.Interval);
                m_Fui5V5Map.SkillW_Bar.Visible = true;
            });
            this.m_ECDInfo = m_CDComponent.AddCDData(playerUnitId, "E", 0, info =>
            {
                if (info.Result)
                {
                    m_Fui5V5Map.SkillE_CDInfo.visible = false;
                    m_Fui5V5Map.SkillE_Bar.Visible = false;
                    return;
                }

                m_Fui5V5Map.SkillE_CDInfo.text =
                        ((int) Math.Ceiling((double) (info.RemainCDLength) / 1000))
                        .ToString();
                m_Fui5V5Map.SkillE_CDInfo.visible = true;
                m_Fui5V5Map.SkillE_Bar.self.value = 100 * (info.RemainCDLength / info.Interval);
                m_Fui5V5Map.SkillE_Bar.Visible = true;
            });
        }

        public void Update()
        {
            //此处填写Update逻辑
            if (!m_CDComponent.GetCDResult(UnitComponent.Instance.MyUnit.Id, "Q"))
            {
                this.m_Fui5V5Map.SkillQ_CDInfo.text =
                        ((int) Math.Ceiling((double) (this.m_QCDInfo.RemainCDLength) / 1000))
                        .ToString();
                this.m_Fui5V5Map.SkillQ_Bar.self.value = 100 * (m_QCDInfo.RemainCDLength * 1f / m_QCDInfo.Interval);
            }

            if (!m_CDComponent.GetCDResult(UnitComponent.Instance.MyUnit.Id, "W"))
            {
                this.m_Fui5V5Map.SkillW_CDInfo.text =
                        ((int) Math.Ceiling((double) (this.m_WCDInfo.RemainCDLength) / 1000))
                        .ToString();
                this.m_Fui5V5Map.SkillW_Bar.self.value = 100 * (m_WCDInfo.RemainCDLength * 1f / m_WCDInfo.Interval);
            }

            if (!m_CDComponent.GetCDResult(UnitComponent.Instance.MyUnit.Id, "E"))
            {
                this.m_Fui5V5Map.SkillE_CDInfo.text =
                        ((int) Math.Ceiling((double) (this.m_ECDInfo.RemainCDLength) / 1000))
                        .ToString();
                this.m_Fui5V5Map.SkillE_Bar.self.value = 100 * (m_ECDInfo.RemainCDLength * 1f / m_ECDInfo.Interval);
            }
        }

        public void FixedUpdate()
        {
            //此处填写FixedUpdate逻辑
        }

        public void Destroy()
        {
            //此处填写Destroy逻辑
        }

        public override void Dispose()
        {
            if (IsDisposed)
                return;
            base.Dispose();
            //此处填写释放逻辑,但涉及Entity的操作，请放在Destroy中
        }

        #endregion
    }
}