/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;

namespace ETHotfix.FUIRoom
{
    public class FUIRoomBinder
    {
        public static void BindAll()
        {
            UIObjectFactory.SetPackageItemExtension(FUIRoom.URL, typeof(FUIRoom));
            UIObjectFactory.SetPackageItemExtension(Btn_QuitRoom.URL, typeof(Btn_QuitRoom));
            UIObjectFactory.SetPackageItemExtension(PlayerData.URL, typeof(PlayerData));
            UIObjectFactory.SetPackageItemExtension(KickPlayer.URL, typeof(KickPlayer));
            UIObjectFactory.SetPackageItemExtension(Btn_StartGame.URL, typeof(Btn_StartGame));
        }
    }
}