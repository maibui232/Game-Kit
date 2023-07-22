namespace GDK.Scripts.UI.Base
{
    using Cysharp.Threading.Tasks;
    using GDK.Scripts.UI.Interface;
    using GDK.Scripts.Utils;

    public abstract class BasePresenter<TView> : IPresenter<TView> where TView : BaseView
    {
        public string     Id         => $"{SceneDirector.Instance.CurrentSceneName}/{this.Name}";
        public string     Name       => this.View.RootView.name;
        public ViewStatus ViewStatus { get; private set; }

        public abstract ViewType ViewType { get; }

        public TView View { get; private set; }

        public void SetView(TView view) { this.View = view; }

        public void OpenView()
        {
            this.View.OpenView();
            this.ViewStatus = ViewStatus.Open;
            this.BindData();
            this.OpenViewAsync();
        }

        public virtual UniTask OpenViewAsync() { return UniTask.CompletedTask; }

        public void CloseView()
        {
            this.View.CloseView();
            this.ViewStatus = ViewStatus.Close;
            this.Dispose();
            this.CloseViewAsync();
        }

        public virtual UniTask CloseViewAsync() { return UniTask.CompletedTask; }

        public void DestroyView()
        {
            this.View.DestroyView();
            this.ViewStatus = ViewStatus.Destroy;
        }

        public void ShowView()
        {
            this.View.ShowView();
            this.ViewStatus = ViewStatus.Open;
        }

        public void HideView()
        {
            this.View.HideView();
            this.ViewStatus = ViewStatus.Close;
        }

        public abstract UniTask BindData();

        public virtual void Dispose() { }
    }

    public abstract class BasePresenter<TView, TModel> : BasePresenter<TView>, IPresenter<TView, TModel> where TView : BaseView where TModel : IModel
    {
        public TModel Model { get; private set; }

        public void SetModel(TModel model) { this.Model = model; }
    }

    public abstract class BaseScreenPresenter<TView> : BasePresenter<TView> where TView : BaseView
    {
        public override ViewType ViewType => ViewType.Screen;
    }

    public abstract class BaseScreenPresenter<TView, TModel> : BasePresenter<TView, TModel> where TView : BaseView where TModel : IModel
    {
        public override ViewType ViewType => ViewType.Screen;
    }

    public abstract class BasePopupPresenter<TView> : BasePresenter<TView> where TView : BaseView
    {
        public override ViewType ViewType => ViewType.Popup;
    }

    public abstract class BasePopupPresenter<TView, TModel> : BasePresenter<TView, TModel> where TView : BaseView where TModel : IModel
    {
        public override ViewType ViewType => ViewType.Popup;
    }

    public abstract class BasePagePresenter<TView> : BasePresenter<TView> where TView : BaseView
    {
        public override ViewType ViewType => ViewType.Page;
    }

    public abstract class BasePagePresenter<TView, TModel> : BasePresenter<TView, TModel> where TView : BaseView where TModel : IModel
    {
        public override ViewType ViewType => ViewType.Page;
    }

    public abstract class BaseTooltipPresenter<TView> : BasePresenter<TView> where TView : BaseView
    {
        public override ViewType ViewType => ViewType.Tooltip;
    }

    public abstract class BaseTooltipPresenter<TView, TModel> : BasePresenter<TView, TModel> where TView : BaseView where TModel : IModel
    {
        public override ViewType ViewType => ViewType.Tooltip;
    }
}