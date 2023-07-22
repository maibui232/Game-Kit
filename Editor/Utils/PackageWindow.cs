// namespace GDK.Editor.Utils
// {
//     using System.Collections.Generic;
//     using Sirenix.OdinInspector.Editor;
//     using UnityEditor;
//     using UnityEditor.PackageManager;
//     using UnityEngine;
//
//     public class PackageWindow : OdinEditorWindow
//     {
//         [MenuItem("Utils/PackageSetup")]
//         public static void InitWindow()
//         {
//             var window = GetWindow<PackageWindow>();
//             window.Show();
//         }
//
//         private static readonly List<string> PackageExist = new();
//
//
//         #region PackInfo
//
//         // DOTS
//         private const string DOTS_Package = "DOTS";
//
//         private PackageInfo entitiesPack        = new("Entities", "com.unity.entities", PackageExist);
//         private PackageInfo entitiesGraphicPack = new("Entities Graphic", "com.unity.entities.graphics", PackageExist);
//         private PackageInfo entitiesPhysicPack  = new("Entities Physic", "com.unity.physics", PackageExist);
//
//         private const string UtilsPackage = "Utils";
//
//         private PackageInfo uniTaskPack = new("UniTask", "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask", PackageExist);
//         private PackageInfo uniRxPack   = new("UniRx", "https://github.com/neuecc/UniRx.git?path=Assets/Plugins/UniRx/Scripts", PackageExist);
//
//         #endregion
//
//         protected override void OnEnable()
//         {
//             base.OnEnable();
//
//             var listRequest = Client.List();
//             foreach (var info in listRequest.Result)
//             {
//                 PackageExist.Add(info.name);
//             }
//         }
//
//         protected override void OnGUI()
//         {
//             base.OnGUI();
//             GUILayout.Label(DOTS_Package);
//             this.entitiesPack.OnDrawEditor();
//             this.entitiesGraphicPack.OnDrawEditor();
//             this.entitiesPhysicPack.OnDrawEditor();
//
//             GUILayout.Space(10);
//
//             GUILayout.Label(UtilsPackage);
//             this.uniTaskPack.OnDrawEditor();
//             this.uniRxPack.OnDrawEditor();
//         }
//     }
//
//     public class PackageInfo
//     {
//         private string name;
//         private string url;
//         private bool   isImport;
//
//         public PackageInfo(string name, string url, List<string> packages)
//         {
//             this.url      = url;
//             this.isImport = this.IsImported(packages);
//         }
//
//         public void OnDrawEditor()
//         {
//             EditorGUILayout.BeginHorizontal();
//             this.isImport = EditorGUILayout.Toggle(this.name, this.isImport);
//             EditorGUILayout.EndHorizontal();
//         }
//
//         public void Import()
//         {
//             if (this.isImport) return;
//             if (string.IsNullOrEmpty(this.url))
//             {
//                 Debug.LogError("Package name invalid!!");
//                 return;
//             }
//
//             var request = Client.Add(this.url);
//             EditorApplication.update += OnProgress;
//
//             void OnProgress()
//             {
//                 if (!request.IsCompleted) return;
//                 if (request.Status == StatusCode.Success)
//                 {
//                     this.isImport = true;
//                 }
//
//                 Debug.Log(request.Status == StatusCode.Success ? "UniTask import completed" : $"Fail to import, check URL: {url}, please!");
//                 EditorApplication.update -= OnProgress;
//             }
//         }
//
//         private bool IsImported(List<string> packages) { return packages.Contains(this.url); }
//     }
// }