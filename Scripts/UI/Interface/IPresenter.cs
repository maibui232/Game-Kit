namespace GDK.Scripts.UI.Interface
{
    using System;
    using Cysharp.Threading.Tasks;

    public enum ViewStatus
    {
        Open,
        Hide,
        Close
    }

    public enum ScreenType
    {
        Screen,
        Popup,
        Page,
        Tooltip
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
        ScreenType ScreenType { get; }
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