using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameGraphics
{
    /// <summary>
    /// �ֱ��ʽṹ��
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
    /// ��Ϸ����ҳ��
    /// </summary>
	public class UIViewSetting : UIViewSettingBase
    {
        //������
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
        /// ��ʼ��
        /// </summary>
        protected override void OnEnter(object data = null)
        {
            //��ʼ��
            //�����������Ӧ
            _sliderMusicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
            _sliderBgmSlider.onValueChanged.AddListener(OnBgmSliderValueChanged);
            _sliderSoundSlider.onValueChanged.AddListener(OnSoundSliderValueChanged);
            //����ƫ������
            LoadPlayerPrefer();
         }

        /// <summary>
        /// ʵ�ֵ����������ر�����ҳ��
        /// </summary>
        protected override void OnButtonPanelClick()
        {
            //����ƫ������
            SetPlayerPrefer();
            //�رյ�ǰҳ��
            Close();
        }

        /// <summary>
        /// ����������ť����Ӧ
        /// </summary>
        private void OnMusicSliderValueChanged(float value)
        {
            AudioManager.Instance.AdjustMusicVolume(value);
        }


        /// <summary>
        /// ����Bgm������ť����Ӧ
        /// </summary>
        private void OnBgmSliderValueChanged(float value)
        {
            AudioManager.Instance.AdjustBgmVolume(value);
        }

        /// <summary>
        /// ������Ч������ť����Ӧ
        /// </summary>
        private void OnSoundSliderValueChanged(float value)
        {
            AudioManager.Instance.AdjustSoundVolume(value);
        }

        /// <summary>
        /// ���ڻ��棨��ť
        /// </summary>
        protected override void OnButtonFrameLeftBtnClick()
        {
            isFullScreen = true;
            ChangeItem();
        }

        /// <summary>
        /// ���ڻ��棨�Ұ�ť
        /// </summary>
        protected override void OnButtonFrameRightBtnClick()
        {
            isFullScreen = false;
            ChangeItem();
        }

        /// <summary>
        /// ���ڷֱ��ʣ���ť
        /// </summary>
        protected override void OnButtonResolutionLeftBtnClick()
        {
            resolutionIndex--;
            ChangeItem();
        }

        /// <summary>
        /// ���ڷֱ��ʣ��Ұ�ť
        /// </summary>
        protected override void OnButtonResolutionRightBtnClick()
        {
            resolutionIndex++;
            ChangeItem();
        }

        /// <summary>
        /// ����ƫ������
        /// </summary>
        private void LoadPlayerPrefer()
        {
            //1.����
            _sliderMusicSlider.value = PlayerPrefs.GetFloat(musicVolume, 1.0f);
            _sliderBgmSlider.value = PlayerPrefs.GetFloat(bgmVolume, 1.0f);
            _sliderSoundSlider.value = PlayerPrefs.GetFloat(soundVolume, 1.0f);

            //2.���漰�ֱ���
            resolutionIndex = PlayerPrefs.GetInt(resolutIndex, 0);
            isFullScreen = PlayerPrefs.GetInt(fullScreen, 1) == 1 ? true : false;
            ChangeItem();
        }

        /// <summary>
        /// ���桢�ֱ��ʸ���
        /// </summary>
        private void ChangeItem()
        {
            int width = resolutionList[resolutionIndex].width;
            int height = resolutionList[resolutionIndex].height;
            Screen.SetResolution(width, height, isFullScreen);

            //UI���
            _buttonFrameLeftBtn.gameObject.SetActive(!isFullScreen);
            _buttonFrameRightBtn.gameObject.SetActive(isFullScreen);
            if (isFullScreen) _textFrameChoice.text = "ȫ��";
            else _textFrameChoice.text = "���ڻ�";

            _buttonResolutionLeftBtn.gameObject.SetActive(resolutionIndex != 0);
            _buttonResolutionRightBtn.gameObject.SetActive(resolutionIndex != resolutionList.Count - 1);
            _textResolutionChoice.text = width + "x" + height;
        }

        /// <summary>
        /// ����ƫ������
        /// </summary>
        private void SetPlayerPrefer()
        {
            //1.����
            PlayerPrefs.SetFloat(musicVolume, _sliderMusicSlider.value);
            PlayerPrefs.SetFloat(bgmVolume, _sliderBgmSlider.value);
            PlayerPrefs.SetFloat(soundVolume, _sliderSoundSlider.value);

            //2.���漰�ֱ���
            PlayerPrefs.SetInt(resolutIndex,resolutionIndex);
            PlayerPrefs.SetInt(fullScreen, isFullScreen ? 1 : 0);
        }


    }
}
