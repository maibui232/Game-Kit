namespace GDK.Scripts.Services.UI.CustomAttribute
{
    using System;

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class UIInfoAttribute : Attribute
    {
        public string AddressableId;

        public UIInfoAttribute(string addressableId) { this.AddressableId = addressableId; }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ScreenInfoAttribute : UIInfoAttribute
    {
        public ScreenInfoAttribute(string addressableId) : base(addressableId) { }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class PopupAttribute : UIInfoAttribute
    {
        public bool Blur;
        public bool CloseWhenClickOutSize;
        public bool Overlay;

        public PopupAttribute(string addressableId, bool blur = false, bool closeWhenClickOutSize = false, bool overlay = false) : base(addressableId)
        {
            this.Blur                  = blur;
            this.CloseWhenClickOutSize = closeWhenClickOutSize;
            this.Overlay               = overlay;
        }
    }
}