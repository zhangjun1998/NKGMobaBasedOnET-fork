using System.Collections.Generic;
using UnityEngine;

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

        public struct PingChange
        {
            public Scene ZoneScene;
            public long Ping;
        }

        public struct AfterCreateZoneScene
        {
            public Scene ZoneScene;
        }

        public struct AfterCreateLoginScene
        {
            public Scene LoginScene;
        }

        public struct AppStartInitFinish
        {
            public Scene ZoneScene;
        }

        public struct LoginGateFinish
        {
            public Scene ZoneScene;
        }

        public struct LoginOrRegsiteFail
        {
            public string ErrorMessage;
            public Scene ZoneScene;
        }

        public struct LoadingBegin
        {
            public Scene Scene;
        }

        public struct LoadingFinish
        {
            public Scene Scene;
        }

        public struct EnterMapFinish
        {
            public Scene ZoneScene;
        }

        public struct AfterHeroCreate_CreateGo
        {
            public int HeroConfigId;
            public Unit Unit;
        }

        public struct AfterHeroSpilingCreate_CreateGO
        {
            public int HeroSpilingConfigId;
            public Unit Unit;
        }

        public struct AfterBulletCreate_CreateGO
        {
            public Unit FireUnit;
            public Unit BulletUnit;
            public Unit TargetUnit;
            public int BulletConfigId;
            public Vector3 BulletPos;
            public Vector3 BulletDir;
        }

        public struct MoveStart
        {
            public Unit Unit;
            public float Speed;
        }

        public struct MoveStop
        {
            public Unit Unit;
        }

        public struct NumericChange
        {
            public NumericComponent NumericComponent;
            public NumericType NumericType;
            public float Result;
        }

        public struct View_PrepareFire
        {
            public Scene DomainScene;
            public Unit FiredUnit;
            public float DirX;
            public float DirZ;
        }

        public struct View_RealFire
        {
            public Scene DomainScene;
            public Unit FiredUnit;
            public Unit TargetUnit;
        }

        public struct CreateRoom
        {
            public Scene DomainScene;
            public List<PlayerInfoRoom> PlayerInfoRooms;
        }

        public struct LoginLobbyFinish
        {
            public Scene DomainScene;
        }

        public struct JoinRoom
        {
            public Scene DomainScene;
            public List<PlayerInfoRoom> PlayerInfoRooms;
        }
        
        public struct CreatePlayerCard
        {
            public Scene DomainScene;
            public long PlayerId;
            public string PlayerName;
            public int Camp;
        }
        public struct RemovePlayerCard
        {
            public Scene DomainScene;
            public long PlayerId;
        }
        public struct LeaveRoom
        {
            public Scene DomainScene;
        }
        
        public struct SpriteReceiveDamage
        {
            public Unit Sprite;
            public float DamageValue;
        }

        public struct UnitChangeProperty
        {
            public Unit Sprite;
            public NumericType NumericType;
            public float FinalValue;
        }
        
        public struct SpriteResurrection
        {
            public Unit Sprite;
        }

        public struct BattleEnd
        {
            public Scene ZoneScene;
            public List<(string name, int score)> Scores;
        }
    }
}