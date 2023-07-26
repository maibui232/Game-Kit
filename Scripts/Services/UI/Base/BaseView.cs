namespace GDK.Scripts.Services.UI.Base
{
    using GDK.Scripts.Services.UI.Interface;
    using UnityEngine;

    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(RectTransform))]
    public class BaseView : MonoBehaviour, IView
    {
        private CanvasGroup   canvasGroup;
        private RectTransform rootView;

        private void Awake()
        {
            this.canvasGroup = this.GetComponent<CanvasGroup>();
            this.rootView    = this.GetComponent<RectTransform>();
        }

        public RectTransform RootView => this.rootView ??= this.GetComponent<RectTransform>();

        private void SetViewAlpha(float alpha) { this.canvasGroup.alpha = alpha; }

        private void SetBlockRaycast(bool isBlock) { this.canvasGroup.blocksRaycasts = isBlock; }

        private void ShowView()
        {
            this.SetViewAlpha(1);
            this.SetBlockRaycast(true);
        }

        public void HideView()
        {
            this.SetViewAlpha(0);
            this.SetBlockRaycast(false);
        }

        public void OpenView()
        {
            // play animation open and set parent is close canvas
            this.ShowView();
        }

        public void CloseView()
        {
            // play animation close and set parent is close canvas
            this.HideView();
        }

        public void DestroyView() { Destroy(this.gameObject); }

        public void SetParent(Transform parent) { this.transform.SetParent(parent, false); }
    }
}