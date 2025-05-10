using System.Diagnostics;
using Frontier_UI;
using Game_UI;
using Unity.Collections.LowLevel.Unsafe;
using VContainer;

namespace Settings_Screen
{
    public interface ISoundObserver
    {
        void Enable();
        void Disable();
    }

    public class SoundObserver : ISoundObserver
    {
        [Inject] private ISettingsSlotMouseEvents slotMouse;
        private ISettingsSlotSlider view;

        public SoundObserver(ISettingsSlotSlider view)
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