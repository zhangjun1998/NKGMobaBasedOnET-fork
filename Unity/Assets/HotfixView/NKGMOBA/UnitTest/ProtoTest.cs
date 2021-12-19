using System.Collections.Generic;

namespace ET
{
    public class ProtoTest
    {
        public void Do()
        {
            NP_BBValue_Float npBbValueFloat = new NP_BBValue_Float();
            npBbValueFloat.Value = 99;
            byte[] s = ProtobufHelper.ToBytes(npBbValueFloat);
            NP_BBValue_Float npBbValueFloat1 = ProtobufHelper.FromBytes<NP_BBValue_Float>(s, 0, s.Length);

            NP_BBValue_List_Long npBbValueListlong = new NP_BBValue_List_Long();
            npBbValueListlong.Value = new List<long>() {99, 98};
            byte[] s1 = ProtobufHelper.ToBytes(npBbValueListlong);
            NP_BBValue_List_Long npBbValueFloat2 = ProtobufHelper.FromBytes<NP_BBValue_List_Long>(s1, 0, s1.Length);

            LSF_ChangeBBValue lsf = new LSF_ChangeBBValue();

            lsf.TargetBBValues.Add("TestList", npBbValueListlong);
            lsf.TargetBBValues.Add("TestFloat", npBbValueFloat);
            
            byte[] s2 = ProtobufHelper.ToBytes(lsf);
            LSF_ChangeBBValue lsf1 = ProtobufHelper.FromBytes<LSF_ChangeBBValue>(s2, 0, s2.Length);
            
            Log.Info(lsf1.ToString());
        }
    }
}