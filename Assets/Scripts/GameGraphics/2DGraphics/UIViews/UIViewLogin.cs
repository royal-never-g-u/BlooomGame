using GameLogics;
using System;
using System.Collections.Generic;
using UnityEditor;
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

            //�򿪶�ȡ�浵��ť
            List<PlayerData> dataList = SaveManager.Instance.LoadPlayerDataList();
            for(int i = 0; i < dataList.Count; i++)
            {
                if (dataList[i].isExist)
                {
                    _buttonOldGameBtn.interactable = true;
                    break;
                }
            }
        }

        /// <summary>
        /// ��ʼ����Ϸ��ť
        /// </summary>
        protected override void OnButtonNewGameBtnClick()
        {
            Close();
            Open<UIViewExtra>(null, EUIViewOpenType.Overlying, EUIViewPriority.Level5);
            Cursor.lockState = CursorLockMode.Locked;
            //GameObject.Find("Person").AddComponent<PersonComponent>();
        }
        
        /// <summary>
        /// ��ȡ����Ϸ��ť����ȡ�浵
        /// </summary>
        protected override void OnButtonOldGameBtnClick()
        {
            Open<UIViewGameSave>(2, EUIViewOpenType.Overlying, EUIViewPriority.Level5);
        }

        /// <summary>
        /// ��Ϸ���ð�ť
        /// </summary>
        protected override void OnButtonSettingBtnClick()
        {
            //�򿪻�ر�����ҳ��
            Open<UIViewSetting>(false, EUIViewOpenType.Overlying, EUIViewPriority.Level5);
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
            #if UNITY_EDITOR
                EditorApplication.ExitPlaymode();
            #else
                Application.Quit();
            #endif
        }
    }
}
