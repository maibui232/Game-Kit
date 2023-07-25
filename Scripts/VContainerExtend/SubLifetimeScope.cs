namespace GDK.Scripts.VContainerExtend
{
    using VContainer;

    public abstract class SubLifetimeScope<TDerived> where TDerived : SubLifetimeScope<TDerived>, new()
    {
        public static void Config(IContainerBuilder builder)
        {
            var scope = new TDerived();
            // var scope = FindObjectOfType<TDerived>();
            scope.Configure(builder);
        }

        protected abstract void Configure(IContainerBuilder builder);
    }
}