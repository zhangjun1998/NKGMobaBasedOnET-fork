namespace ET
{
    // 不需要返回消息
    public interface IActorMessage: IMessage
    {
    }

    public interface IActorRequest: IRequest
    {
    }

    public interface IActorResponse: IResponse
    {
    }

    public interface IRoomMessage : IActorMessage
    {
    }

    public interface IRoomRequest : IActorRequest
    {
    }
    public interface IRoomResponse : IActorResponse
    {
    }
}