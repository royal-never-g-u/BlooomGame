/**
 * 指令集
 */

using GameBaseFramework.Base;

namespace GameBaseFramework.Patterns
{
    /// <summary>
    /// 指令优先级
    /// </summary>
    public enum ECmdPriority
    {
        Level0 = 0,
        Level1 = 1,
        Level2 = 2,
        Level3 = 3,
        Level4 = 4,
        Level5 = 5,
        Level6 = 6,
        Level7 = 7,
        Level8 = 8,
        Level9 = 9,
    }

    /// <summary>
    /// 指令基类
    /// </summary>
    public abstract class Command : ITypeId
    {
        /// <summary>
        /// 类型Id
        /// </summary>
        public int TypeId = -1;
        /// <summary>
        /// 指令索引
        /// </summary>
        public int Index = 0;
        /// <summary>
        /// 是否同步执行
        /// </summary>
        public bool SyncExecute = true;
        /// <summary>
        /// 指令优先级
        /// </summary>
        public ECmdPriority Priority = ECmdPriority.Level0;
    }
}
