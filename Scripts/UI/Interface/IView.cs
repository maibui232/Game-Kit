namespace GDK.Scripts.UI.Interface
{
    using UnityEngine;

    public interface IView
    {
        void          SetParent(Transform parent);
        RectTransform RootView { get; }

        void ShowView();

        void HideView();

        void OpenView();

        void CloseView();

        void DestroyView();
    }
}