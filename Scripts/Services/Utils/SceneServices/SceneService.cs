namespace GameKit.Services.Utils.SceneServices
{
    using Cysharp.Threading.Tasks;
    using GameKit.Services.Addressable;
    using UnityEngine.SceneManagement;

    public interface ISceneService
    {
        string  CurrentSceneName { get; }
        UniTask LoadSingleScene(string sceneName, bool activeOnLoad = true);
        UniTask LoadAdditiveScene(string sceneName, bool activeOnLoad = true);
    }

    public class SceneService : ISceneService
    {
        #region Inject

        private readonly IAssetServices assetServices;

        #endregion

        public static SceneService Instance;

        public SceneService(IAssetServices assetServices) { this.assetServices = assetServices; }

        public string CurrentSceneName => SceneManager.GetActiveScene().name;

        public async UniTask LoadSingleScene(string sceneName, bool activeOnLoad = true) { await this.assetServices.LoadSceneAsync(sceneName, LoadSceneMode.Single, activeOnLoad); }

        public async UniTask LoadAdditiveScene(string sceneName, bool activeOnLoad = true) { await this.assetServices.LoadSceneAsync(sceneName, LoadSceneMode.Additive, activeOnLoad); }
    }
}