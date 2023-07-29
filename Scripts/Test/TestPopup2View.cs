namespace GDK.Scripts.Test
{
    using Cysharp.Threading.Tasks;
    using GDK.Scripts.Services.Message;
    using GDK.Scripts.Services.UI.Base;
    using GDK.Scripts.Services.UI.CustomAttribute;
    using GDK.Scripts.Services.UI.Service;
    using GDK.Scripts.Test.Message;
    using UnityEngine;
    using UnityEngine.UI;

    public class TestPopup2View : BaseView
    {
        [SerializeField] private Button changeButton, showMessageButton;
        public                   Button ShowMessageButton => this.showMessageButton;
        public                   Button ChangeButton      => this.changeButton;
    }

    [PopupInfo(nameof(TestPopup2View), overlay: true)]
    public class TestPopup2Presenter : BasePopupPresenter<TestPopup2View>
    {
        private readonly IUIService      uiService;
        private readonly IMessageService messageService;

        public TestPopup2Presenter(IUIService uiService, IMessageService messageService)
        {
            this.uiService      = uiService;
            this.messageService = messageService;
        }
        public override UniTask BindData()
        {
            this.View.ChangeButton.onClick.AddListener(this.OnChange);
            this.View.ShowMessageButton.onClick.AddListener(this.OnClickShowMessage);

            return UniTask.CompletedTask;
        }

        private void OnClickShowMessage() { this.messageService.Publish(new TestMessage("Adu ma .....................................")); }

        public override void Dispose()
        {
            base.Dispose();
            this.View.ChangeButton.onClick.RemoveAllListeners();
            this.View.ShowMessageButton.onClick.RemoveAllListeners();
        }

        private void OnChange() { this.uiService.OpenView<TestScreenScreenPresenter>(); }
    }
}