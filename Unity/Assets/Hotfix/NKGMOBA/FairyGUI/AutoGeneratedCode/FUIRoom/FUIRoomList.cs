/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class FUIRoomListAwakeSystem : AwakeSystem<FUIRoomList, GObject>
    {
        public override void Awake(FUIRoomList self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public sealed class FUIRoomList : FUI
    {	
        public const string UIPackageName = "FUIRoom";
        public const string UIResName = "FUIRoomList";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GComponent self;
            
    public GImage n2;
    public GGraph n4;
    public Btn_QuitRoom CreateButton;
    public GList RoomList;
    public Btn_QuitRoom RefreshButton;
    public Btn_QuitRoom QutiButton;
    public const string URL = "ui://hya28zzrbp61c";

    private static GObject CreateGObject()
    {
        return UIPackage.CreateObject(UIPackageName, UIResName);
    }
    
    private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
    {
        UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
    }
        
    public static FUIRoomList CreateInstance()
    {			
        return ComponentFactory.Create<FUIRoomList, GObject>(CreateGObject());
    }
        
    public static ETTask<FUIRoomList> CreateInstanceAsync(Entity domain)
    {
        ETTaskCompletionSource<FUIRoomList> tcs = new ETTaskCompletionSource<FUIRoomList>();
        CreateGObjectAsync((go) =>
        {
            tcs.SetResult(ComponentFactory.Create<FUIRoomList, GObject>(go));
        });
        return tcs.Task;
    }
        
    public static FUIRoomList Create(GObject go)
    {
        return ComponentFactory.Create<FUIRoomList, GObject>(go);
    }
        
    /// <summary>
    /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
    /// </summary>
    public static FUIRoomList GetFormPool(GObject go)
    {
        var fui = go.Get<FUIRoomList>();
        if(fui == null)
        {
            fui = Create(go);
        }
        fui.isFromFGUIPool = true;
        return fui;
    }
        
    public void Awake(GObject go)
    {
        if(go == null)
        {
            return;
        }
        
        GObject = go;	
        
        if (string.IsNullOrWhiteSpace(Name))
        {
            Name = Id.ToString();
        }
        
        self = (GComponent)go;
        
        self.Add(this);
        
        var com = go.asCom;
            
        if(com != null)
        {	
            
    		n2 = (GImage)com.GetChildAt(0);
    		n4 = (GGraph)com.GetChildAt(1);
    		CreateButton = Btn_QuitRoom.Create(com.GetChildAt(2));
    		RoomList = (GList)com.GetChildAt(3);
    		RefreshButton = Btn_QuitRoom.Create(com.GetChildAt(4));
    		QutiButton = Btn_QuitRoom.Create(com.GetChildAt(5));
    	}
}
       public override void Dispose()
       {
            if(IsDisposed)
            {
                return;
            }
            
            base.Dispose();
            
            self.Remove();
            self = null;
            
			n2 = null;
			n4 = null;
			CreateButton = null;
			RoomList = null;
			RefreshButton = null;
			QutiButton = null;
		}
}
}