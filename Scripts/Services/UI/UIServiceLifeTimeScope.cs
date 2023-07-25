namespace GDK.Scripts.Services.UI
{
    using GDK.Scripts.Services.Addressable;
    using GDK.Scripts.Services.UI.Service;
    using GDK.Scripts.VContainerExtend;
    using VContainer;

    public class UIServiceLifeTimeScope : SubLifetimeScope<UIServiceLifeTimeScope>
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<AddressableServices>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterFromResource<RootUI>("RootUI", Lifetime.Singleton).DontDestroyOnLoad();
            builder.Register<UIService>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}