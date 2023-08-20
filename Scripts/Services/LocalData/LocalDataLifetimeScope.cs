namespace GameKit.Services.LocalData
{
    using GameKit.Extensions;
    using GameKit.Services.LocalData.ApplicationManager;
    using GameKit.VContainerBridge;
    using VContainer;
    using VContainer.Unity;

    public class LocalDataLifetimeScope : SubLifetimeScope<LocalDataLifetimeScope>
    {
        protected override void Configure(IContainerBuilder builder, IObjectResolver container)
        {
            builder.RegisterAllDerivedTypeFrom<ILocalData>(Lifetime.Singleton);

            builder.RegisterMessage<ApplicationStateChangeMessage>();
            builder.RegisterMessage<LoadLocalDataCompletedMessage>();

            builder.RegisterEntryPoint<LocalDataHandler>().As<ILocalDataHandler>();
            builder.RegisterComponentOnNewGameObject<ApplicationManger>(Lifetime.Singleton, nameof(ApplicationManger)).AsProject().NonLazy<ApplicationManger>();
        }
    }
}