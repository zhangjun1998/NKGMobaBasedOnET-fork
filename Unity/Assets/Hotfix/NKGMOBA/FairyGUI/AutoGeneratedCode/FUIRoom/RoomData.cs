/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class RoomDataAwakeSystem : AwakeSystem<RoomData, GObject>
    {
        public override void Awake(RoomData self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public sealed class RoomData : FUI
    {	
        public const string UIPackageName = "FUIRoom";
        public const string UIResName = "RoomData";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GComponent self;
            
    public GTextField RoomName;
    public GTextField n8;
    public GTextField PlayerNum;
    public Join JoinButton;
    public GTextField RoomId;
    public const string URL = "ui://hya28zzrbp61d";

    private static GObject CreateGObject()
    {
        return UIPackage.CreateObject(UIPackageName, UIResName);
    }
    
    private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
    {
        UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
    }
        
    public static RoomData CreateInstance()
    {			
        return ComponentFactory.Create<RoomData, GObject>(CreateGObject());
    }
        
    public static ETTask<RoomData> CreateInstanceAsync(Entity domain)
    {
        ETTaskCompletionSource<RoomData> tcs = new ETTaskCompletionSource<RoomData>();
        CreateGObjectAsync((go) =>
        {
            tcs.SetResult(ComponentFactory.Create<RoomData, GObject>(go));
        });
        return tcs.Task;
    }
        
    public static RoomData Create(GObject go)
    {
        return ComponentFactory.Create<RoomData, GObject>(go);
    }
        
    /// <summary>
    /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
    /// </summary>
    public static RoomData GetFormPool(GObject go)
    {
        var fui = go.Get<RoomData>();
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
            
    		RoomName = (GTextField)com.GetChildAt(0);
    		n8 = (GTextField)com.GetChildAt(1);
    		PlayerNum = (GTextField)com.GetChildAt(2);
    		JoinButton = Join.Create(com.GetChildAt(3));
    		RoomId = (GTextField)com.GetChildAt(4);
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
            
			RoomName = null;
			n8 = null;
			PlayerNum = null;
			JoinButton = null;
			RoomId = null;
		}
}
}