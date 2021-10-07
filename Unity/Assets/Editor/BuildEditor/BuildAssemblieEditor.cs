using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;

namespace ET
{
    [InitializeOnLoad]
    public static class BuildAssemblieEditor
    {
        /// <summary>
        /// 最原始的4个程序集路径
        /// </summary>
        private static string[] s_OriginDllDirs = new[]
        {
            "Library/ScriptAssemblies/Unity.Model.dll",
            "Library/ScriptAssemblies/Unity.ModelView.dll",
            "Library/ScriptAssemblies/Unity.Hotfix.dll",
            "Library/ScriptAssemblies/Unity.HotfixView.dll"
        };

        /// <summary>
        /// 脚本所在目录
        /// </summary>
        private static string[] s_OriginScriptsDirs = new[]
        {
            "Assets/Model/",
            "Assets/ModelView/",
            "Assets/Hotfix/",
            "Assets/HotfixView/"
        };

        /// <summary>
        /// 最终构建的dll名称
        /// </summary>
        private static string s_FinalHotfixDllName = "Hotfix";

        /// <summary>
        /// 合并的程序集路径
        /// </summary>
        private static string s_ScriptAssembliesDir = "Temp/FinalHotfixAssembly/";

        /// <summary>
        /// m_OriginDll的md5文件，只有发生了改变才会进行重新编译
        /// </summary>
        private static string s_ScriptAssembliesMd5FilePath = $"{s_ScriptAssembliesDir}md5.txt";

        /// <summary>
        /// 最终的Hotfix dll路径
        /// </summary>
        private static string s_FinalHotfixDllDir = "Assets/Res/Code/";


        static BuildAssemblieEditor()
        {
            Directory.CreateDirectory(s_ScriptAssembliesDir);

            if (File.Exists(s_ScriptAssembliesMd5FilePath))
            {
                string oldMD5 = File.ReadAllText(s_ScriptAssembliesMd5FilePath);
                string newMD5 = "";
                foreach (var t in s_OriginDllDirs)
                {
                    newMD5 += MD5Helper.FileMD5(t);
                }

                if (newMD5 == oldMD5)
                {
                    return;
                }

                BuildMuteAssembly();

                using (StreamWriter file = File.CreateText(s_ScriptAssembliesMd5FilePath))
                {
                    file.Write(newMD5);
                }
            }
            else
            {
                BuildMuteAssembly();
                using (StreamWriter file = File.CreateText(s_ScriptAssembliesMd5FilePath))
                {
                    string newMD5 = "";
                    foreach (var t in s_OriginDllDirs)
                    {
                        newMD5 += MD5Helper.FileMD5(t).ToString();
                    }

                    file.Write(newMD5);
                }
            }
        }

        private static void CopyDllAndPdb(string FileName)
        {
            string dllOriPath = Path.Combine(s_ScriptAssembliesDir, FileName + ".dll");
            string dllDesPath = Path.Combine(s_FinalHotfixDllDir, FileName + ".dll.bytes");

            string pdbOriPath = Path.Combine(s_ScriptAssembliesDir, FileName + ".pdb");
            string pdbDesPath = Path.Combine(s_FinalHotfixDllDir, FileName + ".pdb.bytes");
            
            File.Copy(dllOriPath, dllDesPath, true);
            File.Copy(pdbOriPath, pdbDesPath, true);
            
            AssetDatabase.ImportAsset(dllDesPath);
            AssetDatabase.ImportAsset(pdbDesPath);
            AssetDatabase.Refresh();
        }

        public static void BuildMuteAssembly()
        {
            List<string> scripts = new List<string>();
            for (int i = 0; i < s_OriginScriptsDirs.Length; i++)
            {
                DirectoryInfo dti = new DirectoryInfo(s_OriginScriptsDirs[i]);
                FileInfo[] fileInfos = dti.GetFiles("*.cs", System.IO.SearchOption.AllDirectories);
                for (int j = 0; j < fileInfos.Length; j++)
                {
                    scripts.Add(fileInfos[j].FullName);
                }
            }

            string outputAssembly = s_ScriptAssembliesDir + s_FinalHotfixDllName + ".dll";

            Directory.CreateDirectory(s_ScriptAssembliesDir);

            BuildTargetGroup buildTargetGroup =
                BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);

            AssemblyBuilder assemblyBuilder = new AssemblyBuilder(outputAssembly, scripts.ToArray());

            //启用UnSafe
            assemblyBuilder.compilerOptions.AllowUnsafeCode = true;

            assemblyBuilder.compilerOptions.ApiCompatibilityLevel =
                PlayerSettings.GetApiCompatibilityLevel(buildTargetGroup);

            assemblyBuilder.compilerOptions.CodeOptimization = CodeOptimization.Release;

            assemblyBuilder.flags = AssemblyBuilderFlags.EditorAssembly;
            //AssemblyBuilderFlags.None                 正常发布
            //AssemblyBuilderFlags.DevelopmentBuild     开发模式打包
            //AssemblyBuilderFlags.EditorAssembly       编辑器状态


            assemblyBuilder.referencesOptions = ReferencesOptions.UseEngineModules;

            assemblyBuilder.buildTarget = EditorUserBuildSettings.activeBuildTarget;

            assemblyBuilder.buildTargetGroup = buildTargetGroup;


             // assemblyBuilder.additionalDefines = new[]
             // {
             //     "UNITY_EDITOR"
             // };

            //需要排除自身的引用
            assemblyBuilder.excludeReferences = s_OriginDllDirs;

            assemblyBuilder.buildStarted += delegate(string assemblyPath)
            {
                Debug.LogFormat("程序集开始构建：" + assemblyPath);
            };

            assemblyBuilder.buildFinished += delegate(string assemblyPath, CompilerMessage[] compilerMessages)
            {
                int errorCount = compilerMessages.Count(m => m.type == CompilerMessageType.Error);
                int warningCount = compilerMessages.Count(m => m.type == CompilerMessageType.Warning);
                Debug.LogFormat("程序集构建完成：" + assemblyPath);
                Debug.LogFormat("Warnings: {0} - Errors: {1}", warningCount, errorCount);
                for (int i = 0;
                    i < compilerMessages.Length;
                    i++)
                {
                    if (compilerMessages[i].type == CompilerMessageType.Error)
                    {
                        Debug.LogError(compilerMessages[i].message);
                    }

                    if (compilerMessages[i].type == CompilerMessageType.Warning)
                    {
                        Debug.LogWarning(compilerMessages[i].message);
                    }
                }

                if (errorCount == 0)
                {
                    CopyDllAndPdb(s_FinalHotfixDllName);
                }
            };

            //开始构建
            if (!assemblyBuilder.Build())
            {
                Debug.LogErrorFormat("构建程序集失败：" + assemblyBuilder.assemblyPath + "构建程序集时遇到致命错误，请修复！");
                return;
            }
        }
    }
}