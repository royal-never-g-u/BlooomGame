using GameLogics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameGraphics
{
    /// <summary>
    /// ��Ϸ�浵ҳ�棬����һ��intֵ��1����浵ģʽ��2�������ģʽ
    /// </summary>
	public class UIViewGameSave : UIViewGameSaveBase
	{
        private int type = 0;
        private List<PlayerData> dataList;
        private List<Button> slotButton;
        private Button curSlotButton;
        private int curSlotIndex = 0;     //��ǰѡ�����λ

        /// <summary>
        /// ��ʼ��
        /// </summary>
        protected override void OnEnter(object data = null)
        {
            //ȷ����ǰģʽ-�浵or����
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

            //��λ�б��ʼ��
            slotButton = new List<Button>()
            {
                _buttonSlot_1,
                _buttonSlot_2,
                _buttonSlot_3,
            };

            //����浵���ݣ���ʼ��ҳ��
            LoadDataList();
            InitStatus();
        }

        /// <summary>
        /// �˳���ҳ��
        /// </summary>
        protected override void OnButtonPanelClick()
        {
            Close();
        }

        /// <summary>
        /// ��������
        /// </summary>
        public void LoadDataList()
        {
            dataList = SaveManager.Instance.LoadPlayerDataList();
        }

        /// <summary>
        /// ����ҳ��״̬
        /// </summary>
        public void InitStatus()
        {
            //������λ����
            for(int i = 0; i < dataList.Count; i++)
            {
                slotButton[i].transform.Find("Text").GetComponent<Text>().text = dataList[i].dateTime;
            }
        }

        /// <summary>
        /// ȡ����λѡ��
        /// </summary>
        protected override void OnButtonCancelPanelClick()
        {
            if (curSlotButton != null)
            {
                ChangeSlotButtonsStatus(curSlotButton);
            }
        }

        /// <summary>
        /// ��λ1
        /// </summary>
        protected override void OnButtonSlot_1Click()
        {
            curSlotIndex = 1;
            ChangeSlotButtonsStatus(_buttonSlot_1);
        }

        /// <summary>
        /// ��λ2
        /// </summary>
        protected override void OnButtonSlot_2Click()
        {
            curSlotIndex = 2;
            ChangeSlotButtonsStatus(_buttonSlot_2);
        }

        /// <summary>
        /// ��λ3
        /// </summary>
        protected override void OnButtonSlot_3Click()
        {
            curSlotIndex = 3;
            ChangeSlotButtonsStatus(_buttonSlot_3);
        }

        /// <summary>
        /// �ı�SlotButton״̬
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
        /// �ı�SelectButton��CancelPanel��״̬��isOnClick
        /// </summary>
        private void ChangeSelectButtonsStatus()
        {
            //1.��ť״̬ 1-�浵 2-����
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

            //2.ȡ������״̬
            _buttonCancelPanel.gameObject.SetActive(curSlotIndex == 0 ? false : true);
        }

        /// <summary>
        /// �浵
        /// </summary>
        protected override void OnButtonSaveClick()
        {
            SaveManager.Instance.SaveData(curSlotIndex);  //�������
            LoadDataList();
            InitStatus();
        }

        /// <summary>
        /// ����
        /// </summary>
        protected override void OnButtonReadClick()
        {
            SaveManager.Instance.curSlotIndex = curSlotIndex;
            Close();
            Close<UIViewLogin>();
            Open<UIViewExtra>(null, EUIViewOpenType.Overlying, EUIViewPriority.Level5);
        }

        /// <summary>
        /// ɾ���浵
        /// </summary>
        protected override void OnButtonDeleteClick()
        {
            SaveManager.Instance.DeleteData(curSlotIndex);  //ɾ������
            LoadDataList();
            InitStatus();
            ChangeSelectButtonsStatus();
        }
    }
}
