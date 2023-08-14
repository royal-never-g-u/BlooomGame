/**
 * 单例模式
 */

namespace GameBaseFramework.Patterns
{
    public class Singleton<T> where T : class, new()
    {
        /// <summary>
        /// 唯一引用
        /// </summary>
        protected static T _instance;

        /// <summary>
        /// 公用访问
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
                return _instance;
            }
        }
    }
}