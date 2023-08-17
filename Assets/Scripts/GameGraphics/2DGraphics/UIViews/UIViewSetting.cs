using GameLogics;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameGraphics
{

    /// <summary>
    /// ��Ϸ����ҳ�棬����һ��boolֵ������ײ���ť��״̬������Ǵ���Ϸ����ҳ�����Ļ�������true��������Ϊfalse
    /// </summary>
	public class UIViewSetting : UIViewSettingBase
    {
        public static float musicVolume = 1.0f;
        public static float bgmVolume = 1.0f;
        public static float soundVolume = 1.0f;
        public static bool isFullScreen = true;
        public static int resolutionIndex = 0;

        /// <summary>
        /// ��ʼ��
        /// </summary>
        protected override void OnEnter(object data = null)
        {
            //�ײ���ť��״̬
            _gameobjectExtraSetting.SetActive((bool)data);
            //��ʼ��
            LoadPrefer(SaveManager.Instance.LoadPlayerPrefer());
            //�����������Ӧ
            _sliderMusicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
            _sliderBgmSlider.onValueChanged.AddListener(OnBgmSliderValueChanged);
            _sliderSoundSlider.onValueChanged.AddListener(OnSoundSliderValueChanged);
            //����ƫ�����øı�UI��ʾ
            ChangeStatus();
         }

        /// <summary>
        /// ����ƫ������
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
        /// ����״̬
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
            if (isFullScreen) _textFrameChoice.text = "ȫ��";
            else _textFrameChoice.text = "���ڻ�";

            _buttonResolutionLeftBtn.gameObject.SetActive(resolutionIndex != 0);
            _buttonResolutionRightBtn.gameObject.SetActive(resolutionIndex != SaveManager.Instance.resolutionList.Count - 1);
            _textResolutionChoice.text = width + "x" + height;
        }

        /// <summary>
        /// ����ƫ������
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
        /// ʵ�ֵ����������ر�����ҳ��
        /// </summary>
        protected override void OnButtonPanelClick()
        {
            //����ƫ��
            LoadPrefer(SaveManager.Instance.LoadPlayerPrefer());
            ChangeStatus();
            //�رյ�ǰҳ��
            Close();
        }

        /// <summary>
        /// ����������ť����Ӧ
        /// </summary>
        private void OnMusicSliderValueChanged(float value)
        {
            musicVolume = value;
            AudioManager.Instance.AdjustMusicVolume(value);
        }


        /// <summary>
        /// ����Bgm������ť����Ӧ
        /// </summary>
        private void OnBgmSliderValueChanged(float value)
        {
            bgmVolume = value;
            AudioManager.Instance.AdjustBgmVolume(value);
        }

        /// <summary>
        /// ������Ч������ť����Ӧ
        /// </summary>
        private void OnSoundSliderValueChanged(float value)
        {
            soundVolume = value;
            AudioManager.Instance.AdjustSoundVolume(value);
        }

        /// <summary>
        /// ���ڻ��棨��ť
        /// </summary>
        protected override void OnButtonFrameLeftBtnClick()
        {
            isFullScreen = true;
            ChangeStatus();
        }

        /// <summary>
        /// ���ڻ��棨�Ұ�ť
        /// </summary>
        protected override void OnButtonFrameRightBtnClick()
        {
            isFullScreen = false;
            ChangeStatus();
        }

        /// <summary>
        /// ���ڷֱ��ʣ���ť
        /// </summary>
        protected override void OnButtonResolutionLeftBtnClick()
        {
            resolutionIndex--;
            ChangeStatus();
        }

        /// <summary>
        /// ���ڷֱ��ʣ��Ұ�ť
        /// </summary>
        protected override void OnButtonResolutionRightBtnClick()
        {
            resolutionIndex++;
            ChangeStatus();
        }

        /// <summary>
        /// Ӧ��ƫ������
        /// </summary>
        protected override void OnButtonApplyClick()
        {
            SavePrefer();
        }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        protected override void OnButtonLoadClick()
        {
            Open<UIViewGameSave>(2, EUIViewOpenType.Overlying, EUIViewPriority.Level5);
        }

        /// <summary>
        /// �������
        /// </summary>
        protected override void OnButtonSaveClick()
        {
            Open<UIViewGameSave>(1, EUIViewOpenType.Overlying, EUIViewPriority.Level5);
        }

        /// <summary>
        /// ���ر���
        /// </summary>
        protected override void OnButtonReturnClick()
        {
            SaveManager.Instance.AutoSaveData();  //�Զ�����
            Application.quitting -= SaveManager.Instance.AutoSaveData;
            Close();
            Open<UIViewLogin>(null, EUIViewOpenType.Overlying, EUIViewPriority.Level5);
        }
    }
}
