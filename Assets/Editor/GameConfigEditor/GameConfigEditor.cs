/**
 * 全局配置
 */

using System.IO;
using UnityEditor;
using UnityEngine;
using GameLogics;

namespace Editor.GameConfigEditor
{
    public class GameConfigEditor : EditorWindow
    {
        public UnityEditor.Editor Editor;

        /// <summary>
        /// 窗口入口
        /// </summary>
        [MenuItem("Tools/GameConfig")]
        public static void Open()
        {
            var window = EditorWindow.GetWindow<GameConfigEditor>(true, "全局配置");
            //直接根据ScriptableObject构造一个Editor
            var assetPath = Path.Combine("Assets/ArtPacks", GameConfig.AssetPath);
            var data = AssetDatabase.LoadAssetAtPath<GameConfig>(assetPath);
            if (data == null)
            {
                data = ScriptableObject.CreateInstance<GameConfig>();
                var dir = Path.GetDirectoryName(assetPath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                AssetDatabase.CreateAsset(data, assetPath);
            }
            window.Editor = UnityEditor.Editor.CreateEditor(data);
        }

        /// <summary>
        /// 绘制
        /// </summary>
        private void OnGUI()
        {
            //直接调用Inspector的绘制显示
            string[] options = new string[] { "192.168.102.49", "Option 2", "Option 3" };
            //MainLogic.GameConfig.Port=EditorGUILayout.Popup("My Dropdown Menu",0, options);
            this.Editor.OnInspectorGUI();
        }
    }
}
