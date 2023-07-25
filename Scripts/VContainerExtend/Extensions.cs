namespace GDK.Scripts.VContainerExtend
{
    using UnityEngine;
    using VContainer;
    using VContainer.Unity;

    public static class Extensions
    {
        public static ComponentRegistrationBuilder RegisterFromResource<T>(this IContainerBuilder builder, string resourcePath, Lifetime lifetime) where T : MonoBehaviour
        {
            var obj = Resources.Load<GameObject>(resourcePath).GetComponent<T>();
            return builder.RegisterComponentInNewPrefab(obj, lifetime);
        }
    }
}