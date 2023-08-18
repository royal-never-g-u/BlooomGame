using GameLogics;
using System;
using System.Collections.Generic;
using UnityEditor;
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

            //打开读取存档按钮
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
        /// 开始新游戏按钮
        /// </summary>
        protected override void OnButtonNewGameBtnClick()
        {
            Close();
            Open<UIViewExtra>(null, EUIViewOpenType.Overlying, EUIViewPriority.Level5);
            Cursor.lockState = CursorLockMode.Locked;
            //GameObject.Find("Person").AddComponent<PersonComponent>();
        }
        
        /// <summary>
        /// 读取旧游戏按钮（读取存档
        /// </summary>
        protected override void OnButtonOldGameBtnClick()
        {
            Open<UIViewGameSave>(2, EUIViewOpenType.Overlying, EUIViewPriority.Level5);
        }

        /// <summary>
        /// 游戏设置按钮
        /// </summary>
        protected override void OnButtonSettingBtnClick()
        {
            //打开或关闭设置页面
            Open<UIViewSetting>(false, EUIViewOpenType.Overlying, EUIViewPriority.Level5);
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
            #if UNITY_EDITOR
                EditorApplication.ExitPlaymode();
            #else
                Application.Quit();
            #endif
        }
    }
}
