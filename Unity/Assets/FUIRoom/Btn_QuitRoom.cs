/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ETHotfix.FUIRoom
{
    public partial class Btn_QuitRoom : GButton
    {
        public Controller button;
        public GImage n0;
        public GImage n1;
        public GTextField title;
        public const string URL = "ui://hya28zzrbp613";

        public static Btn_QuitRoom CreateInstance()
        {
            return (Btn_QuitRoom)UIPackage.CreateObject("FUIRoom", "Btn_QuitRoom");
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