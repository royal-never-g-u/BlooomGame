using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ϸƫ��
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
/// �洢��Ҫ����
/// </summary>
public class SaveData
{
    //1.��Ϸƫ�ã����������桢�ֱ���
    public PlayerPrefer prefer;

    //2.��Ϸ����->List
}

