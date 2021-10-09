//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年10月21日 15:15:48
//------------------------------------------------------------

using System.Collections.Generic;
using ET.EventType;
using NPBehave;
using UnityEngine;

namespace ET
{
    public class CommonAttackComponentUpdateSystem : FixedUpdateSystem<CommonAttackComponent>
    {
        public override void FixedUpdate(CommonAttackComponent self)
        {
            self.FixedUpdate();
        }
    }

    public class CancelAttackFromFsm : AEvent<EventType.CancelAttackFromFSM>
    {
        protected override async ETTask Run(CancelAttackFromFSM a)
        {
            if (a.ResetAttackTarget)
            {
                a.Unit.GetComponent<CommonAttackComponent>().CancelCommonAttack();
            }
            else
            {
                a.Unit.GetComponent<CommonAttackComponent>().CancelCommonAttackWithOutResetTarget();
            }

            await ETTask.CompletedTask;
        }
    }

    public static class CommonAttackComponentUtilities
    {
        public static void SetAttackTarget(this CommonAttackComponent self, Unit targetUnit)
        {
            if (targetUnit == null)
            {
                Log.Error("普攻组件接收到的targetUnit为null");
                return;
            }

            if (targetUnit.GetComponent<B2S_RoleCastComponent>().GetRoleCastToTarget(self.GetParent<Unit>()) ==
                RoleCast.Adverse)
            {
                if (self.CachedUnitForAttack != targetUnit)
                {
                    self.CancelCommonAttack();
                }

                self.CachedUnitForAttack = targetUnit;

                self.m_StackFsmComponent.ChangeState<CommonAttackState>(StateTypes.CommonAttack, "CommonAttack", 1);
            }
        }

        private static async ETVoid StartCommonAttack(this CommonAttackComponent self)
        {
            self.CancellationTokenSource?.Cancel();
            self.CancellationTokenSource = new ETCancellationToken();
            //如果有要执行攻击流程替换的内容，就执行替换流程
            if (self.HasAttackReplaceInfo())
            {
                Unit unit = self.GetParent<Unit>();

                NP_RuntimeTree npRuntimeTree = unit.GetComponent<NP_RuntimeTreeManager>()
                    .GetTreeByRuntimeID(self.AttackReplaceNPTreeId);
                Blackboard blackboard = npRuntimeTree.GetBlackboard();

                blackboard.Set(self.AttackReplaceBB.BBKey, true);
                blackboard.Set(self.CancelAttackReplaceBB.BBKey, false);

                blackboard.Set("NormalAttackUnitIds", new List<long>() {self.CachedUnitForAttack.Id});

                CDInfo commonAttackCDInfo = CDComponent.Instance.GetCDData(unit.Id, "CommonAttack");
                await TimerComponent.Instance.WaitAsync(commonAttackCDInfo.Interval, self.CancellationTokenSource);
            }
            else
            {
                await self.CommonAttack_Internal();
            }

            //此次攻击完成
            self.CancellationTokenSource = null;
        }

        private static async ETTask CommonAttack_Internal(this CommonAttackComponent self)
        {
            Unit unit = self.GetParent<Unit>();
            MessageHelper.BroadcastToRoom(unit.BelongToRoom, new M2C_CommonAttack()
            {
                AttackCasterId = unit.Id, TargetUnitId = self.CachedUnitForAttack.Id, CanAttack = true
            });
            UnitAttributesDataComponent heroDataComponent = unit.GetComponent<UnitAttributesDataComponent>();
            float attackPre = heroDataComponent.UnitAttributesNodeDataBase.OriAttackPre /
                              (1 + heroDataComponent.GetAttribute(NumericType.AttackSpeedAdd));
            float attackSpeed = heroDataComponent.GetAttribute(NumericType.AttackSpeed);

            //播放动画，如果动画播放完成还不能进行下一次普攻，则播放空闲动画
            if (!await TimerComponent.Instance.WaitAsync((long) (attackPre * 1000), self.CancellationTokenSource))
            {
                return;
            }

            DamageData damageData = ReferencePool.Acquire<DamageData>().InitData(
                BuffDamageTypes.PhysicalSingle | BuffDamageTypes.CommonAttack,
                heroDataComponent.GetAttribute(NumericType.Attack), unit, self.CachedUnitForAttack);

            unit.GetComponent<CastDamageComponent>().BaptismDamageData(damageData);
            float finalDamage = self.CachedUnitForAttack.GetComponent<ReceiveDamageComponent>()
                .BaptismDamageData(damageData);

            if (finalDamage >= 0)
            {
                self.CachedUnitForAttack.GetComponent<UnitAttributesDataComponent>().NumericComponent
                    .ApplyChange(NumericType.Hp, -finalDamage);

                BattleEventSystem battleEventSystem = unit.BelongToRoom.GetComponent<BattleEventSystem>();

                //抛出伤害事件，需要监听伤害的buff（比如吸血buff）需要监听此事件
                battleEventSystem.Run($"ExcuteDamage{unit.Id}", damageData);
                //抛出受伤事件，需要监听受伤的Buff（例如反甲）需要监听此事件
                battleEventSystem.Run($"TakeDamage{self.CachedUnitForAttack.Id}", damageData);
            }

            CDComponent.Instance.TriggerCD(unit.Id, "CommonAttack");
            CDInfo commonAttackCDInfo = CDComponent.Instance.GetCDData(unit.Id, "CommonAttack");
            commonAttackCDInfo.Interval = (long) (1 / attackSpeed - attackPre) * 1000;

            List<NP_RuntimeTree> targetSkillCanvas =
                unit.GetComponent<SkillCanvasManagerComponent>().GetSkillCanvas(10001);
            foreach (var skillCanva in targetSkillCanvas)
            {
                skillCanva.GetBlackboard().Set("CastNormalAttack", true);
                skillCanva.GetBlackboard().Set("NormalAttackUnitIds", new List<long>() {self.CachedUnitForAttack.Id});
            }

            await TimerComponent.Instance.WaitAsync(commonAttackCDInfo.Interval, self.CancellationTokenSource);
        }

