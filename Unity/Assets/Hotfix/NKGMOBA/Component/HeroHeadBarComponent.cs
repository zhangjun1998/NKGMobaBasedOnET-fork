//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月31日 14:21:00
//------------------------------------------------------------

using ETHotfix;
using ETModel;
using FairyGUI;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class HeroHeadBarComponentAwakeSystem: AwakeSystem<HeroHeadBarComponent, Unit, FUI>
    {
        public override void Awake(HeroHeadBarComponent self, Unit m_Hero, FUI m_HeadBar)
        {
            self.Awake(m_Hero, m_HeadBar);
        }
    }

    [ObjectSystem]
    public class HeroHeadBarComponentUpdateSystem: UpdateSystem<HeroHeadBarComponent>
    {
        public override void Update(HeroHeadBarComponent self)
        {
            self.Update();
        }
    }

    /// <summary>
    /// 头部血条组件，负责血条的密度以及血条与人物的同步
    /// </summary>
    public class HeroHeadBarComponent: Component
    {
        public Unit Hero;
        private FUIHeadBar m_HeadBar;
        private Vector2 m_Hero2Screen;
        private Vector2 m_HeadBarScreenPos;
        private Renderer m_HeadBarGapRender;
        private static readonly int UVStart = Shader.PropertyToID("UVStart");
        private static readonly int UVFactor = Shader.PropertyToID("UVFactor");
        private static readonly int PerSplitWidth = Shader.PropertyToID("PerSplitWidth");

        public void Awake(Unit hero, FUI headBar)
        {
            this.Hero = hero;
            UnitAttributesDataComponent unitAttributesDataComponent = hero.GetComponent<UnitAttributesDataComponent>();
            this.m_HeadBar = headBar as FUIHeadBar;
            this.m_HeadBar.Bar_HP.self.value = unitAttributesDataComponent.GetAttribute(NumericType.MaxHp);
            this.m_HeadBar.Bar_MP.self.max = unitAttributesDataComponent.GetAttribute(NumericType.MaxMp);
            this.m_HeadBar.Bar_MP.self.value = unitAttributesDataComponent.GetAttribute(NumericType.MaxMp);
            
            this.m_HeadBarGapRender = this.m_HeadBar.Img_Gap.displayObject.gameObject.GetComponent<Renderer>();
            
            //因为FGUI的GImage并不会在当前帧构建顶点数据，所以只能使用监听的方式
            m_HeadBar.Img_Gap.displayObject.graphics.meshModifier += this.InitHPBarGap;
        }

        public void Update()
        {
            // 游戏物体的世界坐标转屏幕坐标
            this.m_Hero2Screen =
                    Camera.main.WorldToScreenPoint(new Vector3(this.Hero.Position.x, this.Hero.Position.y, this.Hero.Position.z));

            // 屏幕坐标转FGUI全局坐标
            this.m_HeadBarScreenPos.x = m_Hero2Screen.x;
            this.m_HeadBarScreenPos.y = Screen.height - m_Hero2Screen.y;

            // FGUI全局坐标转头顶血条本地坐标
            this.m_HeadBar.GObject.position = GRoot.inst.GlobalToLocal(m_HeadBarScreenPos);

            // 血条本地坐标修正
            this.m_HeadBar.GObject.x -= GetOffsetX(m_HeadBarScreenPos);
            this.m_HeadBar.GObject.y -= 180;
        }

        /// <summary>
        /// 得到偏移的x
        /// </summary>
        /// <param name="barPos">血条的屏幕坐标</param>
        /// <returns></returns>
        private float GetOffsetX(Vector2 barPos)
        {
            float final = 100 + (Screen.width / 2.0f - barPos.x) * 0.05f;
            return final;
        }

        private void InitHPBarGap()
        {
            SetDensityOfBar(Hero.GetComponent<UnitAttributesDataComponent>()
                    .GetAttribute(NumericType.MaxHp));
            m_HeadBar.Img_Gap.displayObject.graphics.meshModifier -= InitHPBarGap;
        }

        public void SetDensityOfBar(float maxHP)
        {
            float actual = 0;
            if (maxHP % 100 - 0 <= 0.1f)
            {
                actual = maxHP / 100 + 1;
            }
            else
            {
                actual = maxHP / 100 + 2;
            }

            this.m_HeadBar.Bar_HP.self.max = maxHP;

            m_HeadBar.Img_Gap.material = ResourcesComponent.Instance.LoadAsset<GameObject>(ABPathUtilities.GetMaterialPath("FGUIMaterials"))
                    .GetTargetObjectFromRC<Material>("Mat_LifeBarGap");

            Vector2[] uv = m_HeadBar.Img_Gap.displayObject.gameObject.GetComponent<MeshFilter>().sharedMesh.uv;

            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
            
            this.m_HeadBarGapRender.GetPropertyBlock(materialPropertyBlock);
            materialPropertyBlock.SetFloat(UVStart, uv[0].x);
            materialPropertyBlock.SetFloat(UVFactor, 1 / (uv[2].x - uv[0].x));
            materialPropertyBlock.SetFloat(PerSplitWidth, 100 / (maxHP / 100));
            this.m_HeadBarGapRender.SetPropertyBlock(materialPropertyBlock);
        }

        public override void Dispose()
        {
            if (this.IsDisposed) return;
            base.Dispose();
            this.Hero = null;
            m_HeadBar = null;
            m_Hero2Screen = Vector2.zero;
            this.m_HeadBarScreenPos = Vector2.zero;
            this.m_HeadBarGapRender = null;
        }
    }
}