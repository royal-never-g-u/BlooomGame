﻿/**
 * UIPrefab转代码
 */

using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Editor.UIPrefabCodeGenerate
{
    public class UICodeGenerate
    {
        /// <summary>
        /// 基类代码导出路径
        /// </summary>
        public static string BaseCodeGenerateExportPath = "Scripts/GameGraphics/2DGraphics/UIViews/AutoGenerated";
        /// <summary>
        /// 实现类代码导出路径
        /// </summary>
        public static string ViewCodeGenerateExportPath = "Scripts/GameGraphics/2DGraphics/UIViews";
        
        [MenuItem("Assets/UIPrefabCodeGenerate")]
        public static void Generate()
        {
            GenerateBaseCode();
            GenerateViewCode();
            GenerateUIViewConfig();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 生成视图基类代码
        /// </summary>
        private static void GenerateBaseCode()
        {
            var sb = new StringBuilder();
            sb.Append(LineText("/**"));
            sb.Append(LineText(" * AutoGenerated Code By UIPrefabCodeGenerate"));
            sb.Append(LineText(" */\n"));
            sb.Append(LineText("using UnityEngine;"));
            sb.Append(LineText("using UnityEngine.UI;"));
            sb.Append(LineText("using GameUnityFramework.Extensions;\n"));
            var prefab = Selection.activeObject as GameObject;
            sb.Append(LineText($"namespace GameGraphics"));
            sb.Append(LineText("{"));
            sb.Append(LineText($"public class {prefab.name}Base : UIViewBase", 1));
            sb.Append(LineText("{", 1));
            var childs = prefab.transform.GetComponentsInChildren<Transform>(true);
            var bindSB = new StringBuilder();
            bindSB.Append(LineText($"protected override void BindWidgets()", 2));
            bindSB.Append(LineText("{", 2));
            for (int i = 0; i < childs.Length; i++)
            {
                var componentLineText = GetComponentLineText(childs[i], prefab.transform, bindSB);
                if (!string.IsNullOrEmpty(componentLineText))
                {
                    sb.Append(componentLineText);
                }
            }
            bindSB.Append(LineText("}", 2));
            sb.Append(bindSB);
            sb.Append(LineText($"protected override string GetPrefabPath()", 2));
            sb.Append(LineText("{", 2));
            var prefabePath = AssetDatabase.GetAssetPath(prefab);
            prefabePath = prefabePath.Replace("Assets/ArtPacks/", "");
            sb.Append(LineText($"return \"{prefabePath}\";", 3));
            sb.Append(LineText("}", 2));

            sb.Append(LineText("}", 1));
            sb.Append(LineText("}"));
            var dirPath = Path.Combine(Application.dataPath, BaseCodeGenerateExportPath);
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
            var baseCodePath = Path.Combine(dirPath, $"{prefab.name}Base.cs");
            File.WriteAllText(baseCodePath, sb.ToString());
           
        }

        /// <summary>
        /// 如果手动创建了则跳过
        /// 否则默认创建一个继承基类的实现类
        /// </summary>
        private static void GenerateViewCode()
        {
            var sb = new StringBuilder();
            var prefab = Selection.activeObject as GameObject;
            sb.Append(LineText($"namespace GameGraphics"));
            sb.Append(LineText("{"));
            sb.Append(LineText($"public class {prefab.name} : {prefab.name}Base", 1));
            sb.Append(LineText("{", 1));
            sb.Append(LineText("}", 1));
            sb.Append(LineText("}"));
            var dirPath = Path.Combine(Application.dataPath, ViewCodeGenerateExportPath);
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
            var viewCodePath = Path.Combine(dirPath, $"{prefab.name}.cs");
            if (!File.Exists(viewCodePath))
            {
                File.WriteAllText(viewCodePath, sb.ToString());
            }
        }

        /// <summary>
        /// 自动生成UIView配置
        /// UIViewName -> UIVIewType
        /// 初始化要手动生成 UIViewConfig
        /// </summary>
        private static void GenerateUIViewConfig()
        {
            var sb = new StringBuilder();
            sb.Append(LineText("/**"));
            sb.Append(LineText(" * AutoGenerated Code By UIPrefabCodeGenerate ==> CodeGenerate"));
            sb.Append(LineText(" */\n"));
            sb.Append(LineText("using System;"));
            sb.Append(LineText("using System.Collections.Generic;\n"));
            sb.Append(LineText($"namespace GameGraphics"));
            sb.Append(LineText("{"));
            sb.Append(LineText($"public partial class UIViewConfig", 1));
            sb.Append(LineText("{", 1));
            sb.Append(LineText($"public static Dictionary<string, Type> NameToTypeDict = new Dictionary<string, Type>()", 2));
            sb.Append(LineText("{", 2));
            if (GameGraphics.UIViewConfig.NameToTypeDict != null)
            {
                foreach(var item in GameGraphics.UIViewConfig.NameToTypeDict)
                {
                    sb.Append(LineText("{" + $"\"{item.Key}\", typeof({item.Key})" + "},", 3));
                }
                var prefab = Selection.activeObject as GameObject;
                if (!GameGraphics.UIViewConfig.NameToTypeDict.ContainsKey(prefab.name))
                {
                    sb.Append(LineText("{" + $"\"{prefab.name}\", typeof({prefab.name})" + "},", 3));
                }
            }
            sb.Append(LineText("};", 2));
            sb.Append(LineText("}", 1));
            sb.Append(LineText("}"));
            var dirPath = Path.Combine(Application.dataPath, BaseCodeGenerateExportPath);
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
            File.WriteAllText(Path.Combine(dirPath, "UIViewConfig.cs"), sb.ToString());
        }

        private static string GetComponentLineText(Transform node, Transform root, StringBuilder bindSB)
        {
            var name = node.name;
            if (name.StartsWith("<") && name.Contains(">"))
            {
                var index = name.IndexOf(">");
                var componentType = name.Substring(1, index - 1);
                var componentName = $"_{componentType.ToLower()}{name.Substring(index + 1)}";

                var _nodePath = name;
                var temp = node;
                while (temp.parent != null && temp.parent != root)
                {
                    _nodePath = temp.parent.name + "/" + _nodePath;
                    temp = temp.parent;
                }
                var result = LineText($"protected {componentType} {componentName};", 2);
                var appendStr1 = LineText($"{componentName} = _root.transform.Find(\"{_nodePath}\").GetComponent<{componentType}>();", 3);
                var appendStr2 = "";
                switch (componentType)
                {
                    case "GameObject":
                        appendStr1 = LineText($"{componentName} = _root.transform.Find(\"{_nodePath}\").gameObject;", 3);
                        break;
                    case "Button":
                        var btnClickName = $"On{componentType}{name.Substring(index + 1)}Click";
                        result += LineText($"protected virtual void {btnClickName}()" + "{ }", 2);
                        appendStr2 = LineText($"{componentName}.onClick.AddListener({btnClickName});", 3);
                        break;
                    case "MutexBtn":
                        var mutexBtnClickName = $"OnButton{name.Substring(index + 1)}Click";
                        result = LineText($"protected Button {componentName};", 2);
                        appendStr1 = LineText($"{componentName} = _root.transform.Find(\"{_nodePath}\").GetComponent<Button>();", 3);
                        result += LineText($"protected virtual void {mutexBtnClickName}()" + "{ }", 2);
                        appendStr2 = LineText(componentName + ".onClick.AddListener(()=>{ ChangeMutexBtnState(" + componentName + ");" + mutexBtnClickName + "();});", 3);
                        break;
                    case "Toggle":
                        var toggleClickName = $"On{componentType}{name.Substring(index + 1)}Click";
                        result += LineText($"protected virtual void {toggleClickName}(bool value)" + "{ }", 2);
                        appendStr2 = LineText($"{componentName}.onValueChanged.AddListener({toggleClickName});", 3);
                        break;
                }
                bindSB.Append(appendStr1);
                bindSB.Append(appendStr2);
                return result;
            }
            return "";
        }

        private static string LineText(string text, int tabCount = 0)
        {
            string result = "";
            for (int i = 1; i <= tabCount; i++)
            {
                result = "\t" + result;
            }
            return result + text + "\n";
        }
    }
}