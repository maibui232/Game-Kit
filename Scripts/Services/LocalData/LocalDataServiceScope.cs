namespace GameKit.Services.LocalData
{
    using GameKit.Extensions;
    using GameKit.Services.LocalData.ApplicationManager;
    using GameKit.VContainerBridge;
    using VContainer;
    using VContainer.Unity;

    public class LocalDataServiceScope : SubScope<LocalDataServiceScope>
    {
        protected override void Configure(IContainerBuilder builder, IObjectResolver container)
        {
            builder.RegisterEntryPoint<LocalDataHandler>().As<ILocalDataHandler>();

            builder.RegisterMessage<ApplicationStateChangeMessage>();
            builder.RegisterMessage<LoadLocalDataCompletedMessage>();

            builder.RegisterComponentOnNewGameObject<ApplicationManger>(Lifetime.Singleton, nameof(ApplicationManger)).AsProject().NonLazy<ApplicationManger>();
        }
    }
}