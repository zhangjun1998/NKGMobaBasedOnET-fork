//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月5日 20:09:52
//------------------------------------------------------------

using ETModel;

namespace ETHotfix
{
    [Event(EventIdType.ShowErrorDialog)]
    public class ShowErrorDialogEvent : AEvent<int>
    {
        public override void Run(int error)
        {
            //Log.Info("服务端主动断开连接要显示对话框");
            var hotfixui = FUIDialog.CreateInstance();
            //默认将会以Id为Name，也可以自定义Name，方便查询和管理
            hotfixui.Name = FUIPackage.FUIDialog;
            hotfixui.towmode.visible = false;


            hotfixui.Tittle.text = "错误";
            string content = "";
            switch (error)
            {
                case ErrorCode.ERR_AlreadyInBattle:
                    content = "已经在战斗中";
                    break;
                default:
                    content = $"服务端返回错误码:{error}";
                    break;
            }
            hotfixui.Conten.text = content;

            hotfixui.one_confirm.self.onClick.Add(() =>
            {
                Game.Scene.GetComponent<FUIComponent>().Remove(FUIPackage.FUIDialog);
            });

            hotfixui.GObject.sortingOrder = 40;
            hotfixui.MakeFullScreen();
            Game.Scene.GetComponent<FUIComponent>().Add(hotfixui, true);
        }
    }
    [Event(EventIdType.ShowMsgDialog)]
    public class ShowMsgDialogEvent : AEvent<string>
    {
        public override void Run(string msg)
        {
            //Log.Info("服务端主动断开连接要显示对话框");
            var hotfixui = FUIDialog.CreateInstance();
            //默认将会以Id为Name，也可以自定义Name，方便查询和管理
            hotfixui.Name = FUIPackage.FUIDialog;
            hotfixui.towmode.visible = false;


            hotfixui.Tittle.text = "通知";

            hotfixui.Conten.text = msg;

            hotfixui.one_confirm.self.onClick.Add(() =>
            {
                Game.Scene.GetComponent<FUIComponent>().Remove(FUIPackage.FUIDialog);
            });

            hotfixui.GObject.sortingOrder = 40;
            hotfixui.MakeFullScreen();
            Game.Scene.GetComponent<FUIComponent>().Add(hotfixui, true);
        }
    }
}