namespace GDK.Scripts.Services.UI
{
    using GDK.Scripts.Services.Addressable;
    using GDK.Scripts.Services.UI.Interface;
    using GDK.Scripts.Services.UI.Service;
    using GDK.Scripts.VContainerExtend;
    using VContainer;
    using VContainer.Unity;

    public class UIServiceLifetimeScope : SubLifetimeScope<UIServiceLifetimeScope, RootUI>
    {
        protected override void Configure(IContainerBuilder builder, IObjectResolver container, RootUI param1)
        {
            builder.Register<AddressableServices>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterComponentInNewPrefab(param1, Lifetime.Singleton).DontDestroyOnLoad();
            builder.Register<UIService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterAllDerivedTypeFrom<IUIPresenter>(Lifetime.Scoped);
        }
    }
}