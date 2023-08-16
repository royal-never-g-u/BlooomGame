using GameLogics;
using UnityEngine;

namespace GameGraphics
{
    /// <summary>
    /// 开始游戏页面
    /// </summary>
	public class UIViewLogin : UIViewLoginBase
    { 

        /// <summary>
        /// 初始化
        /// </summary>
        protected override void OnEnter(object data = null)
        {
            SaveManager.Instance.ApplyPlayerPrefer();  //应用偏好设置
            AudioManager.Instance.PlayBGM("1");        //播放BGM

        }

        /// <summary>
        /// 开始新游戏按钮
        /// </summary>
        protected override void OnButtonNewGameBtnClick()
        {
            
        }
        
        /// <summary>
        /// 读取旧游戏按钮（读取存档
        /// </summary>
        protected override void OnButtonOldGameBtnClick()
        {
            Open<UIViewGameSave>(null, EUIViewOpenType.Overlying, EUIViewPriority.Level5);
        }

        /// <summary>
        /// 游戏设置按钮
        /// </summary>
        protected override void OnButtonSettingBtnClick()
        {
            //打开或关闭设置页面
            Open<UIViewSetting>(null, EUIViewOpenType.Overlying, EUIViewPriority.Level5);
        }

        /// <summary>
        /// 额外信息按钮
        /// </summary>
        protected override void OnButtonInfoBtnClick()
        {
            //打开或关闭开发者信息页面
            Open<UIViewDeveloperInfo>(null, EUIViewOpenType.Overlying, EUIViewPriority.Level5);
        }

        /// <summary>
        /// 退出游戏按钮
        /// </summary>
        protected override void OnButtonQuitBtnClick()
        {
            
        }
    }
}
