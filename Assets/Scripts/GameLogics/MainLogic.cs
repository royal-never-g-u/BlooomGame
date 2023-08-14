/**
 * 游戏逻辑入口
 */

using GameBaseFramework.Base;
using GameBaseFramework.Patterns;
using GameLogics.Modules;
using GameBaseFramework.Timer;
using System;

namespace GameLogics
{
    public class MainLogic
    {
        /// <summary>
        /// 表格数据
        /// </summary>
        /// <summary>
        /// 全局数据模块
        /// </summary>
        public static DataModuleManager DataModuleManager { get; private set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            DataModuleManager = new DataModuleManager();
        }

        /// <summary>
        /// 逻辑驱动
        /// </summary>
        public static void Update(float deltaTime)
        {
            TimerManager.Update(deltaTime);
            CommandManager.Update();
        }
    }
}