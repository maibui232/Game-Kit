namespace GameKit.Services
{
    using GameKit.Services.Logger;
    using GameKit.Services.Message;
    using GameKit.Services.ObjectPool;
    using GameKit.Services.UI;
    using GameKit.Services.Utils.SceneServices;
    using GameKit.VContainerBridge;
    using VContainer;

    public class GdkLifetimeScope : SubLifetimeScope<GdkLifetimeScope, RootUI>
    {
        protected override void Configure(IContainerBuilder builder, IObjectResolver container, RootUI param1)
        {
            builder.Register<MessageService>(Lifetime.Singleton).AsImplementedInterfaces(); // Message
            builder.Register<LoggerService>(Lifetime.Singleton).AsImplementedInterfaces(); // Logger
            builder.Register<SceneService>(Lifetime.Singleton).AsImplementedInterfaces(); // Scene Service
            builder.Register<ObjectPoolService>(Lifetime.Singleton).AsImplementedInterfaces(); // Object Pool
            UIServiceLifetimeScope.Config(builder, container, param1); // UI Service
        }
    }
}