namespace GameKit.Services.UI
{
    using UnityEngine;

    public class RootUI : MonoBehaviour
    {
        [SerializeField] private RectTransform mainRect;
        [SerializeField] private RectTransform overlayRect;
        [SerializeField] private RectTransform closeRect;

        public RectTransform MainRect    => this.mainRect;
        public RectTransform OverlayRect => this.overlayRect;
        public RectTransform CloseRect   => this.closeRect;
    }
}