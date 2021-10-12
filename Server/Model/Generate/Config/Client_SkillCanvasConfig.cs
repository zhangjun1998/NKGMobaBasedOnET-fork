using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class Client_SkillCanvasConfigCategory : ProtoObject
    {
        public static Client_SkillCanvasConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, Client_SkillCanvasConfig> dict = new Dictionary<int, Client_SkillCanvasConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<Client_SkillCanvasConfig> list = new List<Client_SkillCanvasConfig>();
		
        public Client_SkillCanvasConfigCategory()
        {
            Instance = this;
        }
		
		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            foreach (Client_SkillCanvasConfig config in list)
            {
                this.dict.Add(config.Id, config);
            }
            list.Clear();
            this.EndInit();
        }
		
        public Client_SkillCanvasConfig Get(int id)
        {
            this.dict.TryGetValue(id, out Client_SkillCanvasConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (Client_SkillCanvasConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, Client_SkillCanvasConfig> GetAll()
        {
            return this.dict;
        }

        public Client_SkillCanvasConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class Client_SkillCanvasConfig: ProtoObject, IConfig
	{
		[ProtoMember(1, IsRequired  = true)]
		public int Id { get; set; }
		[ProtoMember(2, IsRequired  = true)]
		public long NPBehaveId { get; set; }
		[ProtoMember(3, IsRequired  = true)]
		public long BelongToSkillId { get; set; }
		[ProtoMember(4, IsRequired  = true)]
		public string SkillConfigName { get; set; }


		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            this.EndInit();
        }
	}
}
