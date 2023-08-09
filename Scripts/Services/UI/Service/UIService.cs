namespace GameKit.Services.UI.Service
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using GameKit.Exception;
    using GameKit.Extensions;
    using GameKit.Services.Addressable;
    using GameKit.Services.Logger;
    using GameKit.Services.UI.Base;
    using GameKit.Services.UI.CustomAttribute;
    using GameKit.Services.UI.Interface;
    using UnityEngine;
    using VContainer;
    using Object = UnityEngine.Object;

    public interface IUIService
    {
        IUIPresenter        CurrentUIPresenter { get; }
        UniTask<TPresenter> OpenView<TPresenter>() where TPresenter : IUIPresenter;
        UniTask<TPresenter> OpenView<TPresenter, TModel>(TModel model) where TPresenter : IUIPresenter<TModel> where TModel : IModel;
        UniTask             CloseCurrentView();
        UniTask             CloseAllView();
        void                HideCurrentView();
        void                HideAllView();
        void                DestroyCurrentView();
        UniTask             DestroyAllView();
    }

    public class UIService : IUIService
    {
        #region Inject

        private readonly IAddressableServices addressableServices;
        private readonly IObjectResolver      objectResolver;
        private readonly RootUI               rootUI;
        private readonly ILoggerService       logger;

        #endregion

        private readonly List<IUIPresenter>      uiPresenterStack    = new();
        private readonly Dictionary<Type, IView> presenterTypeToView = new();

        protected UIService(IAddressableServices addressableServices, IObjectResolver objectResolver, RootUI rootUI, ILoggerService logger)
        {
            this.addressableServices = addressableServices;
            this.objectResolver      = objectResolver;
            this.rootUI              = rootUI;
            this.logger              = logger;
        }

        private IUIPresenter currentScreenShow;

        public IUIPresenter CurrentUIPresenter
        {
            get => this.currentScreenShow ?? (this.uiPresenterStack.Count == 0 ? null : this.uiPresenterStack[^1]);
            private set => this.currentScreenShow = value;
        }

        private TUIInfo GetUIInfo<TUIInfo>(object presenter) where TUIInfo : ScreenInfoAttribute { return (TUIInfo)Attribute.GetCustomAttribute(presenter.GetType(), typeof(TUIInfo)); }

        private void StackView<TPresenter>(TPresenter presenter) where TPresenter : IUIPresenter
        {
            if (this.IsOverlay(presenter))
            {
                return;
            }

            if (this.CurrentUIPresenter == null)
            {
                this.uiPresenterStack.Add(presenter);
                return;
            }

            this.HideCurrentView();
            if (this.uiPresenterStack.Contains(presenter))
            {
                this.uiPresenterStack.Remove(presenter);
            }

            this.uiPresenterStack.Add(presenter);
        }

        private bool IsPopup<TPresenter>() { return typeof(BasePopupPresenter<>).IsSubclassOfRawGeneric(typeof(TPresenter)); }

        private bool IsOverlay<TPresenter>(TPresenter presenter) { return this.IsPopup<TPresenter>() && this.GetUIInfo<PopupInfoAttribute>(presenter).Overlay; }

        private async UniTask<TPresenter> InitPresenter<TPresenter>() where TPresenter : IUIPresenter
        {
            var presenter = this.objectResolver.Resolve<TPresenter>();
            var uiInfo    = this.GetUIInfo<ScreenInfoAttribute>(presenter);
            var view      = await this.GetView(presenter, uiInfo);

            presenter.SetView(view);
            await presenter.OpenViewAsync();
            this.StackView(presenter);
            this.CurrentUIPresenter = presenter;
            return presenter;
        }

        public async UniTask<TPresenter> OpenView<TPresenter>() where TPresenter : IUIPresenter
        {
            var presenter = await this.InitPresenter<TPresenter>();
            presenter.BindData();

            this.logger.Log(Color.green, $"Open view: {presenter}");
            return presenter;
        }

        public async UniTask<TPresenter> OpenView<TPresenter, TModel>(TModel model) where TPresenter : IUIPresenter<TModel> where TModel : IModel
        {
            var presenter = await this.InitPresenter<TPresenter>();
            presenter.SetModel(model);
            presenter.BindData();

            this.logger.Log(Color.green, $"Open view: {presenter}");
            return presenter;
        }

        private async UniTask<IView> GetView(IUIPresenter presenter, ScreenInfoAttribute screenInfo)
        {
            var isOverlay     = this.IsOverlay(presenter);
            var presenterType = presenter.GetType();
            if (this.presenterTypeToView.TryGetValue(presenterType, out var view))
            {
                if (view == null)
                {
                    this.presenterTypeToView.Remove(presenterType);
                }
                else
                {
                    view.SetParent(isOverlay ? this.rootUI.OverlayRect : this.rootUI.MainRect);
                    return view;
                }
            }

            var viewPrefab = await this.addressableServices.LoadAsset<GameObject>(screenInfo.AddressableId);
            var viewSpawn  = Object.Instantiate(viewPrefab, isOverlay ? this.rootUI.OverlayRect : this.rootUI.MainRect).GetComponent<IView>();

            if (viewSpawn == null)
            {
                throw new GdkException($"This view instantiate does not contain: {typeof(IView)}, Add Component: {typeof(IView)}, please!");
            }

            this.presenterTypeToView.Add(presenter.GetType(), viewSpawn);
            return viewSpawn;
        }

        public async UniTask CloseCurrentView()
        {
            var currentView = this.CurrentUIPresenter;
            currentView.SetViewParent(this.rootUI.CloseRect);
            await currentView.CloseViewAsync();
            currentView.Dispose();

            if (this.uiPresenterStack.Count == 0) return;
            this.CurrentUIPresenter = this.uiPresenterStack[^1];
            this.uiPresenterStack.RemoveAt(this.uiPresenterStack.Count - 1);
            if (this.CurrentUIPresenter != null) await this.CurrentUIPresenter.OpenViewAsync();
        }

        public async UniTask CloseAllView()
        {
            foreach (var uiPresenter in this.uiPresenterStack)
            {
                uiPresenter.SetViewParent(this.rootUI.CloseRect);
                await uiPresenter.CloseViewAsync();
                uiPresenter.Dispose();
            }

            this.uiPresenterStack.Clear();
        }

        public void HideCurrentView()
        {
            var currentPresenter = this.CurrentUIPresenter;
            currentPresenter?.HideView();
        }

        public void HideAllView()
        {
            foreach (var presenter in this.uiPresenterStack)
            {
                presenter.HideView();
            }
        }

        public void DestroyCurrentView()
        {
            var currenView = this.CurrentUIPresenter;
            currenView.DestroyView();
        }

        public UniTask DestroyAllView()
        {
            foreach (var uiPresenter in this.uiPresenterStack)
            {
                uiPresenter.DestroyView();
            }

            this.uiPresenterStack.Clear();
            this.CurrentUIPresenter = null;
            return UniTask.CompletedTask;
        }
    }
}