using System.IO;
using UnityEditor;
using UnityEngine;

public class MyEditor
{
    [MenuItem("�ҵĹ���/ɾ��SavaData�浵�ļ�")]
    public static void DeleteSaveData()
    {
        string dataPath = Path.Combine(Application.dataPath, "SaveData.txt");
        if (File.Exists(dataPath))
        {
            File.Delete(dataPath);
            AssetDatabase.Refresh();  //�༭��ˢ��
            Debug.Log("�ļ�ɾ���ˣ�");
        }
        else
        {
            Debug.Log("�ļ�������!");
        }
    }
}
