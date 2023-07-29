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
            this.messageService.Subscribe<TestMessage>(this.OnShowMessage);
        }

        private async void OnDestroy()
        {
            await this.isInjected.Task;
            this.messageService.UnSubscribe<TestMessage>(this.OnShowMessage);
        }

        private void OnShowMessage(TestMessage obj) { Debug.Log(obj.Message); }
    }
}