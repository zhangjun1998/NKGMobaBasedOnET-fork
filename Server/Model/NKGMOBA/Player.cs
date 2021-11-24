using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;
using System;
using System.Runtime.Serialization;

namespace ET
{
	public class PlayerSystem : AwakeSystem<Player, string, int>
	{
		public override void Awake(Player self, string name,int lv)
		{
			self.Awake(name,lv);
		}
	}
	public sealed class Player : Entity
	{
		public string Name { get;  set; }
		public int Lv { get;  set; }
		public long UnitId { get; set; }
		/// <summary>
		/// 所归属的阵营
		/// </summary>
		public Int32 camp { get; set; }
		public long GateSessionId { get; set; }

        public void Awake(string name,int lv)
		{
			this.Name = name;
			this.Lv = lv;
		}
	}
}