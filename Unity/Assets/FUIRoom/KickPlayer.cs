/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ETHotfix.FUIRoom
{
    public partial class KickPlayer : GButton
    {
        public Controller button;
        public GImage n0;
        public const string URL = "ui://hya28zzrbp61a";

        public static KickPlayer CreateInstance()
        {
            return (KickPlayer)UIPackage.CreateObject("FUIRoom", "KickPlayer");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            button = GetControllerAt(0);
            n0 = (GImage)GetChildAt(0);
        }
    }
}