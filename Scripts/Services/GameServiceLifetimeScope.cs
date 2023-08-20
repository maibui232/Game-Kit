namespace GameKit.Services
{
    using Cysharp.Threading.Tasks;
    using GameKit.Services.LocalData;
    using GameKit.Services.LocalData.ApplicationManager;
    using GameKit.Services.Logger;
    using GameKit.Services.Message;
    using GameKit.Services.ObjectPool;
    using GameKit.Services.UI;
    using GameKit.Services.Utils.SceneServices;
    using GameKit.VContainerBridge;
    using VContainer;

    public class GameServiceLifetimeScope : SubLifetimeScope<GameServiceLifetimeScope, RootUI>
    {
        protected override void Configure(IContainerBuilder builder, IObjectResolver container, RootUI rootUI)
        {
            builder.Register<MessageService>(Lifetime.Singleton).AsImplementedInterfaces(); // Message
            LocalDataLifetimeScope.Config(builder, container);

            builder.Register<LoggerService>(Lifetime.Singleton).AsImplementedInterfaces(); // Logger
            builder.Register<SceneService>(Lifetime.Singleton).AsImplementedInterfaces(); // Scene Service
            builder.Register<ObjectPoolService>(Lifetime.Singleton).AsImplementedInterfaces(); // Object Pool
            UIServiceLifetimeScope.Config(builder, container, rootUI); // UI Service
        }
    }
}