namespace GDK.Scripts.UI
{
    using UnityEngine;
    using VContainer;
    using VContainer.Unity;

    public class UIServiceInstaller : LifetimeScope
    {
        [SerializeField] private RootUI rootUI;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.RegisterComponentInNewPrefab(this.rootUI, Lifetime.Scoped);
        }
    }
}