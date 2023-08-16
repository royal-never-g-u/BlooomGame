using Newtonsoft.Json;
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
    private SaveData saveData;             //�浵����
    private int saveIndex = 0;             //�浵λ���
    public void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        //��ʼ���浵����
        InitSaveData();
    }

    /// <summary>
    /// ��ʼ��
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
            //����ļ�·��������
            saveData = new SaveData() { 
                prefer = new PlayerPrefer(),
            };
            string json = JsonConvert.SerializeObject(saveData);
            File.WriteAllText(Application.dataPath + "/SaveData.txt", json);
            AssetDatabase.Refresh();  //�༭��ˢ��
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
        File.WriteAllText(Application.dataPath + "/SaveData.txt", json);
        AssetDatabase.Refresh();  //�༭��ˢ��
    }

    /// <summary>
    /// Ӧ�����ƫ��
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
