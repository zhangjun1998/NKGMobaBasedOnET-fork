using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class UnitDestroySystem : DestroySystem<Unit>
    {
        public override void Destroy(Unit self)
        {
            self.GetComponent<MailBoxComponent>().RemoveLocation().Coroutine();
            self.Position = UnityEngine.Vector3.zero;
            self.Rotation = UnityEngine.Quaternion.identity;
        }
    }
}
