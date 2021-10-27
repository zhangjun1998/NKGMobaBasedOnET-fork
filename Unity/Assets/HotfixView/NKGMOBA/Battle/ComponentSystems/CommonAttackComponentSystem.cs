using Animancer;
using ET.EventType;

namespace ET
{
    public class CommonAttackComponentAwakeSystem : AwakeSystem<CommonAttackComponent>
    {
        public override void Awake(CommonAttackComponent self)
        {
            //此处填写Awake逻辑
            self.m_AnimationComponent = self.GetParent<Unit>().GetComponent<AnimationComponent>();
            self.m_StackFsmComponent = self.GetParent<Unit>().GetComponent<StackFsmComponent>();
            self.m_MouseTargetSelectorComponent = self.GetParent<Unit>().BelongToRoom.GetComponent<MouseTargetSelectorComponent>();
            self.m_UserInputComponent = Game.Scene.GetComponent<UserInputComponent>();
        }
    }


    public class CommonAttackComponentUpdateSystem : UpdateSystem<CommonAttackComponent>
    {
        public override void Update(CommonAttackComponent self)
        {
            //此处填写Update逻辑
            if (self.m_UserInputComponent.RightMouseDown && self.m_MouseTargetSelectorComponent.TargetUnit != null)
            {
                if (self.m_MouseTargetSelectorComponent.TargetUnit.GetComponent<B2S_RoleCastComponent>()
                        .GetRoleCastToTarget(self.GetParent<Unit>()) ==
                    RoleCast.Adverse)
                {
                    self.m_CachedUnit =self.m_MouseTargetSelectorComponent.TargetUnit;
                    Game.Scene.GetComponent<PlayerComponent>().GateSession.Send(new C2M_CommonAttack()
                        {TargetUnitId = self.m_CachedUnit.Id});
                }
            }
        }
    }

    public class CancelAttackFromFsm : AEvent<EventType.CancelAttackFromFSM>
    {
        protected override async ETTask Run(CancelAttackFromFSM a)
        {
            a.Unit.GetComponent<CommonAttackComponent>().CancelCommonAttack();
            await ETTask.CompletedTask;
        }
    }
    
    public static class CommonAttackComponentSystem
    {
        public static async ETVoid CommonAttackStart(this CommonAttackComponent self, Unit targetUnit)
        {
            self.CancellationTokenSource?.Cancel();
            self.CancellationTokenSource = new ETCancellationToken();

            //转向目标Unit
            self.GetParent<Unit>().GetComponent<TurnComponent>().Turn(targetUnit.Position);
            self.m_StackFsmComponent.ChangeState<CommonAttackState>(StateTypes.CommonAttack, "Attack", 1);

            if (await self.CommonAttack_Internal(targetUnit, self.CancellationTokenSource))
            {
                self.CancellationTokenSource = null;
                self.m_StackFsmComponent.RemoveState("Attack");
            }
        }

        private static async ETTask<bool> CommonAttack_Internal(this CommonAttackComponent self, Unit targetUnit,
            ETCancellationToken cancellationTokenSource)
        {
            UnitAttributesDataComponent unitAttributesDataComponent =
                self.GetParent<Unit>().GetComponent<UnitAttributesDataComponent>();
            float attackPre = unitAttributesDataComponent.UnitAttributesNodeDataBase.OriAttackPre /
                              (1 + unitAttributesDataComponent.GetAttribute(NumericType.AttackSpeedAdd));
            float attackSpeed = unitAttributesDataComponent.GetAttribute(NumericType.AttackSpeed);

            //这里假设诺手原始攻击动画0.32s是动画攻击奏效点
            float animationAttackPoint = 0.32f;

            float animationSpeed = animationAttackPoint / attackPre;
            //播放动画，如果动画播放完成还不能进行下一次普攻，则播放空闲动画
            self.m_AnimationComponent.PlayAnimAndReturnIdelFromStart(StateTypes.CommonAttack, speed: animationSpeed, fadeMode: FadeMode.FromStart);

            Game.Scene.GetComponent<SoundComponent>().PlayClip("Darius/Sound_Darius_NormalAttack", 0.4f).Coroutine();

            return await TimerComponent.Instance.WaitAsync((long) (1 / attackSpeed * 1000), cancellationTokenSource);
        }

        public static void CancelCommonAttack(this CommonAttackComponent self)
        {
            self.CancellationTokenSource?.Cancel();
            self.CancellationTokenSource = null;
            self.m_StackFsmComponent.RemoveState("Attack");
        }
    }
}