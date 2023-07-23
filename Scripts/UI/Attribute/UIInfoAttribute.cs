namespace GDK.Scripts.UI.Attribute
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

        public PopupAttribute(string addressableId, bool blur = false, bool closeWhenClickOutSize = false) : base(addressableId)
        {
            this.Blur                  = blur;
            this.CloseWhenClickOutSize = closeWhenClickOutSize;
        }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class PageAttribute : UIInfoAttribute
    {
        public bool Moveable;

        public PageAttribute(string addressableId, bool moveable = false) : base(addressableId) { this.Moveable = moveable; }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ToolTipAttribute : UIInfoAttribute
    {
        public ToolTipAttribute(string addressableId) : base(addressableId) { }
    }
}