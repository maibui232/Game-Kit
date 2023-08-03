namespace GDK.Scripts.Utils.SceneServices
{
    using Cysharp.Threading.Tasks;
    using GDK.Scripts.Services.Addressable;
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

        private readonly IAddressableServices addressableServices;

        #endregion

        public static SceneService Instance;

        public SceneService(IAddressableServices addressableServices) { this.addressableServices = addressableServices; }

        public string CurrentSceneName => SceneManager.GetActiveScene().name;

        public async UniTask LoadSingleScene(string sceneName, bool activeOnLoad = true) { await this.addressableServices.LoadSceneAsync(sceneName, LoadSceneMode.Single, activeOnLoad); }

        public async UniTask LoadAdditiveScene(string sceneName, bool activeOnLoad = true) { await this.addressableServices.LoadSceneAsync(sceneName, LoadSceneMode.Additive, activeOnLoad); }
    }
}