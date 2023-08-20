namespace GameKit.Services.ApplicationManager
{
    using GameKit.Extensions;
    using GameKit.VContainerBridge;
    using VContainer;
    using VContainer.Unity;

    public class ApplicationManagerLifetimeScope : SubLifetimeScope<ApplicationManagerLifetimeScope>
    {
        protected override void Configure(IContainerBuilder builder, IObjectResolver container)
        {
            builder.RegisterMessage<ApplicationStateChangeMessage>();
            builder.RegisterComponentOnNewGameObject<ApplicationManger>(Lifetime.Singleton, nameof(ApplicationManger)).AsProject().NonLazy<ApplicationManger>();
        }
    }
}