namespace GameKit.Services.ObjectPool
{
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameKit.Exception;
    using GameKit.Services.Addressable;
    using UnityEngine;

    public interface IObjectPoolService
    {
        void    CreatePool<T>(T prefab, int initSize) where T : Component;
        UniTask CreatePool<T>(string addressableId, int initSize) where T : Component;

        T          Spawn<T>(T prefab) where T : Component;
        T          Spawn<T>(T prefab, Vector3 position, Transform parent = null, bool worldPositionStays = true) where T : Component;
        T          Spawn<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent = null, bool worldPositionStays = true) where T : Component;
        T          Spawn<T>(T prefab, Vector3 position, Vector3 angle, Transform parent = null, bool worldPositionStays = true) where T : Component;
        UniTask<T> Spawn<T>(string addressableId) where T : Component;
        UniTask<T> Spawn<T>(string addressableId, Vector3 position, Transform parent = null, bool worldPositionStays = true) where T : Component;
        UniTask<T> Spawn<T>(string addressableId, Vector3 position, Quaternion rotation, Transform parent = null, bool worldPositionStays = true) where T : Component;
        UniTask<T> Spawn<T>(string addressableId, Vector3 position, Vector3 angle, Transform parent = null, bool worldPositionStays = true) where T : Component;

        void Recycle<T>(T recycleObj) where T : Component;
        void RecycleAll<T>(T prefab) where T : Component;

        void CleanUpPool<T>(T prefab) where T : Component;
    }

    public class ObjectPoolService : IObjectPoolService
    {
        #region Inject

        private readonly IAssetServices assetServices;

        #endregion

        private readonly Dictionary<string, GameObject>     idToPrefab         = new();
        private readonly Dictionary<GameObject, ObjectPool> prefabToObjectPool = new();

        public static ObjectPoolService Instance;

        public ObjectPoolService(IAssetServices assetServices)
        {
            this.assetServices = assetServices;
            Instance           = this;
        }

        private async UniTask<T> GetPrefab<T>(string addressableId) where T : Component
        {
            if (this.idToPrefab.TryGetValue(addressableId, out var prefab))
            {
                if (prefab == null) this.idToPrefab.Remove(addressableId);
                else return prefab.GetComponent<T>();
            }

            var loadedPrefab = await this.assetServices.LoadAsset<GameObject>(addressableId);
            this.idToPrefab.Add(addressableId, loadedPrefab);
            return loadedPrefab.GetComponent<T>();
        }

        private ObjectPool GetPool(GameObject prefab)
        {
            if (this.prefabToObjectPool.TryGetValue(prefab, out var objectPool))
            {
                if (objectPool == null) this.prefabToObjectPool.Remove(prefab);
                else return objectPool;
            }

            var pool = new GameObject().AddComponent<ObjectPool>();
            pool.CreatePool(prefab, 0);
            this.prefabToObjectPool.Add(prefab, pool);
            return pool;
        }

        public void CreatePool<T>(T prefab, int initSize) where T : Component
        {
            var pool = this.GetPool(prefab.gameObject);
            pool.CreatePool(prefab.gameObject, initSize);
        }

        public async UniTask CreatePool<T>(string addressableId, int initSize) where T : Component
        {
            var prefab = await this.GetPrefab<T>(addressableId);
            var pool   = this.GetPool(prefab.gameObject);
            pool.CreatePool(prefab.gameObject, initSize);
        }

        #region Spawn

        public T Spawn<T>(T prefab) where T : Component
        {
            var pool     = this.GetPool(prefab.gameObject);
            var spawnObj = pool.Spawn().GetComponent<T>();
            spawnObj.transform.SetParent(pool.transform);
            return spawnObj;
        }

        public T Spawn<T>(T prefab, Vector3 position, Transform parent = null, bool worldPositionStays = true) where T : Component
        {
            var spawnObj = this.Spawn(prefab);
            if (parent != null)
            {
                spawnObj.transform.SetParent(parent, worldPositionStays);
            }

            spawnObj.transform.position = position;
            return spawnObj;
        }

        public T Spawn<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent = null, bool worldPositionStays = true) where T : Component
        {
            var spawnObj = this.Spawn(prefab, position, parent, worldPositionStays);
            spawnObj.transform.rotation = rotation;
            return spawnObj;
        }

        public T Spawn<T>(T prefab, Vector3 position, Vector3 angle, Transform parent = null, bool worldPositionStays = true) where T : Component
        {
            var spawnObj = this.Spawn(prefab, position, parent, worldPositionStays);
            spawnObj.transform.eulerAngles = angle;
            return spawnObj;
        }

        public async UniTask<T> Spawn<T>(string addressableId) where T : Component
        {
            var prefab = await this.GetPrefab<T>(addressableId);
            return this.Spawn(prefab);
        }

        public async UniTask<T> Spawn<T>(string addressableId, Vector3 position, Transform parent = null, bool worldPositionStays = true) where T : Component
        {
            var prefab = await this.GetPrefab<T>(addressableId);
            return this.Spawn(prefab, position, parent, worldPositionStays);
        }

        public async UniTask<T> Spawn<T>(string addressableId, Vector3 position, Quaternion rotation, Transform parent = null, bool worldPositionStays = true) where T : Component
        {
            var prefab = await this.GetPrefab<T>(addressableId);
            return this.Spawn(prefab, position, rotation, parent, worldPositionStays);
        }

        public async UniTask<T> Spawn<T>(string addressableId, Vector3 position, Vector3 angle, Transform parent = null, bool worldPositionStays = true) where T : Component
        {
            var prefab = await this.GetPrefab<T>(addressableId);
            return this.Spawn(prefab, position, angle, parent, worldPositionStays);
        }

        #endregion

        public void Recycle<T>(T recycleObj) where T : Component
        {
            var pool = this.prefabToObjectPool.Values.FirstOrDefault(p => p.IsMember(recycleObj.gameObject));
            if (pool == null)
            {
                throw new GdkException($"Don't has ObjectPool contain: {recycleObj.gameObject.name}, you need create ObjectPool before recycle!");
            }

            pool.Recycle(recycleObj.gameObject);
        }

        public void RecycleAll<T>(T prefab) where T : Component
        {
            var pool = this.GetPool(prefab.gameObject);
            pool.RecycleAll();
        }

        public void CleanUpPool<T>(T prefab) where T : Component
        {
            var pool = this.GetPool(prefab.gameObject);
            pool.CleanUpPool();
        }
    }
}