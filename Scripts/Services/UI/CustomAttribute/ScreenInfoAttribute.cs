namespace GameKit.Services.UI.CustomAttribute
{
    using System;

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ScreenInfoAttribute : Attribute
    {
        public string AddressableId;

        public ScreenInfoAttribute(string addressableId) { this.AddressableId = addressableId; }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class PopupInfoAttribute : ScreenInfoAttribute
    {
        public bool Blur;
        public bool CloseWhenClickOutSize;
        public bool Overlay;

        public PopupInfoAttribute(string addressableId, bool blur = false, bool closeWhenClickOutSize = false, bool overlay = false) : base(addressableId)
        {
            this.Blur                  = blur;
            this.CloseWhenClickOutSize = closeWhenClickOutSize;
            this.Overlay               = overlay;
        }
    }
}