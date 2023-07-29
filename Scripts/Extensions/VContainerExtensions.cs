namespace GDK.Scripts.Extensions
{
    using MessagePipe;
    using UnityEngine;
    using VContainer;
    using VContainer.Unity;

    public static class Extensions
    {
        private static MessagePipeOptions options;

        public static ComponentRegistrationBuilder RegisterFromResource<T>(this IContainerBuilder builder, string resourcePath, Lifetime lifetime) where T : MonoBehaviour
        {
            var obj = Resources.Load<GameObject>(resourcePath).GetComponent<T>();
            return builder.RegisterComponentInNewPrefab(obj, lifetime);
        }

        public static void RegisterAllDerivedTypeFrom<T>(this IContainerBuilder builder, Lifetime lifetime)
        {
            foreach (var type in ReflectionExtension.GetAllNonAbstractDerivedTypeFrom<T>())
            {
                builder.Register(type, lifetime);
            }
        }

        public static void RegisterMessage<T>(this IContainerBuilder builder)
        {
            options ??= builder.RegisterMessagePipe();
            builder.RegisterMessageBroker<T>(options);
        }
    }
}