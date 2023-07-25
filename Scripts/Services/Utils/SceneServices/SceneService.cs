namespace GDK.Scripts.Utils.SceneServices
{
    using Cysharp.Threading.Tasks;
    using UnityEngine.SceneManagement;

    public interface ISceneService
    {
        string  CurrentSceneName { get; }
        UniTask LoadSingleScene(string sceneName);
    }

    public class SceneService : ISceneService
    {
        public static SceneService Instance;

        public string CurrentSceneName { get; private set; }

        public UniTask LoadSingleScene(string sceneName)
        {
            var sceneTask = SceneManager.LoadSceneAsync(sceneName);
            return sceneTask.ToUniTask();
        }
    }
}