namespace GDK.Scripts.UI.Attribute
{
    using System;

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ScreenAttribute : Attribute
    {
        public string AddressableId;

        public ScreenAttribute(string addressableId) { this.AddressableId = addressableId; }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class PopupAttribute : ScreenAttribute
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
    public class PageAttribute : ScreenAttribute
    {
        public bool Moveable;

        public PageAttribute(string addressableId, bool moveable = false) : base(addressableId) { this.Moveable = moveable; }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ToolTipAttribute : ScreenAttribute
    {
        public ToolTipAttribute(string addressableId) : base(addressableId) { }
    }
}