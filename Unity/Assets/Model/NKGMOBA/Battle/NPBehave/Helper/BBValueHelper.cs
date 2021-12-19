﻿using System.Collections.Generic;
using NPBehave;

namespace ET
{
    public static class BBValueHelper
    {
        /// <summary>
        /// 通过ANP_BBValue来设置目标黑板值
        /// </summary>
        /// <param name="self"></param>
        /// <param name="blackboard"></param>
        /// <param name="key"></param>
        public static void SetTargetBlackboardUseANP_BBValue(ANP_BBValue nAnpBbValue, Blackboard blackboard, string key)
        {
            // 这里只能用这个ToString()来做判断，直接获取Name的话是简略版本的
            switch (nAnpBbValue.NP_BBValueType.ToString())
            {
                case "System.String":
                    blackboard.Set(key, (nAnpBbValue as NP_BBValue_String).GetValue());
                    break;
                case "System.Single":
                    blackboard.Set(key, (nAnpBbValue as NP_BBValue_Float).GetValue());
                    break;
                case "System.Int32":
                    blackboard.Set(key, (nAnpBbValue as NP_BBValue_Int).GetValue());
                    break;
                case "System.Int64":
                    blackboard.Set(key, (nAnpBbValue as NP_BBValue_Long).GetValue());
                    break;
                case "System.UInt32":
                    blackboard.Set(key, (nAnpBbValue as NP_BBValue_UInt).GetValue());
                    break;
                case "System.Boolean":
                    blackboard.Set(key, (nAnpBbValue as NP_BBValue_Bool).GetValue());
                    break;
                case "System.Collections.Generic.List`1[System.Int64]":
                    //因为List是引用类型，所以这里要做一下特殊处理，如果要设置的值为0元素的List，就Clear一下，而且这个东西也不会用来做为黑板条件，因为它没办法用来对比
                    //否则就拷贝全部元素
                    NP_BBValue_List_Long selfBBValue =(nAnpBbValue as NP_BBValue_List_Long);
                    List<long> targetList = blackboard.Get<List<long>>(key);
                    if (selfBBValue.Value.Count == 0)
                    {
                        targetList.Clear();
                    }
                    else
                    {
                        targetList.Clear();
                        foreach (var item in selfBBValue.Value)
                        {
                            targetList.Add(item);
                        }
                    }

                    break;
                case "System.Numerics.Vector3":
                    blackboard.Set(key, (nAnpBbValue as NP_BBValue_Vector3).GetValue());
                    break;
            }
        }
    }
}