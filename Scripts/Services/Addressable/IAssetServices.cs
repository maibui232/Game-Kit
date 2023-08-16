namespace GameKit.Services.Addressable
{
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;

    public interface IAssetServices
    {
        UniTask<TAsset>        LoadAssetAsync<TAsset>(object key) where TAsset : Object;
        UniTask<SceneInstance> LoadSceneAsync(object key, LoadSceneMode loadSceneMode = LoadSceneMode.Single, bool activeOnLoad = true, int priority = 100);
        UniTask                UnloadSceneAsync(SceneInstance scene, UnloadSceneOptions options, bool autoReleaseHandler = true);
    }

    public class AssetServices : IAssetServices
    {
        private readonly Dictionary<object, Object> keyToAsset = new();

        public async UniTask<TAsset> LoadAssetAsync<TAsset>(object key) where TAsset : Object
        {
            if (this.keyToAsset.TryGetValue(key, out var asset))
            {
                return asset as TAsset;
            }

            return await Addressables.LoadAssetAsync<TAsset>(key);
        }

        public async UniTask<SceneInstance> LoadSceneAsync(object key, LoadSceneMode loadSceneMode = LoadSceneMode.Single, bool activeOnLoad = true, int priority = 100)
        {
            return await Addressables.LoadSceneAsync(key, loadSceneMode, activeOnLoad, priority);
        }

        public async UniTask UnloadSceneAsync(SceneInstance scene, UnloadSceneOptions options, bool autoReleaseHandler = true)
        {
            await Addressables.UnloadSceneAsync(scene, options, autoReleaseHandler);
        }
    }
}