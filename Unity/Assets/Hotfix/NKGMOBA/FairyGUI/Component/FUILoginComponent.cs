//此文件格式由工具自动生成

using ETModel;
using FairyGUI;

namespace ETHotfix
{
    #region System

    [ObjectSystem]
    public class FUILoginComponentAwakeSystem: AwakeSystem<FUILoginComponent>
    {
        public override void Awake(FUILoginComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class FUILoginComponentUpdateSystem: UpdateSystem<FUILoginComponent>
    {
        public override void Update(FUILoginComponent self)
        {
            self.Update();
        }
    }

    [ObjectSystem]
    public class FUILoginComponentFixedUpdateSystem: FixedUpdateSystem<FUILoginComponent>
    {
        public override void FixedUpdate(FUILoginComponent self)
        {
            self.FixedUpdate();
        }
    }

    [ObjectSystem]
    public class FUILoginComponentDestroySystem: DestroySystem<FUILoginComponent>
    {
        public override void Destroy(FUILoginComponent self)
        {
            self.Destroy();
        }
    }

    #endregion

    public class FUILoginComponent: Component
    {
        #region 私有成员

        private FUILogin m_FUILogin;
        private TypingEffect m_m_TypingEffect;
        
        
        #endregion

        #region 公有成员

        #endregion

        #region 生命周期函数

        public void Awake()
        {
            //此处填写Awake逻辑
            this.m_FUILogin = Game.Scene.GetComponent<FUIComponent>().Get(FUILogin.UIPackageName) as FUILogin;
            m_m_TypingEffect = new TypingEffect(m_FUILogin.Tex_OpenDeclaration);
            m_m_TypingEffect.Start();
            m_m_TypingEffect.PrintAll(0.01f);
        }

        public void Update()
        {
            //此处填写Update逻辑
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
            m_m_TypingEffect.Cancel();
            m_m_TypingEffect = null;
            this.m_FUILogin = null;
        }

        #endregion
    }
}