namespace GDK.Scripts.UI
{
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using GDK.Scripts.UI.Interface;
    using UnityEngine;

    public interface IScreenService
    {
        UniTask<TPresenter> OpenView<TPresenter>() where TPresenter : IPresenter;
        UniTask<TPresenter> OpenView<TPresenter, TModel>() where TPresenter : IPresenter where TModel : IModel;
        UniTask             CloseCurrentView();
        UniTask             CloseAllView();
        UniTask             DestroyCurrentView();
        UniTask             DestroyAllView();
        UniTask             HideCurrentView();
        UniTask             HideAllView();
    }

    public class ScreenService : MonoBehaviour, IScreenService
    {
        private List<IPresenter> listPresenter = new();

        public UniTask<TPresenter> OpenView<TPresenter>() where TPresenter : IPresenter                               { throw new System.NotImplementedException(); }
        public UniTask<TPresenter> OpenView<TPresenter, TModel>() where TPresenter : IPresenter where TModel : IModel { throw new System.NotImplementedException(); }
        public UniTask             CloseCurrentView()                                                                 { throw new System.NotImplementedException(); }
        public UniTask             CloseAllView()                                                                     { throw new System.NotImplementedException(); }
        public UniTask             DestroyCurrentView()                                                               { throw new System.NotImplementedException(); }
        public UniTask             DestroyAllView()                                                                   { throw new System.NotImplementedException(); }
        public UniTask             HideCurrentView()                                                                  { throw new System.NotImplementedException(); }
        public UniTask             HideAllView()                                                                      { throw new System.NotImplementedException(); }
    }
}