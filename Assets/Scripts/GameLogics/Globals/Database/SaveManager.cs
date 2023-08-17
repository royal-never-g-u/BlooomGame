using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// �ֱ��ʽṹ��
/// </summary>
public struct Resolution
{
    public int width;
    public int height;
    public Resolution(int x, int y)
    {
        width = x;
        height = y;
    }
}

/// <summary>
/// ʵ�ִ浵ϵͳ
/// </summary>
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;    //����
    public List<Resolution> resolutionList = new List<Resolution>()
    {
        new Resolution(1920,1080),
        new Resolution(1600,900),
    };
    private SaveData saveData;                   //�浵����
    public string dataPath;                      //�浵����λ��

    public int curSlotIndex = 0;                 //��ǰ��ȡ��λ
    public int curPlayerLevel = 0;               //��ҵȼ�
    public void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        dataPath = Path.Combine(Application.dataPath, "SaveData.txt");

        //��ʼ���浵����
        InitSaveData();
    }

    /// <summary>
    /// ��ʼ��
    /// </summary>
    private void InitSaveData()
    {
        if (File.Exists(dataPath))
        {
            string json = File.ReadAllText(dataPath);
            saveData = JsonConvert.DeserializeObject<SaveData>(json);
        }
        else
        {
            saveData = new SaveData() { 
                prefer = new PlayerPrefer(),
                dataList = new List<PlayerData>() {
                    new PlayerData(),  //��λ1��2��3
                    new PlayerData(),
                    new PlayerData(),
                },
            };
        }
    }

    /// <summary>
    /// ��ȡ���ƫ��
    /// </summary>
    public PlayerPrefer LoadPlayerPrefer()
    {
        return saveData.prefer;
    }

    /// <summary>
    /// �������ƫ��
    /// </summary>
    public void SavePlayerPrefer(PlayerPrefer prefer)
    {
        saveData.prefer = prefer;
        string json = JsonConvert.SerializeObject(saveData);
        File.WriteAllText(dataPath, json);
        AssetDatabase.Refresh();  //�༭��ˢ��
    }

    /// <summary>
    /// Ӧ�����ƫ��
    /// </summary>
    public void ApplyPlayerPrefer()
    {
        PlayerPrefer prefer = LoadPlayerPrefer();

        AudioManager.Instance.AdjustMusicVolume(prefer.musicVolume);
        AudioManager.Instance.AdjustBgmVolume(prefer.bgmVolume);
        AudioManager.Instance.AdjustSoundVolume(prefer.soundVolume);

        int width = resolutionList[prefer.resolutionIndex].width;
        int height = resolutionList[prefer.resolutionIndex].height;
        Screen.SetResolution(width, height, prefer.isFullScreen);
    }

    /// <summary>
    /// ��ȡ��������б�
    /// </summary>
    public List<PlayerData> LoadPlayerDataList()
    {
        return saveData.dataList;
    }

    /// <summary>
    /// ��ȡ�������
    /// </summary>
    /// <returns></returns>
    public PlayerData LoadPlayerData(int index)
    {
        return LoadPlayerDataList()[index-1];
    }

    /// <summary>
    /// ��ȡ�浵����
    /// </summary>
    public SaveData LoadSaveData()
    {
        return saveData;
    }

    /// <summary>
    /// �Զ��浵��Ĭ��Ϊ��λ1
    /// </summary>
    public void AutoSaveData()
    {
        SaveData(1);
    }

    /// <summary>
    /// �浵��index->�ڼ�����λ����λ1��Ϊ1
    /// </summary>
    public void SaveData(int index)
    {
        List<PlayerData> list = LoadPlayerDataList();
        list[index-1] = GetCurPlayerData();
        string json = JsonConvert.SerializeObject(saveData);
        File.WriteAllText(dataPath, json);
        AssetDatabase.Refresh();           //�༭��ˢ��
    }

    /// <summary>
    /// ������������ȡ��ǰ�������
    /// </summary>
    private PlayerData GetCurPlayerData()
    {
        DateTime curTime = DateTime.Now;
        string str = curTime.ToString("yyyy/MM/dd HH:mm:ss");
        PlayerData data = new PlayerData()
        {
            isExist = true,
            dateTime = str,
            level = curPlayerLevel,
        };
        return data;
    }

    /// <summary>
    /// ɾ���浵��index->�ڼ�����λ����λ1��Ϊ1
    /// </summary>
    public void DeleteData(int index)
    {
        List<PlayerData> list = LoadPlayerDataList();
        list[index - 1] = new PlayerData();
        string json = JsonConvert.SerializeObject(saveData);
        File.WriteAllText(dataPath, json);
        AssetDatabase.Refresh();           //�༭��ˢ��
    }
}
