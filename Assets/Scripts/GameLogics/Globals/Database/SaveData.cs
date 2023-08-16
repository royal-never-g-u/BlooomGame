using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏偏好
/// </summary>
public class PlayerPrefer
{
    public float musicVolume = 1.0f;
    public float bgmVolume = 1.0f;
    public float soundVolume = 1.0f;
    public bool isFullScreen = true;
    public int resolutionIndex = 0;
}

/// <summary>
/// 存储必要数据
/// </summary>
public class SaveData
{
    //1.游戏偏好（音量、画面、分辨率
    public PlayerPrefer prefer;

    //2.游戏数据->List
}

