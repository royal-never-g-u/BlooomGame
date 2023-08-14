/**
 * 旧项目升级工具
 */

using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Editor.UpgradeTool
{
    public class UpgradeTool
    {
        [MenuItem("Assets/UpgradeUIPrefabs")]
        public static void Generate()
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            var fileList = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            foreach (var file in fileList)
            {
                if (!file.EndsWith(".meta"))
                {
                    var src = AssetDatabase.LoadAssetAtPath<GameObject>(file);
                    var dest = new GameObject(src.name);
                    dest.AddComponent<RectTransform>();
                    UpgradePrefab(src, dest);
                }
            }
        }

        private static void UpgradePrefab(GameObject src, GameObject dest)
        {
            for(int i = 0; i < src.transform.childCount; i++)
            {
                var oldTr = src.transform.GetChild(i);
                var newTr = new GameObject(oldTr.name).AddComponent<RectTransform>();
                newTr.gameObject.SetActive(oldTr.gameObject.activeSelf);
                newTr.SetParent(dest.transform);
                UpgradePrefab(oldTr.gameObject, newTr.gameObject);
            }
        }
    }
}