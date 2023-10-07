namespace GameKit.Editor.Utils
{
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public static class CreateExtensions
    {
        public static void CreateWithPath(string path)
        {
            var obj = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(path));
            Place(obj as GameObject);
        }

        public static void Place(GameObject gameObject)
        {
            var lastView = SceneView.lastActiveSceneView;
            gameObject.transform.position = lastView ? lastView.pivot : Vector3.zero;
            StageUtility.PlaceGameObjectInCurrentStage(gameObject);
            GameObjectUtility.EnsureUniqueNameForSibling(gameObject);

            Undo.RegisterCreatedObjectUndo(gameObject, $"Create Object: {gameObject.name}");
            Selection.activeGameObject = gameObject;

            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
    }
}