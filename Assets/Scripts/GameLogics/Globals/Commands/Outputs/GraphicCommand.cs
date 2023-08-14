/**
 * 表现层跟UI相关的指令
 */

using GameBaseFramework.Patterns;
using System;
#if !TESTQUIZ
using UnityEngine;
#endif

namespace GameLogics
{
    #region UIView
    public enum EUIViewPriority
    {
        Level0,
        Level1,
        Level2,
        Level3,
        Level4,
        Level5,
        Level6,
        Level7,
        Level8,
        Level9,
        Max,
    }

    /// <summary>
    /// UIView打开状态
    /// </summary>
    public enum EUIViewOpenType
    {
        /// <summary>
        /// 会关闭当前除了常驻的所有界面
        /// 再打开新界面
        /// </summary>
        Replace,
        /// <summary>
        /// 直接叠加在当前界面上
        /// </summary>
        Overlying,
        /// <summary>
        /// 常驻永久
        /// </summary>
        Forever
    }

    /// <summary>
    /// 打开界面指令
    /// </summary>
    public class UIViewOpenCommand : Command
    {
        /// <summary>
        /// 界面类型
        /// </summary>
        public Type ViewType;
        /// <summary>
        /// 界面名
        /// </summary>
        public string Name;
        /// <summary>
        /// 参数数据
        /// </summary>
        public object Data;
        /// <summary>
        /// 表现层级优先级
        /// </summary>
        public EUIViewPriority ViewPriority = EUIViewPriority.Level5;
        /// <summary>
        /// UIView打开状态
        /// </summary>
        public EUIViewOpenType ViewOpenType = EUIViewOpenType.Replace;
        /// <summary>
        /// UIView结束时执行的事件，通常用于刷新前一个页面
        /// </summary>
        public Action _EndEvent = null;
    }

    public class UIViewCloseCommand : Command
    {
        /// <summary>
        /// 界面名
        /// </summary>
        public string Name;
    }
    #endregion

    #region 场景
    public class LoadSceneCommand : Command
    {
        public string SceneName;
    }
    public class EnterHomeSceneCommand : Command { }
    public class ExitHomeSceneCommand : Command { }
    #endregion

}
