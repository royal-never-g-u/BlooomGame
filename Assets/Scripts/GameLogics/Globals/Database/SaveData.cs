using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
/// 玩家数据
/// </summary>
public class PlayerData
{
    public bool isExist = false;     //当前数据是否存在
    public string dateTime = "栏位";

    public int level = 0;                  //等级
}

/// <summary>
/// 存储必要数据
/// </summary>
public class SaveData
{
    //1.游戏偏好（音量、画面、分辨率
    public PlayerPrefer prefer;

    //2.游戏数据（和存档栏一一对应
    public List<PlayerData> dataList;
}

