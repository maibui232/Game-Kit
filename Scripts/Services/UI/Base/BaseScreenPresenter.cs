namespace GameKit.Services.UI.Base
{
    using System.Threading.Tasks;
    using Cysharp.Threading.Tasks;
    using GameKit.Services.UI.Interface;
    using UnityEngine;

    public abstract class BaseScreenPresenter<TView> : IUIPresenter where TView : BaseView
    {
        public string     Id         => $"{this.Name}";
        public string     Name       => this.View.RootView.name;
        public ViewStatus ViewStatus { get; private set; }

        private bool onViewReady;

        public TView View { get; private set; }

        public void SetView(IView view)
        {
            this.onViewReady = this.View == null;
            this.View        = (TView)view;
        }

        public void SetViewParent(Transform parent) { this.View.SetParent(parent); }

        public virtual void Dispose() { }

        public virtual UniTask OnViewReady() { return UniTask.WaitUntil(() => this.View != null); }

        public async void OpenView()
        {
            if (this.ViewStatus == ViewStatus.Open) return;
            if (this.onViewReady)
            {
                await this.OnViewReady();
            }

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
            this.ViewStatus = ViewStatus.Close;
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

    public abstract class BaseScreenPresenter<TView, TModel> : BaseScreenPresenter<TView>, IUIPresenter<TModel> where TView : BaseView where TModel : IModel
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

    public abstract class BasePopupPresenter<TView> : BaseScreenPresenter<TView> where TView : BaseView
    {
    }

    public abstract class BasePopupPresenter<TView, TModel> : BaseScreenPresenter<TView, TModel> where TModel : IModel where TView : BaseView
    {
    }
}