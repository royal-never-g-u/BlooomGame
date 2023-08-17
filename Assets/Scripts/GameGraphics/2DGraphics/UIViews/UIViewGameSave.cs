using GameLogics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameGraphics
{
    /// <summary>
    /// 游戏存档页面，传入一个int值，1代表存档模式，2代表读档模式
    /// </summary>
	public class UIViewGameSave : UIViewGameSaveBase
	{
        private int type = 0;
        private List<PlayerData> dataList;
        private List<Button> slotButton;
        private Button curSlotButton;
        private int curSlotIndex = 0;     //当前选择的栏位

        /// <summary>
        /// 初始化
        /// </summary>
        protected override void OnEnter(object data = null)
        {
            //确定当前模式-存档or读档
            type = (int)data;
            if (type == 1)
            {
                _buttonSave.gameObject.SetActive(true);
                _buttonDelete.gameObject.SetActive(true);
            }else if (type == 2)
            {
                _buttonRead.gameObject.SetActive(true);
                _buttonDelete.gameObject.SetActive(true);
            }
            ChangeSelectButtonsStatus();

            //栏位列表初始化
            slotButton = new List<Button>()
            {
                _buttonSlot_1,
                _buttonSlot_2,
                _buttonSlot_3,
            };

            //载入存档数据，初始化页面
            LoadDataList();
            InitStatus();
        }

        /// <summary>
        /// 退出本页面
        /// </summary>
        protected override void OnButtonPanelClick()
        {
            Close();
        }

        /// <summary>
        /// 载入数据
        /// </summary>
        public void LoadDataList()
        {
            dataList = SaveManager.Instance.LoadPlayerDataList();
        }

        /// <summary>
        /// 更新页面状态
        /// </summary>
        public void InitStatus()
        {
            //更改栏位文字
            for(int i = 0; i < dataList.Count; i++)
            {
                slotButton[i].transform.Find("Text").GetComponent<Text>().text = dataList[i].dateTime;
            }
        }

        /// <summary>
        /// 取消栏位选择
        /// </summary>
        protected override void OnButtonCancelPanelClick()
        {
            if (curSlotButton != null)
            {
                ChangeSlotButtonsStatus(curSlotButton);
            }
        }

        /// <summary>
        /// 栏位1
        /// </summary>
        protected override void OnButtonSlot_1Click()
        {
            curSlotIndex = 1;
            ChangeSlotButtonsStatus(_buttonSlot_1);
        }

        /// <summary>
        /// 栏位2
        /// </summary>
        protected override void OnButtonSlot_2Click()
        {
            curSlotIndex = 2;
            ChangeSlotButtonsStatus(_buttonSlot_2);
        }

        /// <summary>
        /// 栏位3
        /// </summary>
        protected override void OnButtonSlot_3Click()
        {
            curSlotIndex = 3;
            ChangeSlotButtonsStatus(_buttonSlot_3);
        }

        /// <summary>
        /// 改变SlotButton状态
        /// </summary>
        private void ChangeSlotButtonsStatus(Button buttonSlot)
        {
            if (curSlotButton == null)
            {
                curSlotButton = buttonSlot;
                curSlotButton.transform.Find("Back").gameObject.SetActive(true);
            }
            else if (curSlotButton == buttonSlot)
            {
                curSlotButton = null;
                buttonSlot.transform.Find("Back").gameObject.SetActive(false);
                curSlotIndex = 0;
            }
            else
            {
                curSlotButton.transform.Find("Back").gameObject.SetActive(false);
                curSlotButton = buttonSlot;
                curSlotButton.transform.Find("Back").gameObject.SetActive(true);
            }
            ChangeSelectButtonsStatus();
        }

        /// <summary>
        /// 改变SelectButton和CancelPanel的状态，isOnClick
        /// </summary>
        private void ChangeSelectButtonsStatus()
        {
            //1.按钮状态 1-存档 2-读档
            if (type == 1)
            {
                _buttonSave.interactable = curSlotIndex == 0 ? false : true;
            }else if (type == 2)
            {
                if (curSlotIndex == 0) _buttonRead.interactable = false;
                else _buttonRead.interactable = SaveManager.Instance.LoadPlayerData(curSlotIndex).isExist;
            }
            if (curSlotIndex == 0) _buttonDelete.interactable = false;
            else  _buttonDelete.interactable = SaveManager.Instance.LoadPlayerData(curSlotIndex).isExist;

            //2.取消面板的状态
            _buttonCancelPanel.gameObject.SetActive(curSlotIndex == 0 ? false : true);
        }

        /// <summary>
        /// 存档
        /// </summary>
        protected override void OnButtonSaveClick()
        {
            SaveManager.Instance.SaveData(curSlotIndex);  //保存进度
            LoadDataList();
            InitStatus();
        }

        /// <summary>
        /// 读档
        /// </summary>
        protected override void OnButtonReadClick()
        {
            SaveManager.Instance.curSlotIndex = curSlotIndex;
            Close();
            Close<UIViewLogin>();
            Open<UIViewExtra>(null, EUIViewOpenType.Overlying, EUIViewPriority.Level5);
        }

        /// <summary>
        /// 删除存档
        /// </summary>
        protected override void OnButtonDeleteClick()
        {
            SaveManager.Instance.DeleteData(curSlotIndex);  //删除进度
            LoadDataList();
            InitStatus();
            ChangeSelectButtonsStatus();
        }
    }
}
