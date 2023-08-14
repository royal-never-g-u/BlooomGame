
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ShellBuildScript
{
    /// <summary>
    /// 获得shell传入的参数
    /// </summary>
    /// <returns>The arguments.</returns>
    private static Dictionary<string, string> GetShellArgs()
    {
        var args = new Dictionary<string, string>();
        foreach (string arg in System.Environment.GetCommandLineArgs())
        {
            if (arg.StartsWith("@"))
            {
                var splitIndex = arg.IndexOf("=");
                args.Add(arg.Substring(1, splitIndex - 1), arg.Substring(splitIndex + 1));
            }
        }
        return args;
    }

    /// <summary>
    /// APK
    /// </summary>
    [MenuItem("Tools/打包工具/Android")]
    public static void BuildForAndroid()
    {
        try
        {
            var args = GetShellArgs();
            YouHua.TableToAddressable();
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            {
                Debug.Log($"build apk ==> switch active build target to android");
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            }
            var apkName = "zfxq_release.apk";
            var outputPath = args["out"];
            var location = Path.Combine(outputPath, apkName);
            var buildPlayerOptions = new BuildPlayerOptions()
            {
                scenes = new string[] { "Assets/ArtOrigins/Scenes/Home.unity" },
                locationPathName = location,
                target = BuildTarget.Android,
                targetGroup = BuildTargetGroup.Android,
            };
            BuildPipeline.BuildPlayer(buildPlayerOptions);
            AssetDatabase.Refresh();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

    }

    /// <summary>
    /// WebGL
    /// </summary>
    [MenuItem("Tools/打包工具/WebGL")]
    public static void BuildForWeb()
    {
        try
        {
            var args = GetShellArgs();
            YouHua.TableToAddressable();
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.WebGL)
            {
                Debug.Log($"build webgl ==> switch active build target to webgl");
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);
            }
            var outputPath = args["out"];
            var buildPlayerOptions = new BuildPlayerOptions()
            {
                scenes = new string[] { "Assets/ArtOrigins/Scenes/Home.unity" },
                locationPathName = outputPath,
                target = BuildTarget.WebGL,
                targetGroup = BuildTargetGroup.WebGL,
            };
            BuildPipeline.BuildPlayer(buildPlayerOptions);
            AssetDatabase.Refresh();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}
