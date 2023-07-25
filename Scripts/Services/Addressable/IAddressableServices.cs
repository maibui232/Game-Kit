namespace GDK.Scripts.Services.Addressable
{
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    public interface IAddressableServices
    {
        UniTask<TAsset> LoadAsset<TAsset>(object key) where TAsset : Object;
    }

    public class AddressableServices : IAddressableServices
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
    }
}