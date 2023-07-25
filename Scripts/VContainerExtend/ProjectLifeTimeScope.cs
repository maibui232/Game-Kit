namespace GDK.Scripts.VContainerExtend
{
    using GDK.Scripts.Services.UI;
    using UnityEngine;
    using VContainer;
    using VContainer.Unity;

    public class ProjectLifeTimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            UIServiceLifeTimeScope.Config(builder);
            Debug.Log("Init");
        }
    }
}