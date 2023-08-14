using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameBaseFramework.Base
{
    public static class JsonWrapper
    {

        static JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        static JsonSerializerSettings camelCaseserializerSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
        };

        /// <summary>
        /// 将对象序列化为JSON字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="camelCase">是否驼峰命名(首字母小写)</param>
        /// <returns></returns>
        public static string ToJsonStr<T>(T item, bool camelCase = false)
        {

            return JsonConvert.SerializeObject(item, camelCase ? camelCaseserializerSettings : serializerSettings);
        }

        /// <summary>
        /// 将JSON字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T FromJson<T>(string jsonString)
        {
            if (jsonString == null || jsonString == "")
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        /// <summary>
        /// 将JSON字符串反序列化为对象
        /// </summary>
        /// <param name="type">返回对象类型</param>
        /// <param name="jsonString">json字符串</param>
        /// <returns></returns>
        public static object FromJson(Type type, string jsonString)
        {

            if (jsonString == null || jsonString == "")
            {
                return Activator.CreateInstance(type);
            }
            return JsonConvert.DeserializeObject(jsonString, type);
        }
        public static object FromJson(object obj, string str)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type t = typeof(JsonWrapper);
            MethodInfo fun = t.GetMethod("FromJson", new Type[]
            {
                typeof(string),
            });
            fun = fun.MakeGenericMethod(obj.GetType());
            return fun.Invoke(null, new object[] {
                str,
            });
        }
        /// <summary>
        /// 扩展函数，将对象序列化为JSON字符串
        /// </summary>
        /// <param name="obj"></param>
        ///  /// <param name="camelCase">是否驼峰命名(首字母小写)</param>
        /// <returns></returns>
        public static string ToJson(this object obj, bool camelCase = false)
        {
            return ToJsonStr(obj,camelCase);
        }
    }
}
