using ETModel;

namespace ETHotfix
{
    public static class UnitEx 
    {
        public static UnitInfo UnitToUnitInfo(this Unit self)
        {
            UnitInfo unitInfo = new UnitInfo();
            if (self.GetComponent<B2S_RoleCastComponent>() != null)
            {
                //TODO 诺手UnitTypeId暂定10001
                unitInfo.UnitTypeId = 10001;
                unitInfo.RoleCamp = (int)self.GetComponent<B2S_RoleCastComponent>().RoleCamp;
            }

            unitInfo.X = self.Position.x;
            unitInfo.Y = self.Position.y;
            unitInfo.Z = self.Position.z;
            unitInfo.UnitId = self.Id;
            return unitInfo;
        }
    }
}
