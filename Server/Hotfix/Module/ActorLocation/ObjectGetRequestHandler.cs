using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Location)]
    public class ObjectGetRequestHandler: AMRpcHandler<ObjectGetRequest, ObjectGetResponse>
    {
        protected override async ETTask Run(Session session, ObjectGetRequest request, ObjectGetResponse response, Action reply)
        {
            long instanceId = await Game.Scene.GetComponent<LocationComponent>().Get(request.Key);
            response.InstanceId = instanceId;
            reply();
        }
    }
}