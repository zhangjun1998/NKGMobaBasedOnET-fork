/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ETHotfix.FUIRoom
{
    public partial class FUIRoom : GComponent
    {
        public Controller IsMaster;
        public GImage n2;
        public GGraph n4;
        public GTextField RoomName;
        public Btn_QuitRoom QuitButton;
        public GList Team1;
        public GList Team2;
        public Btn_StartGame StartButton;
        public const string URL = "ui://hya28zzrbp610";

        public static FUIRoom CreateInstance()
        {
            return (FUIRoom)UIPackage.CreateObject("FUIRoom", "FUIRoom");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            IsMaster = GetControllerAt(0);
            n2 = (GImage)GetChildAt(0);
            n4 = (GGraph)GetChildAt(1);
            RoomName = (GTextField)GetChildAt(2);
            QuitButton = (Btn_QuitRoom)GetChildAt(3);
            Team1 = (GList)GetChildAt(4);
            Team2 = (GList)GetChildAt(5);
            StartButton = (Btn_StartGame)GetChildAt(6);
        }
    }
}