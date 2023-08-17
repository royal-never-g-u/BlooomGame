using System.IO;
using UnityEditor;
using UnityEngine;

public class MyEditor
{
    [MenuItem("我的工具/删除SavaData存档文件")]
    public static void DeleteSaveData()
    {
        string dataPath = Path.Combine(Application.dataPath, "SaveData.txt");
        if (File.Exists(dataPath))
        {
            File.Delete(dataPath);
            AssetDatabase.Refresh();  //编辑器刷新
            Debug.Log("文件删除了！");
        }
        else
        {
            Debug.Log("文件不存在!");
        }
    }
}
