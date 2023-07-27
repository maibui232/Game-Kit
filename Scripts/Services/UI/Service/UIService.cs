namespace GDK.Scripts.Services.UI.Service
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using GDK.Scripts.Exception;
    using GDK.Scripts.Services.Addressable;
    using GDK.Scripts.Services.UI.Base;
    using GDK.Scripts.Services.UI.CustomAttribute;
    using GDK.Scripts.Services.UI.Interface;
    using UnityEngine;
    using VContainer;
    using Object = UnityEngine.Object;

    public interface IUIService
    {
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

        #endregion

        private readonly Dictionary<string, IView> idToView       = new();
        private readonly Stack<IUIPresenter>       presenterStack = new();


        protected UIService(IAddressableServices addressableServices, IObjectResolver objectResolver, RootUI rootUI)
        {
            this.addressableServices = addressableServices;
            this.objectResolver      = objectResolver;
            this.rootUI              = rootUI;
        }

        private IUIPresenter GetCurrentView()
        {
            if (this.presenterStack.Count == 0) return null;
            return this.presenterStack.Peek() is { ViewStatus: ViewStatus.Open } ? this.presenterStack.Peek() : null;
        }

        private TUIInfo GetUIInfo<TUIInfo>(object presenter) where TUIInfo : ScreenInfoAttribute { return (TUIInfo)Attribute.GetCustomAttribute(presenter.GetType(), typeof(TUIInfo)); }

        private void StackView<TPresenter>(TPresenter presenter) where TPresenter : IUIPresenter
        {
            if (this.IsOverlay(presenter))
            {
                this.presenterStack.Push(presenter);
                return;
            }

            var currentView = this.GetCurrentView();
            if (currentView == null)
            {
                this.presenterStack.Push(presenter);
                return;
            }

            this.HideCurrentView();
            this.presenterStack.Push(presenter);
        }

        private bool IsPopup<TPresenter>() { return typeof(TPresenter).IsSubclassOf(typeof(BaseScreenPopupPresenter<>)); }

        private bool IsOverlay<TPresenter>(TPresenter presenter) { return this.IsPopup<TPresenter>() && this.GetUIInfo<PopupInfoAttribute>(presenter).Overlay; }

        public async UniTask<TPresenter> OpenView<TPresenter>() where TPresenter : IUIPresenter
        {
            var presenter = this.objectResolver.Resolve<TPresenter>();
            var uiInfo    = this.GetUIInfo<ScreenInfoAttribute>(presenter);
            var view      = await this.GetView(presenter, uiInfo);
            presenter.SetView(view);
            await presenter.OpenViewAsync();
            presenter.BindData();
            this.StackView(presenter);
            return presenter;
        }

        public async UniTask<TPresenter> OpenView<TPresenter, TModel>(TModel model) where TPresenter : IUIPresenter<TModel> where TModel : IModel
        {
            var presenter = await this.OpenView<TPresenter>();
            presenter.SetModel(model);
            return presenter;
        }

        private async UniTask<IView> GetView(IUIPresenter presenter, ScreenInfoAttribute screenInfo)
        {
            var isOverlay = this.IsOverlay(presenter);
            if (this.idToView.TryGetValue(screenInfo.AddressableId, out var view))
            {
                view.SetParent(isOverlay ? this.rootUI.OverlayRect : this.rootUI.MainRect);
                return view;
            }

            var viewPrefab = await this.addressableServices.LoadAsset<GameObject>(screenInfo.AddressableId);

            var viewSpawn = Object.Instantiate(viewPrefab, isOverlay ? this.rootUI.OverlayRect : this.rootUI.MainRect).GetComponent<IView>();

            if (viewSpawn == null)
            {
                Debug.LogException(new GdkNullViewException<IView>());
            }

            this.idToView.Add(screenInfo.AddressableId, viewSpawn);
            return viewSpawn;
        }

        public async UniTask CloseCurrentView()
        {
            var currentView = this.GetCurrentView();
            currentView.SetViewParent(this.rootUI.CloseRect);
            await currentView.CloseViewAsync();
            currentView.Dispose();
            this.presenterStack.Pop();
        }

        public async UniTask CloseAllView()
        {
            foreach (var uiPresenter in this.presenterStack)
            {
                uiPresenter.SetViewParent(this.rootUI.CloseRect);
                await uiPresenter.CloseViewAsync();
                uiPresenter.Dispose();
            }

            this.presenterStack.Clear();
        }
        public void HideCurrentView()
        {
            var currentView = this.GetCurrentView();
            currentView.HideView();
        }

        public void HideAllView()
        {
            foreach (var uiPresenter in this.presenterStack)
            {
                uiPresenter.HideView();
            }
        }

        public void DestroyCurrentView()
        {
            var currenView = this.GetCurrentView();
            currenView.DestroyView();
            this.presenterStack.Pop();
        }

        public UniTask DestroyAllView()
        {
            foreach (var uiPresenter in this.presenterStack)
            {
                uiPresenter.DestroyView();
            }

            this.presenterStack.Clear();
            return UniTask.CompletedTask;
        }
    }
}