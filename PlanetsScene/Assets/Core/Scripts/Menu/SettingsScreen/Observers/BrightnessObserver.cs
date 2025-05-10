using Frontier_UI;
using Game_UI;
using VContainer;

namespace Settings_Screen
{
    public interface IBrightnessObserver
    {
        void Enable();
        void Disable();
    }

    public class BrightnessObserver : IBrightnessObserver
    {
        [Inject] private ISettingsSlotMouseEvents slotMouse;
        private ISettingsSlotSlider view;

        public BrightnessObserver(ISettingsSlotSlider view)
        {
            this.view = view;
        }

        public void Enable()
        {
            SetValue(view.SliderValue);
            view.OnSliderValueChanged += SetValue;

            slotMouse.OnMouseEnter += CheckAnimChoosed;
            slotMouse.OnMouseLeave += CheckAnimUnchoosed;
        }

        public void Disable()
        {
            view.OnSliderValueChanged -= SetValue;

            slotMouse.OnMouseEnter -= CheckAnimChoosed;
            slotMouse.OnMouseLeave -= CheckAnimUnchoosed;
        }

        private void SetValue(float val)
        {
            view.Text = ((int)val).ToString();
        }

        private void CheckAnimChoosed(ISettingsSlot settingsSlot)
        {
            ISettingsSlotSlider settingsSlotSlider = settingsSlot as ISettingsSlotSlider;
            if (settingsSlotSlider == null || settingsSlotSlider != view) return;

            view.AnimateChoosed();
        }

        private void CheckAnimUnchoosed(ISettingsSlot settingsSlot)
        {
            ISettingsSlotSlider settingsSlotSlider = settingsSlot as ISettingsSlotSlider;
            if (settingsSlotSlider == null || settingsSlotSlider != view) return;

            view.AnimateUnchoosed();
        }
    }
}