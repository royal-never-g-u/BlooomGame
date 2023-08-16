using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameGraphics
{
    /// <summary>
    /// 分辨率结构体
    /// </summary>
    public struct Resolution
    {
        public int width;
        public int height;

        public Resolution(int x,int y)
        {
            width = x;
            height = y;
        }
    }


    /// <summary>
    /// 游戏设置页面
    /// </summary>
	public class UIViewSetting : UIViewSettingBase
    {
        //公用量
        public static string musicVolume = "MusicVolume";
        public static string bgmVolume = "BgmVolume";
        public static string soundVolume = "SoundVolume";
        public static string resolutIndex = "ResolutionIndex";
        public static string fullScreen = "IsFullScreen";

        public static int resolutionIndex = 0;
        public static bool isFullScreen = true;
        public static List<Resolution> resolutionList = new List<Resolution>()
        {
            new Resolution(1920,1080),
            new Resolution(1600,900),
        };

        /// <summary>
        /// 初始化
        /// </summary>
        protected override void OnEnter(object data = null)
        {
            //初始化
            //给滑块添加响应
            _sliderMusicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
            _sliderBgmSlider.onValueChanged.AddListener(OnBgmSliderValueChanged);
            _sliderSoundSlider.onValueChanged.AddListener(OnSoundSliderValueChanged);
            //载入偏好设置
            LoadPlayerPrefer();
         }

        /// <summary>
        /// 实现点击其他区域关闭设置页面
        /// </summary>
        protected override void OnButtonPanelClick()
        {
            //保存偏好设置
            SetPlayerPrefer();
            //关闭当前页面
            Close();
        }

        /// <summary>
        /// 调节音量按钮的响应
        /// </summary>
        private void OnMusicSliderValueChanged(float value)
        {
            AudioManager.Instance.AdjustMusicVolume(value);
        }


        /// <summary>
        /// 调节Bgm音量按钮的响应
        /// </summary>
        private void OnBgmSliderValueChanged(float value)
        {
            AudioManager.Instance.AdjustBgmVolume(value);
        }

        /// <summary>
        /// 调节音效音量按钮的响应
        /// </summary>
        private void OnSoundSliderValueChanged(float value)
        {
            AudioManager.Instance.AdjustSoundVolume(value);
        }

        /// <summary>
        /// 调节画面（左按钮
        /// </summary>
        protected override void OnButtonFrameLeftBtnClick()
        {
            isFullScreen = true;
            ChangeItem();
        }

        /// <summary>
        /// 调节画面（右按钮
        /// </summary>
        protected override void OnButtonFrameRightBtnClick()
        {
            isFullScreen = false;
            ChangeItem();
        }

        /// <summary>
        /// 调节分辨率（左按钮
        /// </summary>
        protected override void OnButtonResolutionLeftBtnClick()
        {
            resolutionIndex--;
            ChangeItem();
        }

        /// <summary>
        /// 调节分辨率（右按钮
        /// </summary>
        protected override void OnButtonResolutionRightBtnClick()
        {
            resolutionIndex++;
            ChangeItem();
        }

        /// <summary>
        /// 载入偏好设置
        /// </summary>
        private void LoadPlayerPrefer()
        {
            //1.音量
            _sliderMusicSlider.value = PlayerPrefs.GetFloat(musicVolume, 1.0f);
            _sliderBgmSlider.value = PlayerPrefs.GetFloat(bgmVolume, 1.0f);
            _sliderSoundSlider.value = PlayerPrefs.GetFloat(soundVolume, 1.0f);

            //2.画面及分辨率
            resolutionIndex = PlayerPrefs.GetInt(resolutIndex, 0);
            isFullScreen = PlayerPrefs.GetInt(fullScreen, 1) == 1 ? true : false;
            ChangeItem();
        }

        /// <summary>
        /// 画面、分辨率更改
        /// </summary>
        private void ChangeItem()
        {
            int width = resolutionList[resolutionIndex].width;
            int height = resolutionList[resolutionIndex].height;
            Screen.SetResolution(width, height, isFullScreen);

            //UI相关
            _buttonFrameLeftBtn.gameObject.SetActive(!isFullScreen);
            _buttonFrameRightBtn.gameObject.SetActive(isFullScreen);
            if (isFullScreen) _textFrameChoice.text = "全屏";
            else _textFrameChoice.text = "窗口化";

            _buttonResolutionLeftBtn.gameObject.SetActive(resolutionIndex != 0);
            _buttonResolutionRightBtn.gameObject.SetActive(resolutionIndex != resolutionList.Count - 1);
            _textResolutionChoice.text = width + "x" + height;
        }

        /// <summary>
        /// 保存偏好设置
        /// </summary>
        private void SetPlayerPrefer()
        {
            //1.音量
            PlayerPrefs.SetFloat(musicVolume, _sliderMusicSlider.value);
            PlayerPrefs.SetFloat(bgmVolume, _sliderBgmSlider.value);
            PlayerPrefs.SetFloat(soundVolume, _sliderSoundSlider.value);

            //2.画面及分辨率
            PlayerPrefs.SetInt(resolutIndex,resolutionIndex);
            PlayerPrefs.SetInt(fullScreen, isFullScreen ? 1 : 0);
        }


    }
}
