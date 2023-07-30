namespace GDK.Scripts.Services.ObjectPool
{
    using System.Collections.Generic;
    using UnityEngine;

    public interface IObjectPool
    {
        int        CurrentSizeOfPool { get; }
        bool       IsMember(GameObject spawnedObj);
        void       CreatePool(GameObject prefab, int initSize);
        GameObject Spawn();
        GameObject Recycle(GameObject recycleObj);
        void       RecycleAll();
        void       CleanUpPool();
    }

    public class ObjectPool : MonoBehaviour, IObjectPool
    {
        private GameObject currentPrefab;

        [SerializeField] private List<GameObject> listSpawned  = new();
        [SerializeField] private List<GameObject> listRecycled = new();

        public int CurrentSizeOfPool => this.listRecycled.Count;

        public bool IsMember(GameObject spawnedObj) { return this.listSpawned.Contains(spawnedObj); }

        public void CreatePool(GameObject prefab, int initSize)
        {
            if (this.currentPrefab == null) this.currentPrefab = prefab;
            for (var i = 0; i < initSize; i++)
            {
                this.InitObject();
            }
        }

        private GameObject InitObject()
        {
            var obj = Instantiate(this.currentPrefab);
            obj.gameObject.SetActive(false);
            this.listRecycled.Add(obj);
            return obj;
        }

        public GameObject Spawn()
        {
            var spawnObj = this.listRecycled.Count == 0 ? this.InitObject() : this.listRecycled[0];
            this.listRecycled.Remove(spawnObj);
            return spawnObj;
        }

        public GameObject Recycle(GameObject recycleObj)
        {
            if (this.listSpawned.Contains(recycleObj))
            {
                this.listSpawned.Remove(recycleObj);
            }

            recycleObj.gameObject.SetActive(false);
            this.listRecycled.Add(recycleObj);
            return recycleObj;
        }

        public void RecycleAll()
        {
            foreach (var spawnedObj in this.listSpawned)
            {
                this.Recycle(spawnedObj);
            }
        }

        public void CleanUpPool()
        {
            foreach (var spawnedObj in this.listSpawned)
            {
                Destroy(spawnedObj);
            }

            this.listSpawned.Clear();

            foreach (var recycleObj in this.listRecycled)
            {
                Destroy(recycleObj);
            }

            this.listRecycled.Clear();
        }

        private void OnDestroy() { this.CleanUpPool(); }
    }
}