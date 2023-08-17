using GameLogics;
using UnityEngine;

namespace GameGraphics
{
	public class UIViewExtra : UIViewExtraBase
	{
        /// <summary>
        /// ��ʼ��
        /// </summary>
        protected override void OnEnter(object data = null)
        {
            //�����Զ��浵
            Application.quitting += SaveManager.Instance.AutoSaveData;
            //��ʼ��
            InitInfo();
        }

        /// <summary>
        /// ��ʼ����Ϣ
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
        /// ������Ϸ����ҳ��
        /// </summary>
        protected override void OnButtonSettingBtnClick()
        {
            Open<UIViewSetting>(true, EUIViewOpenType.Overlying, EUIViewPriority.Level5);
        }

        /// <summary>
        /// ���Թ��ܣ����ӵȼ�
        /// </summary>
        protected override void OnButtonAddLevelClick()
        {
            SaveManager.Instance.curPlayerLevel++;
            _textLevel.text = SaveManager.Instance.curPlayerLevel.ToString();
        }
    }
}
