using GameLogics;
using UnityEngine;

namespace GameGraphics
{
	public class UIViewExtra : UIViewExtraBase
	{
        /// <summary>
        /// 初始化
        /// </summary>
        protected override void OnEnter(object data = null)
        {
            //订阅自动存档
            Application.quitting += SaveManager.Instance.AutoSaveData;
            //初始化
            InitInfo();
        }

        /// <summary>
        /// 初始化信息
        /// </summary>
        private void InitInfo()
        {
            int index = SaveManager.Instance.curSlotIndex;
            if (index > 0)
            {
                int level = SaveManager.Instance.LoadPlayerData(index).level;
                _textLevel.text = level.ToString();
                SaveManager.Instance.curPlayerLevel = level;
            }
        }

        /// <summary>
        /// 进入游戏设置页面
        /// </summary>
        protected override void OnButtonSettingBtnClick()
        {
            Open<UIViewSetting>(true, EUIViewOpenType.Overlying, EUIViewPriority.Level5);
        }

        /// <summary>
        /// 测试功能：增加等级
        /// </summary>
        protected override void OnButtonAddLevelClick()
        {
            SaveManager.Instance.curPlayerLevel++;
            _textLevel.text = SaveManager.Instance.curPlayerLevel.ToString();
        }
    }
}
