namespace ET
{
    namespace EventType
    {
        public struct AppStart
        {
        }

        public struct ChangePosition
        {
            public Unit Unit;
        }

        public struct ChangeRotation
        {
            public Unit Unit;
        }

        public struct MoveStart
        {
            public Unit Unit;
            public float Speed;
        }

        public struct MoveStop
        {
            public Unit Unit;
            public float Speed;
        }

        public struct NumericChange
        {
            public NumericComponent NumericComponent;
            public NumericType NumericType;
            public float Result;
        }

        public struct SpriteDead
        {
            public Unit KillerSprite;
            public Unit DeadSprite;
        }

        public struct NumericApplyChangeValue
        {
            public long UnitId;
            public NumericType NumericType;
            public float ChangedValue;
        }
    }
}