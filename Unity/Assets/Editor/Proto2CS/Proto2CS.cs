//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年9月1日 21:53:23
//------------------------------------------------------------

using UnityEditor;

namespace ET
{
    public class Proto2CS
    {
        [MenuItem("Tools/PB协议导出")]
        public static void DoExcelExport()
        {
            ProcessHelper.Run("Proto2CS.exe", "", "../Tools/Proto2CS/Bin/");
        }
    }
}