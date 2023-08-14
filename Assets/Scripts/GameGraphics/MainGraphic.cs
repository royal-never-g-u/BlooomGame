/**
 * 游戏渲染入口
 */

using GameGraphics.HomeWorld;

namespace GameGraphics
{
    public class MainGraphic
    {
        #region 私有模块
        public static UIViewManager UIViewManager { get; set; }
        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            UIViewManager = new UIViewManager();
        }

        /// <summary>
        /// 渲染更新驱动
        /// </summary>
        /// <param name="deltaTime"></param>
        public static void Update(float deltaTime)
        {
            UIViewManager?.Update();
        }
    }
}