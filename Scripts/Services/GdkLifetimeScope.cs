namespace GDK.Scripts.Services
{
    using GDK.Scripts.Services.Logger;
    using GDK.Scripts.Services.UI;
    using GDK.Scripts.Utils.SceneServices;
    using GDK.Scripts.VContainerExtend;
    using VContainer;

    public class GdkLifetimeScope : SubLifetimeScope<GdkLifetimeScope, RootUI>
    {
        protected override void Configure(IContainerBuilder builder, IObjectResolver container, RootUI param1)
        {
            builder.Register<LoggerService>(Lifetime.Singleton).AsImplementedInterfaces(); // Logger
            builder.Register<SceneService>(Lifetime.Singleton).AsImplementedInterfaces(); // Scene Service
            UIServiceLifetimeScope.Config(builder, container, param1); // UI Service
        }
    }
}