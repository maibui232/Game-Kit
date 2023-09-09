namespace GameKit.VContainerBridge
{
    using VContainer;
    using VContainer.Unity;

    public abstract class ProjectScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            this.parentReference.Object = null;
        }
    }
}