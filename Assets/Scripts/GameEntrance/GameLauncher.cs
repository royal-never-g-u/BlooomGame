/**
 * 游戏主入口
 */

using UnityEngine;
using GameUnityFramework.Resource;

public class GameLauncher : MonoBehaviour
{
    /// <summary>
    /// 初始化
    /// </summary>
    private void Awake()
    {
        var gameEntrance = GameObject.Find("GameEntrance");
        if (gameEntrance == null)
        {
            UnityObjectManager.Instance.AsyncGameObjectInstantiate("GameEntrance", (go) =>
            {
                go.name = "GameEntrance";
            });
        }
    }
}

