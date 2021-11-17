﻿using System.Collections.Generic;

namespace ET
{
	public class GateSessionKeyComponent : Entity
	{
		private readonly Dictionary<long, long> sessionKey = new Dictionary<long, long>();
		
		public void Add(long key, long playerid)
		{
			this.sessionKey.Add(key, playerid);
			this.TimeoutRemoveKey(key).Coroutine();
		}

		public long Get(long key)
		{
			long account;
			this.sessionKey.TryGetValue(key, out account);
			return account;
		}

		public void Remove(long key)
		{
			this.sessionKey.Remove(key);
		}

		private async ETVoid TimeoutRemoveKey(long key)
		{
			await TimerComponent.Instance.WaitAsync(20000);
			this.sessionKey.Remove(key);
		}
	}
}
