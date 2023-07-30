namespace GDK.Scripts.Test
{
    using System.Threading.Tasks;
    using GDK.Scripts.Services.Message;
    using GDK.Scripts.Services.UI.Service;
    using GDK.Scripts.Test.Message;
    using UnityEngine;
    using VContainer;

    public class Test : MonoBehaviour
    {
        private IUIService      uiService;
        private IMessageService messageService;

        private TaskCompletionSource<bool> isInjected = new(false);

        [Inject]
        private void Init(IUIService uiService, IMessageService messageService)
        {
            this.uiService      = uiService;
            this.messageService = messageService;
            this.isInjected.TrySetResult(true);

            this.uiService.OpenView<TestScreenScreenPresenter>();
            this.messageService.Subscribe<int, TestMessage>(1, this.OnShowMessage);
            this.messageService.Subscribe<int, TestMessage>(2, this.OnShowMessage2);
        }

        private async void OnDestroy()
        {
            await this.isInjected.Task;
            this.messageService.UnSubscribe<int, TestMessage>(1, this.OnShowMessage);
            this.messageService.UnSubscribe<int, TestMessage>(2, this.OnShowMessage2);
        }

        private void OnShowMessage(TestMessage obj) { Debug.Log($"1: {obj.Message}"); }

        private void OnShowMessage2(TestMessage obj) { Debug.Log($"2: {obj.Message}"); }
    }
}