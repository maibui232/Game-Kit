namespace GameKit.Services.Addressable
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;

    public interface IAssetServices
    {
        UniTask<TAsset>     LoadAsset<TAsset>(object key) where TAsset : Object;
        Task<SceneInstance> LoadSceneAsync(object key, LoadSceneMode loadSceneMode = LoadSceneMode.Single, bool activeOnLoad = true, int priority = 100);
    }

    public class AssetServices : IAssetServices
    {
        private readonly Dictionary<object, Object> keyToAsset = new();

        public async UniTask<TAsset> LoadAsset<TAsset>(object key) where TAsset : Object
        {
            if (this.keyToAsset.TryGetValue(key, out var asset))
            {
                return asset as TAsset;
            }

            return await Addressables.LoadAssetAsync<TAsset>(key);
        }

        public async Task<SceneInstance> LoadSceneAsync(object key, LoadSceneMode loadSceneMode = LoadSceneMode.Single, bool activeOnLoad = true, int priority = 100)
        {
            return await Addressables.LoadSceneAsync(key, loadSceneMode, activeOnLoad, priority);
        }
    }
}