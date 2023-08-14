/**
 * Unity相关扩展
 */

using System.Collections.Generic;
using UnityEngine;

namespace GameUnityFramework.Extensions
{
    public static class UnityExtensions
    {
        /// <summary>
        /// 删除所有子节点(Transform)
        /// </summary>
        /// <param name="self"></param>
        public static void RemoveAllChilds(this Transform self)
        {
            for (var i = self.childCount - 1; i >= 0; i--)
            {
                GameObject.DestroyImmediate(self.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// 删除所有子节点(GameObject)
        /// </summary>
        /// <param name="self"></param>
        public static void RemoveAllChilds(this GameObject self)
        {
            RemoveAllChilds(self.transform);
        }
        /// <summary>
        /// 找到所有名称为str的子节点
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<GameObject> FindChildByName(this Transform trans,string str)
        {
            List<GameObject> result=new List<GameObject>();
            foreach(Transform item in trans)
            {
                if (item.name.Contains(str))
                {
                    result.Add(item.gameObject);
                }
                result.AddRange(FindChildByName(item, str));
            }
            return result;
        }
        public static List<GameObject> FindChildByName(GameObject trans, string str)
        {
            return trans.transform.FindChildByName(str);
        }
        public static List<GameObject> FindChildByExactName(GameObject trans, string str)
        {
            List<GameObject> result = new List<GameObject>();
            foreach (Transform item in trans.transform)
            {
                if (item.name==str)
                {
                    result.Add(item.gameObject);
                }
                result.AddRange(FindChildByExactName(item.gameObject, str));
            }
            return result;
        }
        /// <summary>
        /// 获得或添加组件（GameObject）
        /// </summary>
        /// <param name="self">GameObject</param>
        public static T GetOrAddComponent<T>(this GameObject self) where T : Component
        {
            T component = self.GetComponent<T>();
            if (component == null) component = self.AddComponent<T>();
            return component;
        }

        /// <summary>
        /// 获得或添加组件（Transform）
        /// </summary>
        /// <param name="self">Transform</param>
        public static T GetOrAddComponent<T>(this Transform self) where T : Component
        {
            return GetOrAddComponent<T>(self.gameObject);
        }
    }
}
