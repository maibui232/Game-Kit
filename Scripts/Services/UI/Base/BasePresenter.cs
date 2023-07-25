namespace GDK.Scripts.Services.UI.Base
{
    using System.Threading.Tasks;
    using Cysharp.Threading.Tasks;
    using GDK.Scripts.Services.UI.Interface;
    using GDK.Scripts.Utils.SceneServices;

    public abstract class BasePresenter<TView> : IUIPresenter where TView : BaseView
    {
        public string     Id         => $"{SceneService.Instance.CurrentSceneName}/{this.Name}";
        public string     Name       => this.View.RootView.name;
        public ViewStatus ViewStatus { get; private set; }

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
}