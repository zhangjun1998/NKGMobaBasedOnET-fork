using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Runtime.Serialization;

namespace ET
{
	public class PlayerSystem : AwakeSystem<Player, string>
	{
		public override void Awake(Player self, string a)
		{
			self.Awake(a);
		}
	}

	public sealed class Player : Entity
	{
		public string Name { get;  set; }
		
		public long UnitId { get; set; }
        /// <summary>
        /// 所归属的阵营
        /// </summary>
        public Int32 camp { get; set; }
        public long GateSessionId { get; set; }

        public void Awake(string account)
		{
			this.Name = account;
		}
	}
}