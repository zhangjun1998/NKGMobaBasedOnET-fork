//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月14日 21:37:53
//------------------------------------------------------------

namespace ETModel
{
    [ObjectSystem]
    public class HeroDataComponentSystem: AwakeSystem<UnitAttributesDataComponent, long>
    {
        public override void Awake(UnitAttributesDataComponent self, long a)
        {
            //TODO 因为目前只有英雄的创建需求，所以直接指定10001
            self.UnitAttributesNodeDataBase = Game.Scene.GetComponent<UnitAttributesDataRepositoryComponent>()
                    .GetUnitAttributesDataById_DeepCopy<HeroAttributesNodeData>(10001, a);

            self.NumericComponent = self.Entity.GetComponent<NumericComponent>();

            self.NumericComponent.NumericDic[(int) NumericType.Level] = 1;
            self.NumericComponent.NumericDic[(int) NumericType.MaxLevel] = 18;

            self.NumericComponent.NumericDic[(int) NumericType.MaxHpBase] = self.UnitAttributesNodeDataBase.OriHP;
            self.NumericComponent.NumericDic[(int) NumericType.MaxHpAdd] = self.UnitAttributesNodeDataBase.GroHP;
            self.NumericComponent.NumericDic[(int) NumericType.MaxHp] = self.UnitAttributesNodeDataBase.OriHP + self.UnitAttributesNodeDataBase.GroHP;

            self.NumericComponent.NumericDic[(int) NumericType.Hp] = self.UnitAttributesNodeDataBase.OriHP + self.UnitAttributesNodeDataBase.GroHP;

            self.NumericComponent.NumericDic[(int) NumericType.MaxMpBase] = self.UnitAttributesNodeDataBase.OriMagicValue;
            self.NumericComponent.NumericDic[(int) NumericType.MaxMpAdd] = self.UnitAttributesNodeDataBase.GroMagicValue;
            self.NumericComponent.NumericDic[(int) NumericType.MaxMp] =
                    self.UnitAttributesNodeDataBase.OriMagicValue + self.UnitAttributesNodeDataBase.GroMagicValue;

            self.NumericComponent.NumericDic[(int) NumericType.AttackBase] = self.UnitAttributesNodeDataBase.OriAttackValue;
            self.NumericComponent.NumericDic[(int) NumericType.AttackAdd] = self.UnitAttributesNodeDataBase.GroAttackValue;
            self.NumericComponent.NumericDic[(int) NumericType.Attack] =
                    self.UnitAttributesNodeDataBase.OriAttackValue + self.UnitAttributesNodeDataBase.GroAttackValue;

            self.NumericComponent.NumericDic[(int) NumericType.SpeedBase] = self.UnitAttributesNodeDataBase.OriMoveSpeed;
            self.NumericComponent.NumericDic[(int) NumericType.SpeedAdd] = self.UnitAttributesNodeDataBase.GroMoveSpeed;
            self.NumericComponent.NumericDic[(int) NumericType.Speed] =
                    self.UnitAttributesNodeDataBase.OriMoveSpeed + self.UnitAttributesNodeDataBase.GroMoveSpeed;

            self.NumericComponent.NumericDic[(int) NumericType.ArmorBase] = self.UnitAttributesNodeDataBase.OriArmor;
            self.NumericComponent.NumericDic[(int) NumericType.ArmorAdd] = self.UnitAttributesNodeDataBase.GroArmor;
            self.NumericComponent.NumericDic[(int) NumericType.Armor] =
                    self.UnitAttributesNodeDataBase.OriArmor + self.UnitAttributesNodeDataBase.GroArmor;

            self.NumericComponent.NumericDic[(int) NumericType.MagicResistanceBase] = self.UnitAttributesNodeDataBase.OriMagicResistance;
            self.NumericComponent.NumericDic[(int) NumericType.MagicResistanceAdd] = self.UnitAttributesNodeDataBase.GroMagicResistance;
            self.NumericComponent.NumericDic[(int) NumericType.MagicResistance] =
                    self.UnitAttributesNodeDataBase.OriMagicResistance + self.UnitAttributesNodeDataBase.GroMagicResistance;

            self.NumericComponent.NumericDic[(int) NumericType.HPRecBase] = self.UnitAttributesNodeDataBase.OriHPRec;
            self.NumericComponent.NumericDic[(int) NumericType.HPRecAdd] = self.UnitAttributesNodeDataBase.GroHPRec;
            self.NumericComponent.NumericDic[(int) NumericType.HPRec] =
                    self.UnitAttributesNodeDataBase.OriHPRec + self.UnitAttributesNodeDataBase.GroHPRec;

            self.NumericComponent.NumericDic[(int) NumericType.MPRecBase] = self.UnitAttributesNodeDataBase.OriMagicRec;
            self.NumericComponent.NumericDic[(int) NumericType.MPRecAdd] = self.UnitAttributesNodeDataBase.GroMagicRec;
            self.NumericComponent.NumericDic[(int) NumericType.MPRec] =
                    self.UnitAttributesNodeDataBase.OriMagicRec + self.UnitAttributesNodeDataBase.GroMagicRec;

            self.NumericComponent.NumericDic[(int) NumericType.AttackSpeedBase] = self.UnitAttributesNodeDataBase.OriAttackSpeed;
            self.NumericComponent.NumericDic[(int) NumericType.AttackSpeedAdd] = self.UnitAttributesNodeDataBase.GroAttackSpeed;
            self.NumericComponent.NumericDic[(int) NumericType.AttackSpeed] =
                    self.UnitAttributesNodeDataBase.OriAttackSpeed + self.UnitAttributesNodeDataBase.GroAttackSpeed;

            self.NumericComponent.NumericDic[(int) NumericType.AttackSpeedIncome] = self.UnitAttributesNodeDataBase.OriAttackIncome;

            self.NumericComponent.NumericDic[(int) NumericType.CriticalStrikeProbability] =
                    self.UnitAttributesNodeDataBase.OriCriticalStrikeProbability;

            self.NumericComponent.NumericDic[(int) NumericType.CriticalStrikeHarm] = self.UnitAttributesNodeDataBase.OriCriticalStrikeHarm;

            //法术穿透
            self.NumericComponent.NumericDic[(int) NumericType.MagicPenetrationBase] = 0;
            self.NumericComponent.NumericDic[(int) NumericType.MagicPenetrationAdd] = 0;

            self.NumericComponent.NumericDic[(int) NumericType.AttackRangeBase] = self.UnitAttributesNodeDataBase.OriAttackRange;
            self.NumericComponent.NumericDic[(int) NumericType.AttackRangeAdd] = 0;
            self.NumericComponent.NumericDic[(int) NumericType.AttackRange] = self.UnitAttributesNodeDataBase.OriAttackRange;

            self.NumericComponent.InitOriNumerDic();
        }
    }

    /// <summary>
    /// 英雄数据组件，负责管理英雄数据
    /// </summary>
    public class UnitAttributesDataComponent: Component
    {
        public UnitAttributesNodeDataBase UnitAttributesNodeDataBase;

        public NumericComponent NumericComponent;

        public T GetAttributeDataAs<T>() where T : UnitAttributesNodeDataBase
        {
            return UnitAttributesNodeDataBase as T;
        }
        
        public float GetAttribute(NumericType numericType)
        {
            return NumericComponent[numericType];
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
            this.UnitAttributesNodeDataBase = null;
            NumericComponent = null;
        }
    }
}