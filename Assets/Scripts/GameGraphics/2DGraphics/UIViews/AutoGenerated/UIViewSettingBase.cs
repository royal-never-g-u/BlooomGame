/**
 * AutoGenerated Code By UIPrefabCodeGenerate
 */

using UnityEngine;
using UnityEngine.UI;
using GameUnityFramework.Extensions;

namespace GameGraphics
{
	public class UIViewSettingBase : UIViewBase
	{
		protected Button _buttonPanel;
		protected virtual void OnButtonPanelClick(){ }
		protected GameObject _gameobjectSettingPanel;
		protected Slider _sliderMusicSlider;
		protected Slider _sliderBgmSlider;
		protected Slider _sliderSoundSlider;
		protected Text _textFrameChoice;
		protected Button _buttonFrameLeftBtn;
		protected virtual void OnButtonFrameLeftBtnClick(){ }
		protected Button _buttonFrameRightBtn;
		protected virtual void OnButtonFrameRightBtnClick(){ }
		protected Text _textResolutionChoice;
		protected Button _buttonResolutionLeftBtn;
		protected virtual void OnButtonResolutionLeftBtnClick(){ }
		protected Button _buttonResolutionRightBtn;
		protected virtual void OnButtonResolutionRightBtnClick(){ }
		protected Button _buttonApply;
		protected virtual void OnButtonApplyClick(){ }
		protected override void BindWidgets()
		{
			_buttonPanel = _root.transform.Find("<Button>Panel").GetComponent<Button>();
			_buttonPanel.onClick.AddListener(OnButtonPanelClick);
			_gameobjectSettingPanel = _root.transform.Find("<GameObject>SettingPanel").gameObject;
			_sliderMusicSlider = _root.transform.Find("<GameObject>SettingPanel/Setting/Music/<Slider>MusicSlider").GetComponent<Slider>();
			_sliderBgmSlider = _root.transform.Find("<GameObject>SettingPanel/Setting/Bgm/<Slider>BgmSlider").GetComponent<Slider>();
			_sliderSoundSlider = _root.transform.Find("<GameObject>SettingPanel/Setting/Sound/<Slider>SoundSlider").GetComponent<Slider>();
			_textFrameChoice = _root.transform.Find("<GameObject>SettingPanel/Setting/Frame/<Text>FrameChoice").GetComponent<Text>();
			_buttonFrameLeftBtn = _root.transform.Find("<GameObject>SettingPanel/Setting/Frame/<Button>FrameLeftBtn").GetComponent<Button>();
			_buttonFrameLeftBtn.onClick.AddListener(OnButtonFrameLeftBtnClick);
			_buttonFrameRightBtn = _root.transform.Find("<GameObject>SettingPanel/Setting/Frame/<Button>FrameRightBtn").GetComponent<Button>();
			_buttonFrameRightBtn.onClick.AddListener(OnButtonFrameRightBtnClick);
			_textResolutionChoice = _root.transform.Find("<GameObject>SettingPanel/Setting/Resolution/<Text>ResolutionChoice").GetComponent<Text>();
			_buttonResolutionLeftBtn = _root.transform.Find("<GameObject>SettingPanel/Setting/Resolution/<Button>ResolutionLeftBtn").GetComponent<Button>();
			_buttonResolutionLeftBtn.onClick.AddListener(OnButtonResolutionLeftBtnClick);
			_buttonResolutionRightBtn = _root.transform.Find("<GameObject>SettingPanel/Setting/Resolution/<Button>ResolutionRightBtn").GetComponent<Button>();
			_buttonResolutionRightBtn.onClick.AddListener(OnButtonResolutionRightBtnClick);
			_buttonApply = _root.transform.Find("<GameObject>SettingPanel/<Button>Apply").GetComponent<Button>();
			_buttonApply.onClick.AddListener(OnButtonApplyClick);
		}
		protected override string GetPrefabPath()
		{
			return "Prefabs/UIView/UIViewSetting.prefab";
		}
	}
}