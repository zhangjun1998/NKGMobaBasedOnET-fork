/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ETHotfix.FUIRoom
{
    public partial class Btn_StartGame : GButton
    {
        public Controller button;
        public GImage n0;
        public GImage n1;
        public GTextField title;
        public const string URL = "ui://hya28zzrbp61b";

        public static Btn_StartGame CreateInstance()
        {
            return (Btn_StartGame)UIPackage.CreateObject("FUIRoom", "Btn_StartGame");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            button = GetControllerAt(0);
            n0 = (GImage)GetChildAt(0);
            n1 = (GImage)GetChildAt(1);
            title = (GTextField)GetChildAt(2);
        }
    }
}