        public static void FixedUpdate(this CommonAttackComponent self)
        {
            Unit unit = self.GetParent<Unit>();

            if (unit.GetComponent<StackFsmComponent>().GetCurrentFsmState().StateTypes == StateTypes.CommonAttack)
            {
                if (self.CachedUnitForAttack != null && !self.CachedUnitForAttack.IsDisposed)
                {
                    Vector3 selfUnitPos = unit.Position;
                    double distance = Vector3.Distance(selfUnitPos, self.CachedUnitForAttack.Position);
                    float attackRange = unit.GetComponent<UnitAttributesDataComponent>()
                        .NumericComponent[NumericType.AttackRange] / 100;

                    //目标距离大于当前攻击距离会先进行寻路，这里的1.75为175码
                    if (distance - attackRange >= 0.1f)
                    {
                        if (!CDComponent.Instance.GetCDResult(unit.Id, "MoveToAttack")) return;
                        CDComponent.Instance.TriggerCD(unit.Id, "MoveToAttack");

                        CommonAttackState commonAttackState = ReferencePool.Acquire<CommonAttackState>();
                        commonAttackState.SetData(StateTypes.CommonAttack, "CommonAttack", 1);

                        unit.NavigateTodoSomething(self.CachedUnitForAttack.Position, 1.75f, commonAttackState)
                            .Coroutine();
                    }
                    else
                    {
                        //目标不为空，且处于攻击状态，且上次攻击已完成或取消
                        if ((self.CancellationTokenSource == null || self.CancellationTokenSource.IsCancel()))
                        {
                            if (distance - attackRange <= 0.1f &&
                                CDComponent.Instance.GetCDResult(unit.Id, "CommonAttack"))
                                self.StartCommonAttack().Coroutine();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 取消攻击但不重置攻击对象
        /// </summary>
        public static void CancelCommonAttackWithOutResetTarget(this CommonAttackComponent self)
        {
            self.CancellationTokenSource?.Cancel();
            self.CancellationTokenSource = null;

            if (self.HasCancelAttackReplaceInfo())
            {
                Unit unit = self.GetParent<Unit>();
                unit.GetComponent<NP_RuntimeTreeManager>().GetTreeByRuntimeID(self.CancelAttackReplaceNPTreeId)
                    .GetBlackboard()
                    .Set(self.AttackReplaceBB.BBKey, false);
                unit.GetComponent<NP_RuntimeTreeManager>().GetTreeByRuntimeID(self.CancelAttackReplaceNPTreeId)
                    .GetBlackboard()
                    .Set(self.CancelAttackReplaceBB.BBKey, true);
            }

            MessageHelper.BroadcastToRoom(self.GetParent<Unit>().BelongToRoom,
                new M2C_CancelCommonAttack() {TargetUnitId = self.GetParent<Unit>().Id});
        }

        /// <summary>
        /// 取消攻击并且重置攻击对象
        /// </summary>
        public static void CancelCommonAttack(this CommonAttackComponent self)
        {
            self.CancelCommonAttackWithOutResetTarget();
            self.CachedUnitForAttack = null;
        }
    }
}