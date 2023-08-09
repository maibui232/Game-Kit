namespace GameKit.VContainerBridge
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using VContainer;
    using VContainer.Unity;

    public class SceneLifetimeScope<TDerived> : LifetimeScope where TDerived : SceneLifetimeScope<TDerived>
    {
#if UNITY_EDITOR
        [Button]
        private void GetAllInjectGameObject()
        {
            var allObj = FindObjectsOfType<GameObject>();
            this.autoInjectGameObjects.Clear();
            foreach (var obj in allObj)
            {
                var components = obj.GetComponents<MonoBehaviour>();
                var listType   = components.Select(comp => comp.GetType()).ToList();

                if (!listType.Any(this.CheckComponentCanInject)) continue;
                this.autoInjectGameObjects.Add(obj);
            }
        }

        private bool CheckComponentCanInject(Type comp)
        {
            var methods   = comp.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            var fields    = comp.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            var hasMethod = methods.Any(method => Attribute.IsDefined(method, typeof(InjectAttribute)));
            var hasField  = fields.Any(field => Attribute.IsDefined(field, typeof(InjectAttribute)));
            return hasMethod || hasField;
        }
#endif

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            this.parentReference.TypeName = nameof(VContainerSettings.Instance.RootLifetimeScope);
            this.parentReference.Object   = VContainerSettings.Instance.RootLifetimeScope;
        }
    }
}