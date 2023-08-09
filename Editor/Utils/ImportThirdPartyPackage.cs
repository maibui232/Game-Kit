namespace GameKit.Editor.Editor.Utils
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEditor.PackageManager;
    using UnityEngine;

    public class ImportThirdPartyPackage
    {
        private static readonly List<string> ImportedPackages = new();

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

        [MenuItem("Utils/Import/VContainer")]
        public static void ImportVContainer()
        {
            const string vContainerURL = "https://github.com/hadashiA/VContainer.git?path=VContainer/Assets/VContainer#1.13.2";
            ImportPackage(vContainerURL);
        }

        [MenuItem("Utils/Import/MessagePipe")]
        public static void ImportMessagePipe()
        {
            const string messagePipeVContainerURL = "https://github.com/Cysharp/MessagePipe.git?path=src/MessagePipe.Unity/Assets/Plugins/MessagePipe.VContainer";
            const string messagePipeURL           = "https://github.com/Cysharp/MessagePipe.git?path=src/MessagePipe.Unity/Assets/Plugins/MessagePipe";
            ImportPackage(messagePipeURL);
            ImportPackage(messagePipeVContainerURL);
        }

        private static void ImportPackage(string url)
        {
            var request = Client.Add(url);
            EditorApplication.update += OnProgress;

            void OnProgress()
            {
                if (!request.IsCompleted)
                {
                    if (request.Status == StatusCode.InProgress)
                    {
                        Debug.Log($"Importing...");
                    }

                    return;
                }

                Debug.Log(request.Status == StatusCode.Success ? $"Import {request.Result.displayName} completed" : $"Fail to import, check URL: {url}, please!");
                EditorApplication.update -= OnProgress;
            }
        }
    }
}