namespace GameKit.Services.Utils.SceneServices
{
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameKit.Services.Addressable;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;

    public interface ISceneService
    {
        List<SceneInstance> ActiveScenes { get; }
        UniTask             LoadSingleScene(string sceneName, bool activeOnLoad = true);
        UniTask             LoadAdditiveScene(string sceneName, bool activeOnLoad = true);
    }

    public class SceneService : ISceneService
    {
        #region Inject

        private readonly IAssetServices assetServices;

        #endregion

        public static SceneService Instance;

        public SceneService(IAssetServices assetServices) { this.assetServices = assetServices; }

        public List<SceneInstance> ActiveScenes { get; } = new();

        public async UniTask LoadSingleScene(string sceneName, bool activeOnLoad = true)
        {
            var listTask = Enumerable.Select(this.ActiveScenes, sceneInstance => this.assetServices.UnloadSceneAsync(sceneInstance, UnloadSceneOptions.None)).ToList();
            await UniTask.WhenAll(listTask);
            this.ActiveScenes.Clear();

            var scene = await this.assetServices.LoadSceneAsync(sceneName, LoadSceneMode.Additive, activeOnLoad);
            this.ActiveScenes.Add(scene);
        }

        public async UniTask LoadAdditiveScene(string sceneName, bool activeOnLoad = true)
        {
            var scene = await this.assetServices.LoadSceneAsync(sceneName, LoadSceneMode.Additive, activeOnLoad);
            this.ActiveScenes.Add(scene);
        }
    }
}