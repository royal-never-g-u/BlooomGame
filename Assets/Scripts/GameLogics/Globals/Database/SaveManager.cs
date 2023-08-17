using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 分辨率结构体
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
/// 实现存档系统
/// </summary>
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;    //单例
    public List<Resolution> resolutionList = new List<Resolution>()
    {
        new Resolution(1920,1080),
        new Resolution(1600,900),
    };
    private SaveData saveData;                   //存档数据
    public string dataPath;                      //存档数据位置

    public int curSlotIndex = 0;                 //当前读取栏位
    public int curPlayerLevel = 0;               //玩家等级
    public void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        dataPath = Path.Combine(Application.dataPath, "SaveData.txt");

        //初始化存档数据
        InitSaveData();
    }

    /// <summary>
    /// 初始化
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
                    new PlayerData(),  //栏位1、2、3
                    new PlayerData(),
                    new PlayerData(),
                },
            };
        }
    }

    /// <summary>
    /// 获取玩家偏好
    /// </summary>
    public PlayerPrefer LoadPlayerPrefer()
    {
        return saveData.prefer;
    }

    /// <summary>
    /// 保存玩家偏好
    /// </summary>
    public void SavePlayerPrefer(PlayerPrefer prefer)
    {
        saveData.prefer = prefer;
        string json = JsonConvert.SerializeObject(saveData);
        File.WriteAllText(dataPath, json);
        AssetDatabase.Refresh();  //编辑器刷新
    }

    /// <summary>
    /// 应用玩家偏好
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
    /// 获取玩家数据列表
    /// </summary>
    public List<PlayerData> LoadPlayerDataList()
    {
        return saveData.dataList;
    }

    /// <summary>
    /// 获取玩家数据
    /// </summary>
    /// <returns></returns>
    public PlayerData LoadPlayerData(int index)
    {
        return LoadPlayerDataList()[index-1];
    }

    /// <summary>
    /// 获取存档数据
    /// </summary>
    public SaveData LoadSaveData()
    {
        return saveData;
    }

    /// <summary>
    /// 自动存档（默认为栏位1
    /// </summary>
    public void AutoSaveData()
    {
        SaveData(1);
    }

    /// <summary>
    /// 存档，index->第几个栏位，栏位1即为1
    /// </summary>
    public void SaveData(int index)
    {
        List<PlayerData> list = LoadPlayerDataList();
        list[index-1] = GetCurPlayerData();
        string json = JsonConvert.SerializeObject(saveData);
        File.WriteAllText(dataPath, json);
        AssetDatabase.Refresh();           //编辑器刷新
    }

    /// <summary>
    /// 辅助方法：获取当前玩家数据
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
    /// 删除存档，index->第几个栏位，栏位1即为1
    /// </summary>
    public void DeleteData(int index)
    {
        List<PlayerData> list = LoadPlayerDataList();
        list[index - 1] = new PlayerData();
        string json = JsonConvert.SerializeObject(saveData);
        File.WriteAllText(dataPath, json);
        AssetDatabase.Refresh();           //编辑器刷新
    }
}
