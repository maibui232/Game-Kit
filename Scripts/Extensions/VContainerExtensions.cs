namespace GameKit.Extensions
{
    using GameKit.Services.UI.Interface;
    using GameKit.Services.UI.Service;
    using MessagePipe;
    using UnityEngine;
    using VContainer;
    using VContainer.Unity;

    public static class Extensions
    {
        private static MessagePipeOptions options;

        private static GameObject projectLifetime;

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

        public static IContainerBuilder RegisterMessage<TMessage>(this IContainerBuilder builder)
        {
            options ??= builder.RegisterMessagePipe();
            return builder.RegisterMessageBroker<TMessage>(options);
        }

        public static IContainerBuilder RegisterMessage<TKey, TMessage>(this IContainerBuilder builder)
        {
            options ??= builder.RegisterMessagePipe();
            return builder.RegisterMessageBroker<TKey, TMessage>(options);
        }

        public static ComponentRegistrationBuilder AsProject(this ComponentRegistrationBuilder builder)
        {
            projectLifetime ??= new GameObject("ProjectLifetime");
            if (projectLifetime != null) Object.DontDestroyOnLoad(projectLifetime);
            return builder.DontDestroyOnLoad().UnderTransform(projectLifetime.transform);
        }

        public static void InitView<TPresenter>(this IObjectResolver objectResolver) where TPresenter : IUIPresenter
        {
            var uiServices = objectResolver.Resolve<IUIService>();
            uiServices.OpenView<TPresenter>();
        }

        public static void InitView<TPresenter, TModel>(this IObjectResolver objectResolver, TModel model) where TPresenter : IUIPresenter<TModel> where TModel : IModel
        {
            var uiServices = objectResolver.Resolve<IUIService>();
            uiServices.OpenView<TPresenter, TModel>(model);
        }
    }
}