using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class Server_SkillCanvasConfigCategory : ProtoObject
    {
        public static Server_SkillCanvasConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, Server_SkillCanvasConfig> dict = new Dictionary<int, Server_SkillCanvasConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<Server_SkillCanvasConfig> list = new List<Server_SkillCanvasConfig>();
		
        public Server_SkillCanvasConfigCategory()
        {
            Instance = this;
        }
		
		#if SERVER
		[ProtoAfterDeserialization]
        #endif
        public void AfterDeserialization()
        {
            foreach (Server_SkillCanvasConfig config in list)
            {
                this.dict.Add(config.Id, config);
            }
            list.Clear();
            this.EndInit();
        }
		
        public Server_SkillCanvasConfig Get(int id)
        {
            this.dict.TryGetValue(id, out Server_SkillCanvasConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (Server_SkillCanvasConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, Server_SkillCanvasConfig> GetAll()
        {
            return this.dict;
        }

        public Server_SkillCanvasConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class Server_SkillCanvasConfig: ProtoObject, IConfig
	{
		[ProtoMember(1, IsRequired  = true)]
		public int Id { get; set; }
		[ProtoMember(2, IsRequired  = true)]
		public long NPBehaveId { get; set; }
		[ProtoMember(3, IsRequired  = true)]
		public long BelongToSkillId { get; set; }


#if SERVER
		[ProtoAfterDeserialization]
#endif
        public void AfterDeserialization()
        {
            this.EndInit();
        }
	}
}
