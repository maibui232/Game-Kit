namespace GameKit.Services.ApplicationManager
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
            this.messageService.SendMessage(new ApplicationStateChangeMessage(ApplicationState.Pause));
        }

        private void OnApplicationQuit()
        {
            this.messageService.SendMessage(new ApplicationStateChangeMessage(ApplicationState.Quit));
            Debug.Log("QUIT");
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus) return;
            this.messageService.SendMessage(new ApplicationStateChangeMessage(ApplicationState.Run));
        }
    }
}