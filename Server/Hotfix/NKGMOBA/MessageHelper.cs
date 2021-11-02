

using System.IO;

namespace ET
{
    public static class MessageHelper
    {
        public static void BroadcastToRoom(Entity entity, IMessage message)
        {
            var scene = entity.Domain;
            var players = scene.GetComponent<PlayerComponent>().GetAll();
            if (players == null) return;
            (ushort opcode, MemoryStream stream) = MessageSerializeHelper.MessageToStream(0, message);
            //根据opcode决定走特殊广播还是常规actor消息
            if (OpcodeTypeComponent.Instance.IsBroadcastMessage(opcode))
            {
                scene.GetComponent<BroadcastMsgComponent>().BroadcastToAll(stream);
            }
            else
            {
                foreach (Player player in players)
                {
                    ActorMessageSenderComponent.Instance.Send(player.GateSessionId, stream);
                }
            }
        }

        /// <summary>
        /// 发送协议给ActorLocation
        /// </summary>
        /// <param name="id">注册Actor的Id</param>
        /// <param name="message"></param>
        public static void SendToLocationActor(long id, IActorLocationMessage message)
        {
            ActorLocationSenderComponent.Instance.Send(id, message);
        }

        /// <summary>
        /// 发送协议给Actor
        /// </summary>
        /// <param name="actorId">注册Actor的InstanceId</param>
        /// <param name="message"></param>
        public static void SendActor(long actorId, IActorMessage message)
        {
            ActorMessageSenderComponent.Instance.Send(actorId, message);
        }

        /// <summary>
        /// 发送RPC协议给Actor
        /// </summary>
        /// <param name="actorId">注册Actor的InstanceId</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async ETTask<IActorResponse> CallActor(long actorId, IActorRequest message)
        {
            return await ActorMessageSenderComponent.Instance.Call(actorId, message);
        }

        /// <summary>
        /// 发送RPC协议给ActorLocation
        /// </summary>
        /// <param name="id">注册Actor的Id</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async ETTask<IActorResponse> CallLocationActor(long id, IActorLocationRequest message)
        {
            return await ActorLocationSenderComponent.Instance.Call(id, message);
        }
    }
}