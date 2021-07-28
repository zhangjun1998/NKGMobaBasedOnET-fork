/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class PlayerDataAwakeSystem : AwakeSystem<PlayerData, GObject>
    {
        public override void Awake(PlayerData self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public sealed class PlayerData : FUI
    {	
        public const string UIPackageName = "FUIRoom";
        public const string UIResName = "PlayerData";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GComponent self;
            
    public Controller IsMaster;
    public Controller HasAdminFunc;
    public GTextField RoomPlayerLevel;
    public GTextField RoomPlayerName;
    public GLoader n3;
    public KickPlayer KickButton;
    public GTextField PlayerId;
    public const string URL = "ui://hya28zzrbp616";

    private static GObject CreateGObject()
    {
        return UIPackage.CreateObject(UIPackageName, UIResName);
    }
    
    private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
    {
        UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
    }
        
    public static PlayerData CreateInstance()
    {			
        return ComponentFactory.Create<PlayerData, GObject>(CreateGObject());
    }
        
    public static ETTask<PlayerData> CreateInstanceAsync(Entity domain)
    {
        ETTaskCompletionSource<PlayerData> tcs = new ETTaskCompletionSource<PlayerData>();
        CreateGObjectAsync((go) =>
        {
            tcs.SetResult(ComponentFactory.Create<PlayerData, GObject>(go));
        });
        return tcs.Task;
    }
        
    public static PlayerData Create(GObject go)
    {
        return ComponentFactory.Create<PlayerData, GObject>(go);
    }
        
    /// <summary>
    /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
    /// </summary>
    public static PlayerData GetFormPool(GObject go)
    {
        var fui = go.Get<PlayerData>();
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
    		HasAdminFunc = com.GetControllerAt(1);
    		RoomPlayerLevel = (GTextField)com.GetChildAt(0);
    		RoomPlayerName = (GTextField)com.GetChildAt(1);
    		n3 = (GLoader)com.GetChildAt(2);
    		KickButton = KickPlayer.Create(com.GetChildAt(3));
    		PlayerId = (GTextField)com.GetChildAt(4);
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
			HasAdminFunc = null;
			RoomPlayerLevel = null;
			RoomPlayerName = null;
			n3 = null;
			KickButton = null;
			PlayerId = null;
		}
}
}