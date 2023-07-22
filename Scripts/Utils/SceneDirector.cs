namespace GDK.Scripts.Utils
{
    public class SceneDirector
    {
        public static SceneDirector Instance;

        public string CurrentSceneName { get; private set; }
    }
}