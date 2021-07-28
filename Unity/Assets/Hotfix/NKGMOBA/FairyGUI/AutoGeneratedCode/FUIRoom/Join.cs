/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class JoinAwakeSystem : AwakeSystem<Join, GObject>
    {
        public override void Awake(Join self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public sealed class Join : FUI
    {	
        public const string UIPackageName = "FUIRoom";
        public const string UIResName = "Join";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GButton self;
            
    public Controller button;
    public GImage n0;
    public const string URL = "ui://hya28zzrbp61f";

    private static GObject CreateGObject()
    {
        return UIPackage.CreateObject(UIPackageName, UIResName);
    }
    
    private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
    {
        UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
    }
        
    public static Join CreateInstance()
    {			
        return ComponentFactory.Create<Join, GObject>(CreateGObject());
    }
        
    public static ETTask<Join> CreateInstanceAsync(Entity domain)
    {
        ETTaskCompletionSource<Join> tcs = new ETTaskCompletionSource<Join>();
        CreateGObjectAsync((go) =>
        {
            tcs.SetResult(ComponentFactory.Create<Join, GObject>(go));
        });
        return tcs.Task;
    }
        
    public static Join Create(GObject go)
    {
        return ComponentFactory.Create<Join, GObject>(go);
    }
        
    /// <summary>
    /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
    /// </summary>
    public static Join GetFormPool(GObject go)
    {
        var fui = go.Get<Join>();
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
		}
}
}