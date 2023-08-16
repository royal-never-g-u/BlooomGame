using Newtonsoft.Json;
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
    private SaveData saveData;             //存档数据
    private int saveIndex = 0;             //存档位编号
    public void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        //初始化存档数据
        InitSaveData();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void InitSaveData()
    {
        try
        {
            string json = File.ReadAllText(Application.dataPath + "/SaveData.txt");
            saveData = JsonConvert.DeserializeObject<SaveData>(json);  
        }
        catch (FileNotFoundException)
        {
            //如果文件路径不存在
            saveData = new SaveData() { 
                prefer = new PlayerPrefer(),
            };
            string json = JsonConvert.SerializeObject(saveData);
            File.WriteAllText(Application.dataPath + "/SaveData.txt", json);
            AssetDatabase.Refresh();  //编辑器刷新
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
        File.WriteAllText(Application.dataPath + "/SaveData.txt", json);
        AssetDatabase.Refresh();  //编辑器刷新
    }

    /// <summary>
    /// 应用玩家偏好
    /// </summary>
    public void ApplyPlayerPrefer()
    {
        PlayerPrefer prefer = saveData.prefer;

        AudioManager.Instance.AdjustMusicVolume(prefer.musicVolume);
        AudioManager.Instance.AdjustBgmVolume(prefer.bgmVolume);
        AudioManager.Instance.AdjustSoundVolume(prefer.soundVolume);

        int width = resolutionList[prefer.resolutionIndex].width;
        int height = resolutionList[prefer.resolutionIndex].height;
        Screen.SetResolution(width, height, prefer.isFullScreen);
    }
}
