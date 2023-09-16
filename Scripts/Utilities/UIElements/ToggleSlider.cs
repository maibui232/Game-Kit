namespace GameKit.Utilities.UIElements
{
    using System;
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.UI;

    public class ToggleSlider : MonoBehaviour
    {
        [SerializeField] private Button handleBtn;
        [SerializeField] private Slider slider;
        [SerializeField] private float  duration = 0.5f;

        private bool isOn;

        public bool IsOn
        {
            get => this.isOn;
            set
            {
                this.isOn = value;
                if (this.isOn) this.MoveToOn();
                else this.MoveToOff();
            }
        }

        public event Action<bool> ChangeValue;

        public void Init()
        {
            if (this.isOn) this.MoveToOn(true);
            else this.MoveToOff(true);
        }

        private void Awake()
        {
            this.slider.interactable = false;
            this.handleBtn.onClick.AddListener(this.OnClick);
        }

        private void OnClick()
        {
            this.IsOn = !this.IsOn;
            this.OnStartChangeToggle();
        }

        protected virtual void OnChangeValue(bool obj) { this.ChangeValue?.Invoke(obj); }

        private void BlockRaycast(bool isBlock) { this.handleBtn.targetGraphic.raycastTarget = isBlock; }

        private void OnStartChangeToggle()
        {
            this.OnChangeValue(this.isOn);
            this.BlockRaycast(false);
        }

        private void OnCompleteChangeToggle() { this.BlockRaycast(true); }

        private void MoveToOn(bool immediate = false)
        {
            if (immediate)
            {
                this.slider.value = 1f;
                this.OnCompleteChangeToggle();
                return;
            }

            var currentValue = 0f;
            DOTween.To(() => currentValue, x => currentValue = x, 1f, this.duration)
                .OnStart(this.OnStartChangeToggle)
                .OnUpdate(() => this.slider.value = currentValue)
                .OnComplete(this.OnCompleteChangeToggle);
        }

        private void MoveToOff(bool immediate = false)
        {
            if (immediate)
            {
                this.slider.value = 0f;
                this.OnCompleteChangeToggle();
                return;
            }

            var currentValue = 1f;
            DOTween.To(() => currentValue, x => currentValue = x, 0f, this.duration)
                .OnStart(this.OnStartChangeToggle)
                .OnUpdate(() => this.slider.value = currentValue)
                .OnComplete(this.OnCompleteChangeToggle);
        }
    }
}