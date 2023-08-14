/**
 * 数据驱动
 */

using System;

namespace GameBaseFramework.Base
{
#pragma warning disable CS0660 // 类型定义运算符 == 或运算符 !=，但不重写 Object.Equals(object o)
#pragma warning disable CS0661 // 类型定义运算符 == 或运算符 !=，但不重写 Object.GetHashCode()
    public class DataValue<T>
#pragma warning restore CS0661 // 类型定义运算符 == 或运算符 !=，但不重写 Object.GetHashCode()
#pragma warning restore CS0660 // 类型定义运算符 == 或运算符 !=，但不重写 Object.Equals(object o)
    {
        private T _value;
        /// <summary>
        /// 数值变化后回调
        /// </summary>
        private Action<T> _callback;

        private T _data
        {
            get
            {
                return _value;
            }
            set
            {
                if (this != value && _callback != null)
                {
                    _callback(value);
                }
                _value = value;
            }
        }

        /// <summary>
        /// 构造初始化
        /// </summary>
        /// <param name="value"></param>
        public DataValue(T value)
        {
            _value = value;
        }

        /// <summary>
        /// 赋值
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        public void Set(T value)
        {
            _data = value;
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <returns></returns>
        public T Get()
        {
            return _value;
        }

        /// <summary>
        /// 强转
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator T(DataValue<T> value)
        {
            return value._data;
        }

        /// <summary>
        /// 添加监听回调方法
        /// </summary>
        /// <param name="value"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static DataValue<T> operator +(DataValue<T> value, Action<T> callback)
        {
            if (value == null)
            {
                value = new DataValue<T>(default(T));
            }
            value._callback += callback;
            return value;
        }

        /// <summary>
        /// 删除监听回调
        /// </summary>
        /// <param name="value"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static DataValue<T> operator -(DataValue<T> value, Action<T> callback)
        {
            if (value == null)
            {
                value = new DataValue<T>(default(T));
            }
            value._callback -= callback;
            return value;
        }

        /// <summary>
        /// ==
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(DataValue<T> x, T y)
        {
            if (x == null || x._data == null)
            {
                return y == null;
            }
            return x._data.Equals(y);
        }

        /// <summary>
        /// !=
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(DataValue<T> x, T y)
        {
            if (x == null || x._data == null)
            {
                return y != null;
            }
            return !x._data.Equals(y);
        }
    }
}
