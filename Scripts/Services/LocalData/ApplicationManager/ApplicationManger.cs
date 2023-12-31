namespace GameKit.Services.LocalData.ApplicationManager
{
    using GameKit.Services.Message;
    using UnityEngine;
    using VContainer;

    public class ApplicationManger : MonoBehaviour
    {
        #region Inject

        private IMessageService messageService;

        #endregion

        [Inject]
        private void Init(IMessageService messageService) { this.messageService = messageService; }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (!pauseStatus) return;
            this.messageService.Send(new ApplicationStateChangeMessage(ApplicationState.Pause));
        }

        private void OnApplicationQuit() { this.messageService.Send(new ApplicationStateChangeMessage(ApplicationState.Quit)); }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus) return;
            this.messageService.Send(new ApplicationStateChangeMessage(ApplicationState.Run));
        }
    }
}