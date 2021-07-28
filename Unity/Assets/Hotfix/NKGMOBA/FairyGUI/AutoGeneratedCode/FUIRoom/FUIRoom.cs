/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class FUIRoomAwakeSystem : AwakeSystem<FUIRoom, GObject>
    {
        public override void Awake(FUIRoom self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public sealed class FUIRoom : FUI
    {	
        public const string UIPackageName = "FUIRoom";
        public const string UIResName = "FUIRoom";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GComponent self;
            
    public Controller IsMaster;
    public GImage n2;
    public GGraph n4;
    public GTextField RoomName;
    public Btn_QuitRoom QuitButton;
    public GList Team1;
    public GList Team2;
    public Btn_StartGame StartButton;
    public const string URL = "ui://hya28zzrbp610";

    private static GObject CreateGObject()
    {
        return UIPackage.CreateObject(UIPackageName, UIResName);
    }
    
    private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
    {
        UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
    }
        
    public static FUIRoom CreateInstance()
    {			
        return ComponentFactory.Create<FUIRoom, GObject>(CreateGObject());
    }
        
    public static ETTask<FUIRoom> CreateInstanceAsync(Entity domain)
    {
        ETTaskCompletionSource<FUIRoom> tcs = new ETTaskCompletionSource<FUIRoom>();
        CreateGObjectAsync((go) =>
        {
            tcs.SetResult(ComponentFactory.Create<FUIRoom, GObject>(go));
        });
        return tcs.Task;
    }
        
    public static FUIRoom Create(GObject go)
    {
        return ComponentFactory.Create<FUIRoom, GObject>(go);
    }
        
    /// <summary>
    /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
    /// </summary>
    public static FUIRoom GetFormPool(GObject go)
    {
        var fui = go.Get<FUIRoom>();
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
            
    		IsMaster = com.GetControllerAt(0);
    		n2 = (GImage)com.GetChildAt(0);
    		n4 = (GGraph)com.GetChildAt(1);
    		RoomName = (GTextField)com.GetChildAt(2);
    		QuitButton = Btn_QuitRoom.Create(com.GetChildAt(3));
    		Team1 = (GList)com.GetChildAt(4);
    		Team2 = (GList)com.GetChildAt(5);
    		StartButton = Btn_StartGame.Create(com.GetChildAt(6));
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
            
			IsMaster = null;
			n2 = null;
			n4 = null;
			RoomName = null;
			QuitButton = null;
			Team1 = null;
			Team2 = null;
			StartButton = null;
		}
}
}