namespace GameKit.Services.Utils.CommonSystem.GameSetting
{
    public class ChangeInputDeviceMessage
    {
        public InputDevice Device;
        public ChangeInputDeviceMessage(InputDevice device) { this.Device = device; }
    }
}