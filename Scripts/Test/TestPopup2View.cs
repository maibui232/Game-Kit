namespace GDK.Scripts.Test
{
    using Cysharp.Threading.Tasks;
    using GDK.Scripts.Services.UI.Base;
    using GDK.Scripts.Services.UI.CustomAttribute;
    using GDK.Scripts.Services.UI.Service;
    using UnityEditor.Overlays;
    using UnityEngine;
    using UnityEngine.UI;

    public class TestPopup2View : BaseView
    {
        [SerializeField] private Button changeButton;
        public                   Button ChangeButton => this.changeButton;
    }

    [PopupInfo(nameof(TestPopup2View), overlay: true)]
    public class TestPopup2Presenter : BaseScreenPopupPresenter<TestPopup2View>
    {
        private readonly IUIService uiService;

        public TestPopup2Presenter(IUIService uiService) { this.uiService = uiService; }
        public override UniTask BindData()
        {
            this.View.ChangeButton.onClick.AddListener(this.OnChange);

            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
            base.Dispose();
            this.View.ChangeButton.onClick.RemoveAllListeners();
        }

        private void OnChange() { this.uiService.OpenView<TestScreenScreenPresenter>(); }
    }
}