namespace GDK.Scripts.Test
{
    using GDK.Scripts.Extensions;
    using GDK.Scripts.Services;
    using GDK.Scripts.Services.UI;
    using GDK.Scripts.Test.Message;
    using GDK.Scripts.VContainerBridge;
    using UnityEngine;
    using VContainer;

    public sealed class TestProjectLifetimeScope : ProjectLifeTimeScope
    {
        [SerializeField] private RootUI rootUI;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            GdkLifetimeScope.Config(builder, this.Container, this.rootUI);

            builder.RegisterMessage<TestMessage>();
        }
    }
}