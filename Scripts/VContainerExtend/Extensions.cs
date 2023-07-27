namespace GDK.Scripts.VContainerExtend
{
    using System;
    using System.Linq;
    using System.Reflection;
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

        public static void RegisterAllType<T>(this IContainerBuilder builder, Lifetime lifetime)
        {
            var derivedTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(domainAssembly => domainAssembly.GetTypes())
                .Where(type => !type.IsAbstract && typeof(T).IsAssignableFrom(type));
            foreach (var type in derivedTypes)
            {
                builder.Register(type, lifetime);
            }
        }
    }
}