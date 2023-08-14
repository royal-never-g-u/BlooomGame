/**
 * 字段扩展
 */

using System.Collections.Generic;

namespace GameBaseFramework.Extensions
{
    public static class DictionaryExtension
    {
        /// <summary>
        /// 返回Key组成的列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static List<T> GetKeyList<T, U>(this Dictionary<T, U> self)
        {
            var list = new List<T>();
            foreach (var key in self.Keys)
            {
                list.Add(key);
            }
            return list;
        }


        /// <summary>
        /// 返回Value组成的列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static List<U> GetValueList<T, U>(this Dictionary<T, U> self)
        {
            var list = new List<U>();
            foreach(var value in self.Values)
            {
                list.Add(value);
            }
            return list;
        }
    }
}
