/**
 * 游戏主入口
 */

using UnityEngine;
using GameBaseFramework.Base;
using GameBaseFramework.Patterns;
using GameLogics;
using GameGraphics;
using UnityEngine.SceneManagement;

public class GameEntrance : MonoBehaviour, ICommand
{
    /// <summary>
    /// 初始化
    /// </summary>
    private void Awake()
    {
        Debuger.EnableLog = true;
        Debuger.Init(new UnityDebugConsole());
        GameObject.DontDestroyOnLoad(transform);
        Application.runInBackground = true;
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    private void Start()
    {
        MainLogic.Init();
        MainGraphic.Init();
        this.SendCommand(new UIViewOpenCommand() { Name = "UIViewLogin" });
    }


    /// <summary>
    /// 渲染驱动
    /// </summary>
    private void Update()
    {
        var deltaTime = Time.deltaTime;
        MainLogic.Update(deltaTime);
        MainGraphic.Update(deltaTime);
    }

    /// <summary>
    /// 退出（android上无法监听此方法，暂时用OnApplicationPause）
    /// </summary>
    private void OnApplicationQuit()
    {
        
    }
    private bool isAppFocused = true;
    private void OnApplicationFocus(bool focus)
    {
        isAppFocused = focus;
        if (isAppFocused)
        {
            //应用程序从后台切到前台
        }
        else {
            //应用程序从前台切到后台
            Debuger.Log("应用程序从前台切到后台");
        }
    }
}

