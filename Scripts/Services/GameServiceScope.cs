namespace GameKit.Services
{
    using GameKit.Services.LocalData;
    using GameKit.Services.Logger;
    using GameKit.Services.Message;
    using GameKit.Services.ObjectPool;
    using GameKit.Services.UI;
    using GameKit.Services.Utils.SceneServices;
    using GameKit.VContainerBridge;
    using VContainer;

    public class GameServiceScope : SubScope<GameServiceScope, RootUI>
    {
        protected override void Configure(IContainerBuilder builder, IObjectResolver container, RootUI rootUI)
        {
            builder.Register<MessageService>(Lifetime.Singleton).AsImplementedInterfaces(); // Message
            LocalDataServiceScope.Config(builder, container);

            builder.Register<LoggerService>(Lifetime.Singleton).AsImplementedInterfaces(); // Logger
            builder.Register<SceneService>(Lifetime.Singleton).AsImplementedInterfaces(); // Scene Service
            builder.Register<ObjectPoolService>(Lifetime.Singleton).AsImplementedInterfaces(); // Object Pool
            UIServiceScope.Config(builder, container, rootUI); // UI Service
        }
    }
}