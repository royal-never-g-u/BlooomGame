/**
 * 获取类型对应的唯一Id
 */

namespace GameBaseFramework.Base
{
    public interface ITypeId { }

    public class TypeId
    {
        /// <summary>
        /// TypeId
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class Id<T> where T : ITypeId
        {
            public static int TypeId = -1;
        }

        /// <summary>
        /// 唯一Id
        /// </summary>
        public static int _uniqueId = 0;

        /// <summary>
        /// 获取类型的Id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static int GetId<T>() where T : ITypeId
        {
            if (Id<T>.TypeId < 0)
            {
                Id<T>.TypeId = ++_uniqueId;
            }
            return Id<T>.TypeId;
        }
    }
}
