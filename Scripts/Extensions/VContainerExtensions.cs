namespace GameKit.Extensions
{
    using System;
    using Cysharp.Threading.Tasks;
    using GameKit.Services.UI.Interface;
    using GameKit.Services.UI.Service;
    using MessagePipe;
    using UnityEngine;
    using VContainer;
    using VContainer.Unity;
    using Object = UnityEngine.Object;

    public static class VContainerExtensions
    {
        private static MessagePipeOptions options;
        private static IObjectResolver    objectResolver;
        private static GameObject         projectLifetime;

        private static async UniTask WaitToInitialize()
        {
            await UniTask.WaitUntil(() => VContainerSettings.Instance.RootLifetimeScope.Container != null);
            objectResolver = VContainerSettings.Instance.RootLifetimeScope.Container;
        }

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

        public static async void NonLazy<T>(this RegistrationBuilder builder)
        {
            await WaitToInitialize();
            var dummy = new DummyInject<T>();
            objectResolver.Inject(dummy);
            dummy.Dispose();
        }

        public static async void InitUI<TPresenter>() where TPresenter : IUIPresenter
        {
            await WaitToInitialize();
            var uiServices = objectResolver.Resolve<IUIService>();
            uiServices.OpenView<TPresenter>();
        }

        public static async void InitUI<TPresenter, TModel>(TModel model) where TPresenter : IUIPresenter<TModel> where TModel : IModel
        {
            await WaitToInitialize();
            var uiServices = objectResolver.Resolve<IUIService>();
            uiServices.OpenView<TPresenter, TModel>(model);
        }
    }

    public class DummyInject<T> : IDisposable
    {
        [Inject]
        private void Init(T injected) { }

        public void Dispose() { }
    }
}