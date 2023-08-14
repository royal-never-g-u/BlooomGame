/**
 * 全局配置
 */

using UnityEngine;
using GameUnityFramework.Resource;

public class GameConfig : ScriptableObject
{
    /// <summary>
    /// GlobalConfig保存的.asset文件路径
    /// </summary>
    public const string AssetPath = "Datas/Configs/GameConfig.asset";

    public enum HostType
    {
        LOCAL_HOST,
        REMOTE_HOST,
    }
    /// <summary>
    /// IP地址
    /// </summary>
    [SerializeField]
    public HostType Host=HostType.LOCAL_HOST;
    /// <summary>
    /// 端口
    /// </summary>
    public int Port = 10100;

    #region 家园3D场景
    [SerializeField]
    public Vector3 HomeSceneCameraPosition = new Vector3(-40.0f, 70.0f, -40.0f);
    [SerializeField]
    public Vector3 HomeSceneCameraRotation = new Vector3(45.0f, 45.0f, 0.0f);
    [SerializeField]
    public float HomeSceneCameraFOV = 30.0f;
    #endregion
}
