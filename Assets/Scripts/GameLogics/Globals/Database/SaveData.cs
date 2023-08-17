using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
/// �������
/// </summary>
public class PlayerData
{
    public bool isExist = false;     //��ǰ�����Ƿ����
    public string dateTime = "��λ";

    public int level = 0;                  //�ȼ�
}

/// <summary>
/// �洢��Ҫ����
/// </summary>
public class SaveData
{
    //1.��Ϸƫ�ã����������桢�ֱ���
    public PlayerPrefer prefer;

    //2.��Ϸ���ݣ��ʹ浵��һһ��Ӧ
    public List<PlayerData> dataList;
}

