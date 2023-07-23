namespace GDK.Scripts.UI.Service
{
    using Cysharp.Threading.Tasks;
    using GDK.Scripts.UI.Interface;

    public interface IUIService
    {
        UniTask<TPresenter> OpenView<TPresenter>() where TPresenter : IUIPresenter;
        UniTask<TPresenter> OpenView<TPresenter, TModel>() where TPresenter : IPresenter where TModel : IModel;
        UniTask             CloseCurrentView();
        UniTask             CloseAllView();
        UniTask             DestroyCurrentView();
        UniTask             DestroyAllView();
        UniTask             HideCurrentView();
        UniTask             HideAllView();
    }
}