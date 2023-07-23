namespace GDK.Scripts.UI.Service.Manager
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using GDK.Scripts.Addressable;
    using GDK.Scripts.UI.Attribute;
    using GDK.Scripts.UI.Interface;
    using UnityEngine;
    using VContainer;
    using Object = UnityEngine.Object;

    public interface IUIManager
    {
        UniTask<TPresenter>   OpenView<TPresenter>(bool stackView = true) where TPresenter : IUIPresenter;
        UniTask<TPresenter>   OpenView<TPresenter, TModel>(TModel model, bool stackView = true) where TPresenter : IUIPresenter<TModel> where TModel : IModel;
        UniTask<IUIPresenter> GetCurrentView();
        UniTask               CloseCurrentView();
        UniTask               CloseAllView();
        UniTask               DestroyCurrentView();
        UniTask               DestroyAllView();
    }

    public abstract class BaseUIManager : IUIManager
    {
        #region Inject

        protected readonly IAddressableServices AddressableServices;
        protected readonly IObjectResolver      ObjectResolver;
        protected readonly RootUI               RootUI;

        #endregion

        // protected Dictionary<string, IUIPresenter> IdToPresenter = new();
        protected Dictionary<string, IView> IdToView       = new();
        protected Stack<IUIPresenter>       PresenterStack = new();

        protected TUIInfo GetUIInfo<TUIInfo>(object presenter) where TUIInfo : UIInfoAttribute { return (TUIInfo)Attribute.GetCustomAttribute(presenter.GetType(), typeof(TUIInfo)); }

        protected BaseUIManager(IAddressableServices addressableServices, IObjectResolver objectResolver, RootUI rootUI)
        {
            this.AddressableServices = addressableServices;
            this.ObjectResolver      = objectResolver;
            this.RootUI              = rootUI;
        }

        private async void StackView<TPresenter>(TPresenter presenter, bool isStack) where TPresenter : IUIPresenter
        {
            var currentView = await this.GetCurrentView();
            if (currentView == null)
            {
                this.PresenterStack.Push(presenter);
                return;
            }

            if (isStack)
            {
                currentView.HideView();
            }
            else
            {
                this.PresenterStack.Pop();
                await currentView.CloseViewAsync();
            }

            this.PresenterStack.Push(presenter);
        }

        public async UniTask<TPresenter> OpenView<TPresenter>(bool stackView = true) where TPresenter : IUIPresenter
        {
            var presenter = this.ObjectResolver.Resolve<TPresenter>();
            var uiInfo    = this.GetUIInfo<UIInfoAttribute>(presenter);
            if (this.IdToView.TryGetValue(uiInfo.AddressableId, out var view))
            {
                presenter.SetView(view);
                await presenter.OpenViewAsync();
                presenter.BindData();
                this.StackView(presenter, stackView);
                return presenter;
            }

            var viewPrefab = await this.AddressableServices.LoadAsset<GameObject>(uiInfo.AddressableId);
            var viewSpawn  = Object.Instantiate(viewPrefab).GetComponent<IView>();
            this.IdToView.Add(uiInfo.AddressableId, viewSpawn);
            presenter.SetView(viewSpawn);
            presenter.BindData();
            this.StackView(presenter, stackView);
            return presenter;
        }

        public async UniTask<TPresenter> OpenView<TPresenter, TModel>(TModel model, bool stackView = true) where TPresenter : IUIPresenter<TModel> where TModel : IModel
        {
            var presenter = await this.OpenView<TPresenter>(stackView);
            presenter.SetModel(model);
            return presenter;
        }

        public abstract UniTask<IUIPresenter> GetCurrentView();

        public async UniTask CloseCurrentView()
        {
            var currentView = await this.GetCurrentView();
            currentView.CloseViewAsync();
            this.PresenterStack.Pop();
        }

        public async UniTask CloseAllView()
        {
            foreach (var uiPresenter in this.PresenterStack)
            {
                await uiPresenter.CloseViewAsync();
            }

            this.PresenterStack.Clear();
        }

        public async UniTask DestroyCurrentView()
        {
            var currenView = await this.GetCurrentView();
            currenView.DestroyView();
            this.PresenterStack.Pop();
        }

        public UniTask DestroyAllView()
        {
            foreach (var uiPresenter in this.PresenterStack)
            {
                uiPresenter.DestroyView();
            }

            this.PresenterStack.Clear();
            return UniTask.CompletedTask;
        }
    }
}