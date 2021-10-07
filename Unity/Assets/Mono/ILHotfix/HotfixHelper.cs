using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using ILRuntime.CLR.TypeSystem;
using UnityEngine;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace ET
{
    public static class HotfixHelper
    {
        private static MemoryStream s_hotfixDllStream;
        private static MemoryStream s_hotfixPdbStream;
        
        private static IStaticMethod _entryMethod;

        private static AppDomain _appDomain;
        private static Assembly _assembly;

        /// <summary>
        /// 这里开始正式进入游戏逻辑
        /// </summary>
        public static void GoToHotfix(byte[] dllByte, byte[] pdbByte)
        {
            if (GlobalDefine.ILRuntimeMode)
            {
                _appDomain = new ILRuntime.Runtime.Enviorment.AppDomain();
                s_hotfixDllStream = new MemoryStream(dllByte);
                s_hotfixPdbStream = new MemoryStream(pdbByte);
                _appDomain.LoadAssembly(s_hotfixDllStream, s_hotfixPdbStream,
                    new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());

                ILHelper.InitILRuntime(_appDomain);
                
                _entryMethod = new ILStaticMethod(_appDomain, "ET.InitEntry", "RegFunction", 0);
            }
            else
            {
                _assembly = Assembly.Load(dllByte, pdbByte);
                _entryMethod = new MonoStaticMethod(_assembly, "ET.InitEntry", "RegFunction");
            }

            _entryMethod.Run();
        }

        public static Type[] GetAssemblyTypes()
        {
            Type[] types;
            if (GlobalDefine.ILRuntimeMode)
            {
                types = _appDomain.LoadedTypes.Values.Select(t => t.ReflectionType).ToArray();
            }
            else
            {
                types = _assembly.GetTypes();
            }

            return types;
        }

        public static List<Type> GetIlrAttributeTypes(List<Type> types)
        {
            List<Type> attributeTypes = new List<Type>();
            foreach (Type item in types)
            {
                if (item.IsAbstract)
                {
                    continue;
                }

                if (item.IsSubclassOf(typeof(Attribute)))
                {
                    attributeTypes.Add(item);
                }
            }

            return attributeTypes;
        }
    }
}