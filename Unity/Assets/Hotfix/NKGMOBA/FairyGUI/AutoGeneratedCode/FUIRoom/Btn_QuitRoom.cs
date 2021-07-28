/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class Btn_QuitRoomAwakeSystem : AwakeSystem<Btn_QuitRoom, GObject>
    {
        public override void Awake(Btn_QuitRoom self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public sealed class Btn_QuitRoom : FUI
    {	
        public const string UIPackageName = "FUIRoom";
        public const string UIResName = "Btn_QuitRoom";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GButton self;
            
    public Controller button;
    public GImage n0;
    public GImage n1;
    public GTextField title;
    public const string URL = "ui://hya28zzrbp613";

    private static GObject CreateGObject()
    {
        return UIPackage.CreateObject(UIPackageName, UIResName);
    }
    
    private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
    {
        UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
    }
        
    public static Btn_QuitRoom CreateInstance()
    {			
        return ComponentFactory.Create<Btn_QuitRoom, GObject>(CreateGObject());
    }
        
    public static ETTask<Btn_QuitRoom> CreateInstanceAsync(Entity domain)
    {
        ETTaskCompletionSource<Btn_QuitRoom> tcs = new ETTaskCompletionSource<Btn_QuitRoom>();
        CreateGObjectAsync((go) =>
        {
            tcs.SetResult(ComponentFactory.Create<Btn_QuitRoom, GObject>(go));
        });
        return tcs.Task;
    }
        
    public static Btn_QuitRoom Create(GObject go)
    {
        return ComponentFactory.Create<Btn_QuitRoom, GObject>(go);
    }
        
    /// <summary>
    /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
    /// </summary>
    public static Btn_QuitRoom GetFormPool(GObject go)
    {
        var fui = go.Get<Btn_QuitRoom>();
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
        
        self = (GButton)go;
        
        self.Add(this);
        
        var com = go.asCom;
            
        if(com != null)
        {	
            
    		button = com.GetControllerAt(0);
    		n0 = (GImage)com.GetChildAt(0);
    		n1 = (GImage)com.GetChildAt(1);
    		title = (GTextField)com.GetChildAt(2);
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
            
			button = null;
			n0 = null;
			n1 = null;
			title = null;
		}
}
}