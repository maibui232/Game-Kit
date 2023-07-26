namespace GDK.Scripts.Test
{
    using GDK.Scripts.Services.UI;
    using GDK.Scripts.VContainerExtend;
    using UnityEngine;
    using VContainer;

    public sealed class TestProjectLifetimeScope : ProjectLifeTimeScope
    {
        [SerializeField] private RootUI rootUI;

        protected override void Configure(IContainerBuilder builder)
        {
            this.parentReference.Object = null;
            base.Configure(builder);
            UIServiceLifetimeScope.Config(builder, this.Container, this.rootUI);
        }
    }
}