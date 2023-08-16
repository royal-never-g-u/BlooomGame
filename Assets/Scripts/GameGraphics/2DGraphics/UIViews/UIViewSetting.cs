using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameGraphics
{

    /// <summary>
    /// 游戏设置页面
    /// </summary>
	public class UIViewSetting : UIViewSettingBase
    {
        public static float musicVolume = 1.0f;
        public static float bgmVolume = 1.0f;
        public static float soundVolume = 1.0f;
        public static bool isFullScreen = true;
        public static int resolutionIndex = 0;

        /// <summary>
        /// 初始化
        /// </summary>
        protected override void OnEnter(object data = null)
        {
            //初始化
            LoadPrefer(SaveManager.Instance.LoadPlayerPrefer());
            //给滑块添加响应
            _sliderMusicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
            _sliderBgmSlider.onValueChanged.AddListener(OnBgmSliderValueChanged);
            _sliderSoundSlider.onValueChanged.AddListener(OnSoundSliderValueChanged);
            //根据偏好设置改变UI显示
            ChangeStatus();
         }

        /// <summary>
        /// 载入偏好数据
        /// </summary>
        private void LoadPrefer(PlayerPrefer data)
        {
            musicVolume = data.musicVolume;
            bgmVolume = data.bgmVolume;
            soundVolume = data.soundVolume;
            isFullScreen = data.isFullScreen;
            resolutionIndex = data.resolutionIndex;
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        private void ChangeStatus()
        {

            _sliderMusicSlider.value = musicVolume;
            _sliderBgmSlider.value = bgmVolume;
            _sliderSoundSlider.value = soundVolume;

            int width = SaveManager.Instance.resolutionList[resolutionIndex].width;
            int height = SaveManager.Instance.resolutionList[resolutionIndex].height;
            Screen.SetResolution(width, height, isFullScreen);

            _buttonFrameLeftBtn.gameObject.SetActive(!isFullScreen);
            _buttonFrameRightBtn.gameObject.SetActive(isFullScreen);
            if (isFullScreen) _textFrameChoice.text = "全屏";
            else _textFrameChoice.text = "窗口化";

            _buttonResolutionLeftBtn.gameObject.SetActive(resolutionIndex != 0);
            _buttonResolutionRightBtn.gameObject.SetActive(resolutionIndex != SaveManager.Instance.resolutionList.Count - 1);
            _textResolutionChoice.text = width + "x" + height;
        }

        /// <summary>
        /// 保存偏好设置
        /// </summary>
        private void SavePrefer()
        {
            PlayerPrefer prefer = new PlayerPrefer() { 
                musicVolume = musicVolume,
                bgmVolume = bgmVolume,
                soundVolume = soundVolume,
                isFullScreen = isFullScreen,
                resolutionIndex = resolutionIndex,
            };
            SaveManager.Instance.SavePlayerPrefer(prefer);
        }

        /// <summary>
        /// 实现点击其他区域关闭设置页面
        /// </summary>
        protected override void OnButtonPanelClick()
        {
            //关闭当前页面
            Close();
        }

        /// <summary>
        /// 调节音量按钮的响应
        /// </summary>
        private void OnMusicSliderValueChanged(float value)
        {
            musicVolume = value;
            AudioManager.Instance.AdjustMusicVolume(value);
        }


        /// <summary>
        /// 调节Bgm音量按钮的响应
        /// </summary>
        private void OnBgmSliderValueChanged(float value)
        {
            bgmVolume = value;
            AudioManager.Instance.AdjustBgmVolume(value);
        }

        /// <summary>
        /// 调节音效音量按钮的响应
        /// </summary>
        private void OnSoundSliderValueChanged(float value)
        {
            soundVolume = value;
            AudioManager.Instance.AdjustSoundVolume(value);
        }

        /// <summary>
        /// 调节画面（左按钮
        /// </summary>
        protected override void OnButtonFrameLeftBtnClick()
        {
            isFullScreen = true;
            ChangeStatus();
        }

        /// <summary>
        /// 调节画面（右按钮
        /// </summary>
        protected override void OnButtonFrameRightBtnClick()
        {
            isFullScreen = false;
            ChangeStatus();
        }

        /// <summary>
        /// 调节分辨率（左按钮
        /// </summary>
        protected override void OnButtonResolutionLeftBtnClick()
        {
            resolutionIndex--;
            ChangeStatus();
        }

        /// <summary>
        /// 调节分辨率（右按钮
        /// </summary>
        protected override void OnButtonResolutionRightBtnClick()
        {
            resolutionIndex++;
            ChangeStatus();
        }

        /// <summary>
        /// 应用偏好设置
        /// </summary>
        protected override void OnButtonApplyClick()
        {
            SavePrefer();
        }
    }
}
