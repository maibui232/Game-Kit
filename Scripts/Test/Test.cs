namespace GDK.Scripts.Test
{
    using GDK.Scripts.Services.UI.Service;
    using UnityEngine;
    using VContainer;

    public class Test : MonoBehaviour
    {
        private IUIService uiService;

        [Inject]
        private void Init(IUIService uiService)
        {
            this.uiService = uiService;

            this.uiService.OpenView<TestScreenScreenPresenter>();
        }
    }
}