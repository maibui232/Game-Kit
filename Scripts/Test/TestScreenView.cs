namespace GDK.Scripts.Test
{
    using Cysharp.Threading.Tasks;
    using GDK.Scripts.Services.UI.Base;
    using GDK.Scripts.Services.UI.CustomAttribute;
    using GDK.Scripts.Services.UI.Service;
    using UnityEngine;
    using UnityEngine.UI;

    public class TestScreenView : BaseView
    {
        [SerializeField] private Button changeButton;
        public                   Button ChangeButton => this.changeButton;
    }

    [ScreenInfo(nameof(TestScreenView))]
    public class TestScreenScreenPresenter : BaseScreenPresenter<TestScreenView>
    {
        #region Inject

        private readonly IUIService uiService;

        #endregion

        public TestScreenScreenPresenter(IUIService uiService) { this.uiService = uiService; }

        public override async UniTask OnViewReady()
        {
            await base.OnViewReady();
            this.View.ChangeButton.onClick.AddListener(this.OnClickChange);
        }

        public override UniTask BindData() { return UniTask.CompletedTask; }

        private void OnClickChange() { this.uiService.OpenView<TestPopup2Presenter>(); }
    }
}