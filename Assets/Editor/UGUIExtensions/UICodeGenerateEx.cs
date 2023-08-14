/*
 * 处理抖题页面复制三次的标签命名
*/
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UICodeGenerateEx
{
    // Start is called before the first frame update
    [MenuItem("Assets/UIPrefabCodeGenerate Ex")]
    static void ExtensionOfTag()
    {
        Transform parent = Selection.activeGameObject.transform;
        ChangeChildTag(parent.Find("<Button>QuizScrollView/Viewport/Content/<GameObject>PageContainerUp"), "Up");
        ChangeChildTag(parent.Find("<Button>QuizScrollView/Viewport/Content/<GameObject>PageContainerDown"), "Down");
        EditorUtility.SetDirty(parent.gameObject);
    }
    static void ChangeChildTag(Transform trans,string str)
    {
        if (trans.childCount == 0)
        {
            return;
        }
        foreach(Transform item in trans)
        {
            if(item.name.StartsWith("<") && item.name.Contains(">"))
            {
                if (item.name.Substring(item.name.Length - str.Length, str.Length) != str)
                {
                    item.name += str;
                }
            }
            ChangeChildTag(item, str);
        }
    }
}
