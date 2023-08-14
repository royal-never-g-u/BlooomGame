/**
 * 状态基类
 */

namespace GameBaseFramework.Patterns
{
    public abstract class BaseState
    {
        /// <summary>
        /// 进入状态
        /// </summary>
        public virtual void Enter() { }
        /// <summary>
        /// 状态的持续更新
        /// </summary>
        public virtual void Update() { }
        /// <summary>
        /// 退出状态
        /// </summary>
        public virtual void Exit() { }
    }
}
