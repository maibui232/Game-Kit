namespace GameKit.VContainerBridge
{
    using GameKit.Services.Utils.SceneServices;
    using UnityEngine;
    using VContainer;

    public class InitializeProject : MonoBehaviour
    {
        private const string LoadingSceneName = "1.Loading";

        [Inject]
        private void Init(ISceneService sceneService)
        {
            sceneService.LoadSingleScene(LoadingSceneName);
            Destroy(this.gameObject);
        }
    }
}