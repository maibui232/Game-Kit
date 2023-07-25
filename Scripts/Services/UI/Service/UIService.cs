namespace GDK.Scripts.Services.UI.Service
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using GDK.Scripts.Services.Addressable;
    using GDK.Scripts.Services.UI.CustomAttribute;
    using GDK.Scripts.Services.UI.Interface;
    using UnityEngine;
    using VContainer;
    using Object = UnityEngine.Object;

    public interface IUIService
    {
        UniTask<TPresenter> OpenView<TPresenter>(bool stackView = true) where TPresenter : IUIPresenter;
        UniTask<TPresenter> OpenView<TPresenter, TModel>(TModel model, bool stackView = true) where TPresenter : IUIPresenter<TModel> where TModel : IModel;
        void                CloseCurrentView();
        UniTask             CloseAllView();
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

        private IUIPresenter GetCurrentView() { return this.presenterStack.Peek() is { ViewStatus: ViewStatus.Open } ? this.presenterStack.Peek() : null; }

        private TUIInfo GetUIInfo<TUIInfo>(object presenter) where TUIInfo : UIInfoAttribute { return (TUIInfo)Attribute.GetCustomAttribute(presenter.GetType(), typeof(TUIInfo)); }

        private async void StackView<TPresenter>(TPresenter presenter, UIInfoAttribute uiInfo, bool isStack) where TPresenter : IUIPresenter
        {
            var popupInfo = (PopupAttribute)uiInfo;
            if (popupInfo is not { Overlay: false }) return;

            var currentView = this.GetCurrentView();
            if (currentView == null)
            {
                this.presenterStack.Push(presenter);
                return;
            }

            if (isStack)
            {
                currentView.HideView();
            }
            else
            {
                this.presenterStack.Pop();
                await currentView.CloseViewAsync();
            }

            this.presenterStack.Push(presenter);
        }

        public async UniTask<TPresenter> OpenView<TPresenter>(bool stackView = true) where TPresenter : IUIPresenter
        {
            var presenter = this.objectResolver.Resolve<TPresenter>();
            var uiInfo    = this.GetUIInfo<UIInfoAttribute>(presenter);
            var view      = await this.GetView(uiInfo);
            presenter.SetView(view);
            presenter.BindData();
            this.StackView(presenter, uiInfo, stackView);
            return presenter;
        }

        public async UniTask<TPresenter> OpenView<TPresenter, TModel>(TModel model, bool stackView = true) where TPresenter : IUIPresenter<TModel> where TModel : IModel
        {
            var presenter = await this.OpenView<TPresenter>(stackView);
            presenter.SetModel(model);
            return presenter;
        }

        private async UniTask<IView> GetView(UIInfoAttribute uiInfo)
        {
            if (this.idToView.TryGetValue(uiInfo.AddressableId, out var view))
            {
                return view;
            }

            var viewPrefab = await this.addressableServices.LoadAsset<GameObject>(uiInfo.AddressableId);
            var viewSpawn  = Object.Instantiate(viewPrefab).GetComponent<IView>();
            this.idToView.Add(uiInfo.AddressableId, viewSpawn);
            return viewSpawn;
        }

        public void CloseCurrentView()
        {
            var currentView = this.GetCurrentView();
            currentView.CloseViewAsync();
            this.presenterStack.Pop();
        }

        public async UniTask CloseAllView()
        {
            foreach (var uiPresenter in this.presenterStack)
            {
                await uiPresenter.CloseViewAsync();
            }

            this.presenterStack.Clear();
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