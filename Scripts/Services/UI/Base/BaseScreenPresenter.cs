namespace GDK.Scripts.Services.UI.Base
{
    using System.Threading.Tasks;
    using Cysharp.Threading.Tasks;
    using GDK.Scripts.Services.UI.Interface;
    using GDK.Scripts.Utils.SceneServices;
    using UnityEngine;

    public abstract class BaseScreenPresenter<TView> : IUIPresenter where TView : BaseView
    {
        public string     Id         => $"{SceneService.Instance.CurrentSceneName}/{this.Name}";
        public string     Name       => this.View.RootView.name;
        public ViewStatus ViewStatus { get; private set; }

        public TView View { get; private set; }

        public void SetView(IView view) { this.View = (TView)view; }

        public void SetViewParent(Transform parent) { this.View.SetParent(parent); }

        public virtual void Dispose() { }

        public virtual UniTask OnViewReady() { return this.View != null ? UniTask.CompletedTask : UniTask.WaitUntil(() => this.View != null); }

        public async void OpenView()
        {
            if (this.ViewStatus == ViewStatus.Open) return;
            await UniTask.WaitUntil(() => this.View != null);
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