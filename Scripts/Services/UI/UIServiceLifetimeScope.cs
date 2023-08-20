namespace GameKit.Services.UI
{
    using GameKit.Extensions;
    using GameKit.Services.Addressable;
    using GameKit.Services.UI.Interface;
    using GameKit.Services.UI.Service;
    using GameKit.VContainerBridge;
    using VContainer;
    using VContainer.Unity;

    public class UIServiceLifetimeScope : SubLifetimeScope<UIServiceLifetimeScope, RootUI>
    {
        protected override void Configure(IContainerBuilder builder, IObjectResolver container, RootUI rootUI)
        {
            builder.Register<AssetServices>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterComponentInNewPrefab(rootUI, Lifetime.Singleton).AsProject();
            builder.Register<UIService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterAllDerivedTypeFrom<IUIPresenter>(Lifetime.Scoped);
        }
    }
}