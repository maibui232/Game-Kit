namespace GameKit.Services.UI
{
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class RootUI : MonoBehaviour
    {
        [SerializeField] private RectTransform mainRect;
        [SerializeField] private RectTransform overlayRect;
        [SerializeField] private RectTransform closeRect;
        [SerializeField] private EventSystem   eventSystem;

        public RectTransform MainRect    => this.mainRect;
        public RectTransform OverlayRect => this.overlayRect;
        public RectTransform CloseRect   => this.closeRect;
        public EventSystem   EventSystem => this.eventSystem;
    }
}