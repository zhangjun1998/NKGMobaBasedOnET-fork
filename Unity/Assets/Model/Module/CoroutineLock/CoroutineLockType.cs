namespace ET
{
    public enum CoroutineLockType
    {
        None = 0,
        Location,                  // location进程上使用
        ActorLocationSender,       // ActorLocationSender中队列消息 
        Mailbox,                   // Mailbox中队列
        UnitId,                    // Map服务器上线下线时使用
        Room,                    // 和room有关的操作
        DB,
        Resources,

        Max, // 这个必须在最后
    }
}