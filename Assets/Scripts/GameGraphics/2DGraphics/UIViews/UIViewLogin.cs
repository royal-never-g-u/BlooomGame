using GameLogics;
using UnityEngine;

namespace GameGraphics
{
    /// <summary>
    /// ��ʼ��Ϸҳ��
    /// </summary>
	public class UIViewLogin : UIViewLoginBase
    { 

        /// <summary>
        /// ��ʼ��
        /// </summary>
        protected override void OnEnter(object data = null)
        {
            SaveManager.Instance.ApplyPlayerPrefer();  //Ӧ��ƫ������
            AudioManager.Instance.PlayBGM("1");        //����BGM

        }

        /// <summary>
        /// ��ʼ����Ϸ��ť
        /// </summary>
        protected override void OnButtonNewGameBtnClick()
        {
            
        }
        
        /// <summary>
        /// ��ȡ����Ϸ��ť����ȡ�浵
        /// </summary>
        protected override void OnButtonOldGameBtnClick()
        {
            Open<UIViewGameSave>(null, EUIViewOpenType.Overlying, EUIViewPriority.Level5);
        }

        /// <summary>
        /// ��Ϸ���ð�ť
        /// </summary>
        protected override void OnButtonSettingBtnClick()
        {
            //�򿪻�ر�����ҳ��
            Open<UIViewSetting>(null, EUIViewOpenType.Overlying, EUIViewPriority.Level5);
        }

        /// <summary>
        /// ������Ϣ��ť
        /// </summary>
        protected override void OnButtonInfoBtnClick()
        {
            //�򿪻�رտ�������Ϣҳ��
            Open<UIViewDeveloperInfo>(null, EUIViewOpenType.Overlying, EUIViewPriority.Level5);
        }

        /// <summary>
        /// �˳���Ϸ��ť
        /// </summary>
        protected override void OnButtonQuitBtnClick()
        {
            
        }
    }
}
