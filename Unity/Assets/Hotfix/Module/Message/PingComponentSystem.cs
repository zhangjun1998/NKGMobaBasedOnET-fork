using System;
using ET.EventType;
using UnityEngine;

namespace ET
{
    public class FinishEnterMap_BeginPing : AEvent<EventType.FinishEnterMap>
    {
        protected override async ETTask Run(FinishEnterMap a)
        {
            Game.Scene.GetComponent<PlayerComponent>().GateSession.GetComponent<PingComponent>().PingAsync()
                .Coroutine();

            await ETTask.CompletedTask;
        }
    }

    [ObjectSystem]
    public class PingComponentDestroySystem : DestroySystem<PingComponent>
    {
        public override void Destroy(PingComponent self)
        {
            self.C2GPingValue = default;
        }
    }

    public static class PingComponentUtilities
    {
        public static async ETVoid PingAsync(this PingComponent self)
        {
            Session session = self.GetParent<Session>();
            long instanceId = self.InstanceId;

            while (true)
            {
                if (self.InstanceId != instanceId)
                {
                    return;
                }

                try
                {
                    long clientNow_C2GSend = TimeHelper.ClientNow();

                    G2C_Ping responseFromGate = await session.Call(self.C2G_Ping) as G2C_Ping;

                    if (self.InstanceId != instanceId)
                    {
                        return;
                    }

                    long clientNow_C2MSend = TimeHelper.ClientNow();

                    self.C2GPingValue =
                        (uint) Mathf.Clamp(clientNow_C2MSend - clientNow_C2GSend - (long) (Time.deltaTime * 1000), 0.0f,
                            999.0f);

                    M2C_Ping responseFromMap = await session.Call(self.C2M_Ping) as M2C_Ping;

                    self.C2MPingValue =
                        (uint) Mathf.Clamp(TimeHelper.ClientNow() - clientNow_C2MSend - (long) (Time.deltaTime * 1000),
                            0f, 999.0f);

                    //TODO 这里是只有C2M的ping发生变化才发送通知
                    Game.EventSystem.Publish(new EventType.PingChange()
                        {
                            C2GPing = self.C2GPingValue,
                            C2MPing = self.C2MPingValue,
                            ZoneScene = self.DomainScene()
                        })
                        .Coroutine();

                    Game.TimeInfo.ServerMinusClientTime = responseFromGate.Time +
                        (clientNow_C2MSend - clientNow_C2GSend) / 2 - clientNow_C2MSend;

                    await TimerComponent.Instance.WaitAsync(2000);
                }
                catch (RpcException e)
                {
                    // session断开导致ping rpc报错，记录一下即可，不需要打成error
                    Log.Info($"ping error: {self.Id} {e.Error}");
                    return;
                }
                catch (Exception e)
                {
                    Log.Error($"ping error: \n{e}");
                }
            }
        }
    }
}