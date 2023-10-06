namespace GameKit.Services.Utils.SceneServices
{
    using GameKit.Services.GameAsset;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;

    public interface ISceneService
    {
        AsyncOperationHandle<SceneInstance> LoadSingleScene(string sceneName, bool activeOnLoad = true);
        AsyncOperationHandle<SceneInstance> LoadAdditiveScene(string sceneName, bool activeOnLoad = true);
        AsyncOperationHandle                UnloadScene(string sceneName);
    }

    public class SceneService : ISceneService
    {
        #region Inject

        private readonly IGameAssets gameAsset;

        #endregion

        public static SceneService Instance;

        public SceneService(IGameAssets gameAsset) { this.gameAsset = gameAsset; }

        public AsyncOperationHandle<SceneInstance> LoadSingleScene(string sceneName, bool activeOnLoad = true)
        {
            return this.gameAsset.LoadSceneAsync(sceneName, LoadSceneMode.Single, activeOnLoad);
        }

        public AsyncOperationHandle<SceneInstance> LoadAdditiveScene(string sceneName, bool activeOnLoad = true)
        {
            return this.gameAsset.LoadSceneAsync(sceneName, LoadSceneMode.Additive, activeOnLoad);
        }

        public AsyncOperationHandle UnloadScene(string sceneName)
        {
            return this.gameAsset.UnloadSceneAsync(sceneName);
        }
    }
}