/**
 * 逻辑层模块管理
 */

using GameBaseFramework.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameLogics.Modules
{
    public abstract class IActionWithType : Command
    {

        public abstract void Execute();
    }
    public class ActionWithType<T> : IActionWithType
    {
        public Action<T> action;
        public T value;

        public override void Execute()
        {
            action?.Invoke(value);
        }
    }
    public class ActionNoType : Command
    {
        public Action action;
    }
    public class DataModuleManager : ICommand
    {
        #region 数据模块
        #endregion

        #region 整理后的模块
        #endregion

        #region 公用（单属于模块的之后整理到对应模块中）
        public void SendTypeAction<T>(Action<T> action,T value)
        {
            this.SendCommand<IActionWithType>(new ActionWithType<T>()
            {
                action = action,
                value = value,
                SyncExecute=false,
            });
        }
        public void SendAction(Action action)
        {
            if (action != null)
            {
                this.SendCommand<ActionNoType>(new ActionNoType()
                {
                    action = action,
                    SyncExecute = false,
                });
            }
        }
        #endregion
        #region GM模块
        #endregion
        #region 交易所self模块
        #endregion
        # region 交易所market模块
        #endregion
        #region 物品详情模块
        #endregion
        #region 上架模块
        #endregion
        #region 求购模块
        #endregion
        #region 家园消息模块
        #endregion
        #region 家园关注模块
        #endregion
        #region 家园用户资料
        #endregion
        #region 共用模块
        #endregion
        #region 世界消息
        #endregion
    }
}