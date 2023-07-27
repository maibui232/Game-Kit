namespace GDK.Scripts.Exception
{
    using System;
    using GDK.Scripts.Services.UI.Interface;

    public class GdkNullViewException<TView> : Exception where TView : IView
    {
        public override string Message => $"This View Instantiate does not contain: {typeof(TView)}";
    }
}