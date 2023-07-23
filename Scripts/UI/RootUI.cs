namespace GDK.Scripts.UI
{
    using UnityEngine;

    public class RootUI : MonoBehaviour
    {
        [SerializeField] private RectTransform screenRect;
        [SerializeField] private RectTransform popupRect;
        [SerializeField] private RectTransform pageRect;
        [SerializeField] private RectTransform tooltipRect;

        public RectTransform ScreenRect  => this.screenRect;
        public RectTransform PopupRect   => this.popupRect;
        public RectTransform PageRect    => this.pageRect;
        public RectTransform TooltipRect => this.tooltipRect;
    }
}