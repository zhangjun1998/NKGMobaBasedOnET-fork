/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ETHotfix.FUIRoom
{
    public partial class PlayerData : GComponent
    {
        public Controller IsMaster;
        public Controller HasAdminFunc;
        public GTextField RoomPlayerLevel;
        public GTextField RoomPlayerName;
        public GLoader n3;
        public KickPlayer n7;
        public const string URL = "ui://hya28zzrbp616";

        public static PlayerData CreateInstance()
        {
            return (PlayerData)UIPackage.CreateObject("FUIRoom", "PlayerData");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            IsMaster = GetControllerAt(0);
            HasAdminFunc = GetControllerAt(1);
            RoomPlayerLevel = (GTextField)GetChildAt(0);
            RoomPlayerName = (GTextField)GetChildAt(1);
            n3 = (GLoader)GetChildAt(2);
            n7 = (KickPlayer)GetChildAt(3);
        }
    }
}