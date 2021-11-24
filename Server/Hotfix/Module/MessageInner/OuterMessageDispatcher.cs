

using System;
using System.IO;

namespace ET
{
	public class OuterMessageDispatcher: IMessageDispatcher
	{
		public void Dispatch(Session session, MemoryStream memoryStream)
		{
			ushort opcode = BitConverter.ToUInt16(memoryStream.GetBuffer(), Packet.KcpOpcodeIndex);
			Type type = OpcodeTypeComponent.Instance.GetType(opcode);
			object message = MessageSerializeHelper.DeserializeFrom(opcode, type, memoryStream);

			if (message is IResponse response)
			{
				session.OnRead(opcode, response);
				return;
			}

			OpcodeHelper.LogMsg(session.DomainZone(), opcode, message);
			
			DispatchAsync(session, opcode, message).Coroutine();
		}
		
		public async ETVoid DispatchAsync(Session session, ushort opcode, object message)
		{
			// 根据消息接口判断是不是Actor消息，不同的接口做不同的处理
			switch (message)
			{
				//case IActorLocationRequest actorLocationRequest: // gate session收到actor rpc消息，先向actor 发送rpc请求，再将请求结果返回客户端
				//{
				//	long unitId = session.GetComponent<SessionPlayerComponent>().Player.UnitId;
				//	int rpcId = actorLocationRequest.RpcId; // 这里要保存客户端的rpcId
				//	long instanceId = session.InstanceId;
				//	IResponse response = await ActorLocationSenderComponent.Instance.Call(unitId, actorLocationRequest);
				//	response.RpcId = rpcId;
				//	// session可能已经断开了，所以这里需要判断
				//	if (session.InstanceId == instanceId)
				//	{
				//		session.Reply(response);
				//	}
				//	break;
				//}
				//case IActorLocationMessage actorLocationMessage:
				//{
				//	long unitId = session.GetComponent<SessionPlayerComponent>().Player.UnitId;
				//	ActorLocationSenderComponent.Instance.Send(unitId, actorLocationMessage);
				//	break;
				//}
					//目前只有这个接口需要直接转发.所以偷懒不加新的接口类型
				case C2L_RoomListInLobby c2L_RoomListInLobby:
					{
						int rpcId = c2L_RoomListInLobby.RpcId; // 这里要保存客户端的rpcId
						long instanceId = session.InstanceId;
						IResponse response = await ActorMessageSenderComponent.Instance.Call(StartSceneConfigCategory.Instance.GetBySceneName(1, "RoomManager").InstanceId, c2L_RoomListInLobby);
						response.RpcId = rpcId;
						// session可能已经断开了，所以这里需要判断
						if (session.InstanceId == instanceId)
						{
							session.Reply(response);
						}
						break;
					}
				case IRoomRequest roomRequest:
					{
                        if (session.GetComponent<RoomStateOnGateComponent>() == null)
                        {
							Log.Error("RoomStateOnGateComponent null");
							session.Reply(ActorHelper.CreateResponse(roomRequest,ErrorCode.ERR_RoomNotExist));
							return;
                        }
						int rpcId = roomRequest.RpcId; // 这里要保存客户端的rpcId
						long instanceId = session.InstanceId;
						IResponse response = await ActorMessageSenderComponent.Instance.Call(session.GetComponent<RoomStateOnGateComponent>().RoomPlayerActorId, roomRequest);
						response.RpcId = rpcId;
						// session可能已经断开了，所以这里需要判断
						if (session.InstanceId == instanceId)
						{
							session.Reply(response);
							//此处为偷懒.
                            if (response is L2C_LeaveRoomLobby)
                            {
								session.RemoveComponent<RoomStateOnGateComponent>();
                            }
						}
						break;
					}
				case IRoomMessage roomMessage:
                    if (session.GetComponent<RoomStateOnGateComponent>() == null)
					{
						return;
					}
                    ActorMessageSenderComponent.Instance.Send(session.GetComponent<RoomStateOnGateComponent>().RoomPlayerActorId, roomMessage);
					break;
				//case IActorRequest actorRequest:  // 分发IActorRequest消息，目前没有用到，需要的自己添加
				//{
				//	break;
				//}
				//case IActorMessage actorMessage:  // 分发IActorMessage消息，目前没有用到，需要的自己添加
				//{
				//	break;
				//}

				default:
				{
					// 非Actor消息
					MessageDispatcherComponent.Instance.Handle(session, opcode, message);
					break;
				}
			}
		}
	}
}
