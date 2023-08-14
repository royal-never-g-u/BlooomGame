/**
 * UnityObjectManager
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameUnityFramework.Resource
{
    public class UnityObjectManager
    {
        /// <summary>
        /// 唯一引用
        /// </summary>
        protected static UnityObjectManager _instance;

        /// <summary>
        /// 公用访问
        /// </summary>
        public static UnityObjectManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UnityObjectManager();
                }
                return _instance;
            }
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        public void AsyncLoad<T>(string path, System.Action<T> callback)
        {
            path = ResourcePathHelper.GetFullResourcePath(path);
            Addressables.LoadAssetAsync<T>(path).Completed += (handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    callback?.Invoke(handle.Result);
                }
            };
        }

        /// <summary>
        /// 异步加载Sprite
        /// </summary>
        public void SyncLoadSrpite(string path, Image image)
        {
            path = ResourcePathHelper.GetFullResourcePath(path);
            Addressables.LoadAssetAsync<Sprite>(path).Completed += (handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    image.sprite = handle.Result;
                }
            };
            return;
        }

        /// <summary>
        /// 同步加载实例化对象资源
        /// </summary>
        public GameObject SyncGameObjectInstantiate(string path, Transform parent)
        {
            path = ResourcePathHelper.GetFullResourcePath(path);
            var handle = Addressables.InstantiateAsync(path, parent);
            var go = handle.WaitForCompletion();
            go.transform.SetParent(parent);
            return go;
        }

        
        /// <summary>
        /// 异步实例化
        /// </summary>
        public void AsyncGameObjectInstantiate(string path, System.Action<GameObject> callback = null)
        {
            path = ResourcePathHelper.GetFullResourcePath(path);
            Addressables.InstantiateAsync(path).Completed += (handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    callback?.Invoke(handle.Result);
                }
            };
        }

        /// <summary>
        /// 异步实例化
        /// </summary>
        public void AsyncGameObjectInstantiate(string path, Transform parent, System.Action<GameObject> callback = null)
        {
            path = ResourcePathHelper.GetFullResourcePath(path);
            Addressables.InstantiateAsync(path, parent).Completed += (handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    callback?.Invoke(handle.Result);
                }
            };
        }

        /// <summary>
        /// 如果在webgl则异步，其他平台可以同步时自动同步
        /// </summary>
        public void SyncOrAsyncLoad(string path, Transform parent, System.Action<GameObject> callback = null)
        {
#if UNITY_EDITOR || !UNITY_WEBGL
            GameObject item = SyncGameObjectInstantiate(path, parent);
            callback?.Invoke(item);
#else
            AsyncGameObjectInstantiate(path, parent, callback);
#endif
        }
    }
}
