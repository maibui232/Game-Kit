namespace GDK.Editor.Utils
{
    using UnityEditor;
    using UnityEditor.PackageManager;
    using UnityEngine;

    public class ImportThirdPartyPackage
    {
        [MenuItem("Utils/Import/DOTS")]
        public static void ImportDOTS()
        {
            const string renderURL   = "com.unity.entities.graphics";
            const string entitiesURL = "com.unity.entities";
            const string physicURL   = "com.unity.physics";

            ImportPackage(renderURL);
            ImportPackage(entitiesURL);
            ImportPackage(physicURL);
        }

        [MenuItem("Utils/Import/Unitask")]
        public static void ImportUniTask()
        {
            const string uniTaskURL = "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask";
            ImportPackage(uniTaskURL);
        }

        [MenuItem("Utils/Import/UniRx")]
        public static void ImportUniRx()
        {
            const string uniRxURL = "https://github.com/neuecc/UniRx.git?path=Assets/Plugins/UniRx/Scripts";
            ImportPackage(uniRxURL);
        }

        private static void ImportPackage(string url)
        {
            var request = Client.Add(url);
            EditorApplication.update += OnProgress;

            void OnProgress()
            {
                if (!request.IsCompleted) return;
                Debug.Log(request.Status == StatusCode.Success ? "UniTask import completed" : $"Fail to import, check URL: {url}, please!");
                EditorApplication.update -= OnProgress;
            }
        }
    }
}