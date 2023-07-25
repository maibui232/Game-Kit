namespace GDK.Scripts.Services.UI.Interface
{
    using System;
    using Cysharp.Threading.Tasks;

    public enum ViewStatus
    {
        Open,
        Hide,
        Close
    }

    public interface IPresenter
    {
        void SetView(IView view);
    }

    public interface IUIPresenter : IPresenter, IDisposable
    {
        string     Id         { get; }
        string     Name       { get; }
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