namespace GDK.Scripts.Services.UI
{
    using UnityEngine;

    public class RootUI : MonoBehaviour
    {
        [SerializeField] private RectTransform mainRect;
        [SerializeField] private RectTransform popupRect;

        public RectTransform MainRect  => this.mainRect;
        public RectTransform PopupRect => this.popupRect;
    }
}