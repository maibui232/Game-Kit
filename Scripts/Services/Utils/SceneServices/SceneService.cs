namespace GameKit.Services.Utils.SceneServices
{
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using GameKit.Services.Addressable;
    using UnityEngine.SceneManagement;

    public interface ISceneService
    {
        List<string> ActiveScenes { get; }
        UniTask      LoadSingleScene(string sceneName, bool activeOnLoad = true);
        UniTask      LoadAdditiveScene(string sceneName, bool activeOnLoad = true);
    }

    public class SceneService : ISceneService
    {
        #region Inject

        private readonly IAssetServices assetServices;

        #endregion

        public static SceneService Instance;

        public SceneService(IAssetServices assetServices) { this.assetServices = assetServices; }

        public List<string> ActiveScenes { get; } = new();

        public async UniTask LoadSingleScene(string sceneName, bool activeOnLoad = true)
        {
            this.ActiveScenes.Clear();
            await this.assetServices.LoadSceneAsync(sceneName, LoadSceneMode.Single, activeOnLoad);
            this.ActiveScenes.Add(sceneName);
        }

        public async UniTask LoadAdditiveScene(string sceneName, bool activeOnLoad = true)
        {
            await this.assetServices.LoadSceneAsync(sceneName, LoadSceneMode.Additive, activeOnLoad);
            this.ActiveScenes.Add(sceneName);
        }
    }
}