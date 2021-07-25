//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月21日 18:40:49
//------------------------------------------------------------

using ETHotfix.NKGMOBA.Factory;
using ETModel;
using System.Linq;
using UnityEngine;

namespace ETHotfix.NKGMOBA.Battle
{
    [NumericWatcher(NumericType.Hp)]
    public class ChangeHP: INumericWatcher
    {
        public void Run(Unit unit, float value)
        {
            MessageHelper.Broadcast(unit.RoomPlayerArray,new M2C_SyncUnitAttribute() { UnitId = unit.Id, NumericType = (int) NumericType.Hp, FinalValue = value });
        }
    }

    [NumericWatcher(NumericType.Mp)]
    public class ChangeMP: INumericWatcher
    {
        public void Run(Unit unit, float value)
        {
            MessageHelper.Broadcast(unit.RoomPlayerArray, new M2C_SyncUnitAttribute() { UnitId = unit.Id, NumericType = (int) NumericType.Mp, FinalValue = value });
        }
    }

    [NumericWatcher(NumericType.AttackAdd)]
    public class ChangeAttackAdd: INumericWatcher
    {
        public void Run(Unit unit, float value)
        {
            MessageHelper.Broadcast(unit.RoomPlayerArray, new M2C_SyncUnitAttribute() { UnitId = unit.Id, NumericType = (int) NumericType.AttackAdd, FinalValue = value });
        }
    }

    [NumericWatcher(NumericType.Attack)]
    public class ChangeAttack: INumericWatcher
    {
        public void Run(Unit unit, float value)
        {
            MessageHelper.Broadcast(unit.RoomPlayerArray, new M2C_SyncUnitAttribute() { UnitId = unit.Id, NumericType = (int) NumericType.Attack, FinalValue = value });
        }
    }

    [NumericWatcher(NumericType.Speed)]
    public class ChangeSpeed: INumericWatcher
    {
        public void Run(Unit unit, float value)
        {
            MessageHelper.Broadcast(unit.RoomPlayerArray, new M2C_SyncUnitAttribute() { UnitId = unit.Id, NumericType = (int) NumericType.Speed, FinalValue = value });
        }
    }

    [Event(EventIdType.NumericApplyChangeValue)]
    public class SendDamageInfoToClient: AEvent<Entity, NumericType, float>
    {
        public override void Run(Entity unit, NumericType numberType, float changedValue)
        {
            MessageHelper.Broadcast((unit as Unit).RoomPlayerArray, new M2C_ChangeUnitAttribute() { UnitId = unit.Id, NumericType = (int) numberType, ChangeValue = changedValue });
        }
    }

    /// <summary>
    /// 向客户端发送事件，一般为特效表现
    /// long:来自Unit的ID
    /// long:归属Unit的ID
    /// string：要传给客户端的事件ID
    /// </summary>
    [Event(EventIdType.SendBuffInfoToClient)]
    public class SendBuffInfoToClient: AEvent<Unit,M2C_BuffInfo>
    {
        public override void Run(Unit unit,M2C_BuffInfo c)
        {
            MessageHelper.Broadcast(unit.RoomPlayerArray, c);
        }
    }

    /// <summary>
    /// 向客户端发送CD信息
    /// </summary>
    [Event(EventIdType.SendCDInfoToClient)]
    public class SendCDInfoToClient: AEvent<M2C_SyncCDData>
    {
        public override void Run(M2C_SyncCDData a)
        {
            var unit = UnitComponent.Instance.Get(a.UnitId);
            MessageHelper.Broadcast(unit.RoomPlayerArray, a);
        }
    }

    /// <summary>
    /// 向客户端发送黑板bool类型值
    /// </summary>
    [Event(EventIdType.SendNPBBValue_BoolToClient)]
    public class SendNPBBValue_BoolToClient: AEvent<M2C_SyncNPBehaveBoolData>
    {
        public override void Run(M2C_SyncNPBehaveBoolData a)
        {
            var unit = UnitComponent.Instance.Get(a.UnitId);
            MessageHelper.Broadcast(unit.RoomPlayerArray, a);
        }
    }

    [Event(EventIdType.MoveToRandomPos)]
    public class UnitPathComponentInvoke: AEvent<long, Vector3>
    {
        public override void Run(long a, Vector3 b)
        {
            UnitComponent.Instance.Get(a).GetComponent<UnitPathComponent>().CommonNavigate(b);
        }
    }

    [Event(EventIdType.RemoveCollider)]
    public class RemoveCollider: AEvent<long>
    {
        public override void Run(long a)
        {
            UnitFactory.RemoveColliderUnit(a);
        }
    }
}