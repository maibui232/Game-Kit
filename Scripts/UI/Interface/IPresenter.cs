namespace GDK.Scripts.UI.Interface
{
    using System;
    using Cysharp.Threading.Tasks;

    public interface IPresenter : IDisposable
    {
        string     Id         { get; }
        string     Name       { get; }
        ViewType   ViewType   { get; }
        ViewStatus ViewStatus { get; }
        void       OpenView();
        UniTask    OpenViewAsync();
        void       CloseView();
        UniTask    CloseViewAsync();
        void       DestroyView();
        void       ShowView();
        void       HideView();
        UniTask    BindData();
    }

    public interface IPresenter<TView> : IPresenter
    {
        TView View { get; }
        void  SetView(TView view);
    }

    public interface IPresenter<TView, TModel> : IPresenter<TView>
    {
        TModel Model { get; }
        void   SetModel(TModel model);
    }

    public enum ViewStatus
    {
        Open,
        Close,
        Destroy
    }

    public enum ViewType
    {
        Screen,
        Popup,
        Page,
        Tooltip
    }
}