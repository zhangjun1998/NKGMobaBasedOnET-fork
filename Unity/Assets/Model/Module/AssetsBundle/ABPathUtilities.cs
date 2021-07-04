//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年10月14日 22:27:15
//------------------------------------------------------------

namespace ETModel
{
    /// <summary>
    /// AB实用函数集，主要是路径拼接
    /// </summary>
    public class ABPathUtilities
    {
        public static string GetUnitAvatatIcon(string UnitName, string iconName)
        {
            return GetTexturePath($"Avatars/{UnitName}/{iconName}.png");
        }

        public static string GetSkillIcon(string UnitName, string iconName)
        {
            return GetTexturePath($"Skills/{UnitName}/{iconName}.png");
        }

        public static string GetLoadingIcon(string UnitName, string iconName)
        {
            return GetTexturePath($"Loadings/{UnitName}/{iconName}.png");
        }
        
        /// <summary>
        /// 获取纹理，filename需要填全路径（包括拓展名）
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetTexturePath(string fileName)
        {
            return $"Assets/Res/Sprites/{fileName}";
        }

        public static string GetFGUIDesPath(string fileName)
        {
            return $"Assets/Bundles/FUI/{fileName}.bytes";
        }

        public static string GetFGUIResPath(string fileName, string extension)
        {
            return $"Assets/Bundles/FUI/{fileName}{extension}";
        }

        public static string GetNormalConfigPath(string fileName)
        {
            return $"Assets/Bundles/Independent/{fileName}.prefab";
        }

        public static string GetSoundPath(string fileName)
        {
            return $"Assets/Bundles/Sounds/{fileName}.prefab";
        }

        public static string GetSkillConfigPath(string fileName)
        {
            return $"Assets/Bundles/SkillConfigs/{fileName}.prefab";
        }

        public static string GetUnitPath(string fileName)
        {
            return $"Assets/Bundles/Unit/{fileName}.prefab";
        }

        public static string GetScenePath(string fileName)
        {
            return $"Assets/Scenes/{fileName}.unity";
        }
        
        public static string GetMaterialPath(string fileName)
        {
            return $"Assets/Bundles/Materials/{fileName}.prefab";
        }
    }
}