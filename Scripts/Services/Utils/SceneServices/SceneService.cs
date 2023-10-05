namespace GameKit.Services.Utils.SceneServices
{
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using GameKit.Services.GameAsset;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;

    public interface ISceneService
    {
        Dictionary<string, SceneInstance> NameToScene { get; }
        UniTask<SceneInstance>            LoadSingleScene(string sceneName, bool activeOnLoad = true);
        UniTask<SceneInstance>            LoadAdditiveScene(string sceneName, bool activeOnLoad = true);
        UniTask                           UnloadScene(string sceneName);
    }

    public class SceneService : ISceneService
    {
        #region Inject

        private readonly IGameAssets assetServices;

        #endregion

        public static SceneService Instance;

        public SceneService(IGameAssets assetServices) { this.assetServices = assetServices; }


        public Dictionary<string, SceneInstance> NameToScene { get; set; } = new();

        public async UniTask<SceneInstance> LoadSingleScene(string sceneName, bool activeOnLoad = true)
        {
            this.NameToScene.Clear();
            var scene = await this.assetServices.LoadSceneAsync(sceneName, LoadSceneMode.Single, activeOnLoad);
            this.NameToScene.Add(sceneName, scene);
            return scene;
        }

        public async UniTask<SceneInstance> LoadAdditiveScene(string sceneName, bool activeOnLoad = true)
        {
            var scene = await this.assetServices.LoadSceneAsync(sceneName, LoadSceneMode.Additive, activeOnLoad);
            this.NameToScene.Add(sceneName, scene);
            return scene;
        }

        public async UniTask UnloadScene(string sceneName)
        {
            if (!this.NameToScene.TryGetValue(sceneName, out var scene))
            {
                await SceneManager.UnloadSceneAsync(sceneName);
                return;
            }

            await this.assetServices.UnloadSceneAsync(scene);
            this.NameToScene.Remove(sceneName);
        }
    }
}