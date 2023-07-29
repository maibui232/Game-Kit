namespace GDK.Scripts.VContainerBridge
{
    using VContainer;
    using VContainer.Unity;

    public abstract class ProjectLifeTimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            this.parentReference.Object = null;
        }
    }
}