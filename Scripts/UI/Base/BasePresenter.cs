namespace GDK.Scripts.UI.Base
{
    using System.Threading.Tasks;
    using Cysharp.Threading.Tasks;
    using GDK.Scripts.UI.Interface;
    using GDK.Scripts.Utils.SceneServices;

    public abstract class BasePresenter<TView> : IUIPresenter where TView : BaseView
    {
        public          string     Id         => $"{SceneService.Instance.CurrentSceneName}/{this.Name}";
        public          string     Name       => this.View.RootView.name;
        public          ViewStatus ViewStatus { get; private set; }
        public abstract ScreenType ScreenType { get; }

        public TView View { get; private set; }

        public void SetView(IView view) { this.View = (TView)view; }

        public virtual void Dispose() { }

        public UniTask OnViewReady() { return UniTask.WaitUntil(() => this.View != null); }

        public async void OpenView()
        {
            if (this.ViewStatus == ViewStatus.Open) return;
            await this.OnViewReady();
            this.View.OpenView();

            this.ViewStatus = ViewStatus.Open;
        }

        public virtual UniTask OpenViewAsync()
        {
            this.OpenView();
            return UniTask.CompletedTask;
        }

        public void CloseView()
        {
            if (this.ViewStatus == ViewStatus.Close) return;
            this.View.CloseView();
        }

        public UniTask CloseViewAsync()
        {
            this.CloseView();
            return UniTask.CompletedTask;
        }
        public void HideView()
        {
            if (this.ViewStatus == ViewStatus.Hide) return;
            this.View.HideView();
            this.ViewStatus = ViewStatus.Hide;
        }

        public void DestroyView() { this.View.DestroyView(); }

        public abstract UniTask BindData();
    }

    public abstract class BasePresenter<TView, TModel> : BasePresenter<TView>, IUIPresenter<TModel> where TView : BaseView where TModel : IModel
    {
        private TaskCompletionSource<bool> isSetModel = new(false);

        public TModel Model { get; private set; }

        public void SetModel(TModel model)
        {
            this.Model = model;
            this.isSetModel.TrySetResult(true);
        }

        public override async UniTask BindData()
        {
            await this.isSetModel.Task;
            this.BindData(this.Model);
            await UniTask.CompletedTask;
        }

        public abstract UniTask BindData(TModel model);
    }

    public abstract class BaseScreenPresenter<TView> : BasePresenter<TView> where TView : BaseView
    {
        public override ScreenType ScreenType => ScreenType.Screen;
    }

    public abstract class BaseScreenPresenter<TView, TModel> : BasePresenter<TView, TModel> where TView : BaseView where TModel : IModel
    {
        public override ScreenType ScreenType => ScreenType.Screen;
    }

    public abstract class BasePopupPresenter<TView> : BasePresenter<TView> where TView : BaseView
    {
        public override ScreenType ScreenType => ScreenType.Popup;
    }

    public abstract class BasePopupPresenter<TView, TModel> : BasePresenter<TView, TModel> where TView : BaseView where TModel : IModel
    {
        public override ScreenType ScreenType => ScreenType.Screen;
    }

    public abstract class BasePagePresenter<TView> : BasePresenter<TView> where TView : BaseView
    {
        public override ScreenType ScreenType => ScreenType.Page;
    }

    public abstract class BasePagePresenter<TView, TModel> : BasePresenter<TView, TModel> where TView : BaseView where TModel : IModel
    {
        public override ScreenType ScreenType => ScreenType.Page;
    }

    public abstract class BaseTooltipPresenter<TView> : BasePresenter<TView> where TView : BaseView
    {
        public override ScreenType ScreenType => ScreenType.Tooltip;
    }

    public abstract class BaseTooltipPresenter<TView, TModel> : BasePresenter<TView, TModel> where TView : BaseView where TModel : IModel
    {
        public override ScreenType ScreenType => ScreenType.Tooltip;
    }
}