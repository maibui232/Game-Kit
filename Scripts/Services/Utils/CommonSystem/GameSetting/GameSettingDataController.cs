namespace GameKit.Services.Utils.CommonSystem.GameSetting
{
    using GameKit.Services.LocalData;
    using GameKit.Services.Message;

    public class GameSettingDataController
    {
        #region Inject

        private readonly InputDeviceLocalData inputDeviceLocalData;
        private readonly SoundLocalData       soundLocalData;
        private readonly IMessageService      messageService;

        #endregion

        public GameSettingDataController(InputDeviceLocalData inputDeviceLocalData, SoundLocalData soundLocalData, IMessageService messageService)
        {
            this.inputDeviceLocalData = inputDeviceLocalData;
            this.soundLocalData       = soundLocalData;
            this.messageService       = messageService;
        }

        public InputDevice CurrentInputDevice => this.inputDeviceLocalData.InputDevice;
        public bool        MusicOn            => this.soundLocalData.MusicOn;
        public bool        SoundOn            => this.soundLocalData.SoundOn;
        public bool        VibrateOn          => this.soundLocalData.VibrateOn;

        public void ChangeInputDevice(InputDevice device)
        {
            this.inputDeviceLocalData.InputDevice = device;
            this.messageService.Send(new ChangeInputDeviceMessage(device));
        }

        public void ChangeSoundSetting(bool isOn) { this.soundLocalData.SoundOn = isOn; }

        public void ChangeMusicSetting(bool isOn) { this.soundLocalData.MusicOn = isOn; }

        public void ChangeVibrateSetting(bool isOn) { this.soundLocalData.VibrateOn = isOn; }
    }

    public class InputDeviceLocalData : ILocalData
    {
        public InputDevice InputDevice;

        public void Init()
        {
#if UNITY_ANDROID || UNITY_IOS
            this.InputDevice = InputDevice.Joystick;
#elif UNITY_STANDALONE
            this.InputDevice = InputDevice.Keyboard;
#endif
        }
    }

    public enum InputDevice
    {
        Keyboard,
        Joystick,
        DPad
    }

    public class SoundLocalData : ILocalData
    {
        public bool SoundOn;
        public bool MusicOn;
        public bool VibrateOn;

        public void Init()
        {
            this.SoundOn   = true;
            this.MusicOn   = true;
            this.VibrateOn = true;
        }
    }
}