namespace GameKit.Services.UI.Interface
{
    using System;
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    public enum ViewStatus
    {
        None,
        Close,
        Open,
        Hide
    }

    public interface IPresenter
    {
        void SetView(IView view);
    }

    public interface IUIPresenter : IPresenter, IDisposable
    {
        string     Id   { get; }
        string     Name { get; }
        void       SetViewParent(Transform parent);
        ViewStatus ViewStatus { get; }
        UniTask    OnViewReady();
        void       OpenView();
        UniTask    OpenViewAsync();
        void       CloseView();
        UniTask    CloseViewAsync();
        void       HideView();
        void       DestroyView();
        UniTask    BindData();
    }

    public interface IUIPresenter<in TModel> : IUIPresenter
    {
        void SetModel(TModel model);
    }
}