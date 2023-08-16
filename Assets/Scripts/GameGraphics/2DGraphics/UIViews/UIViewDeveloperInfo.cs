namespace GameGraphics
{
	/// <summary>
	/// 开发者信息页面
	/// </summary>
	public class UIViewDeveloperInfo : UIViewDeveloperInfoBase
	{
        /// <summary>
        /// 初始化
        /// </summary>
        protected override void OnEnter(object data = null)
        {
            
        }

        /// <summary>
        /// 实现点击其他区域关闭界面
        /// </summary>
        protected override void OnButtonPanelClick()
        {
            Close();
        }
    }
}
