using Frontier_UI;
using Game_UI;
using VContainer;

namespace Settings_Screen
{
    public interface IWindowModeObserver
    {
        void Enable();
        void Disable();
    }

    public class WindowModeObserver : IWindowModeObserver
    {
        [Inject] private ISettingsSlotMouseEvents slotMouse;
        private ISettingsSlotToggle view;

        public WindowModeObserver(ISettingsSlotToggle view)
        {
            this.view = view;
        }

        public void Enable()
        {
            slotMouse.OnMouseEnter += CheckAnimChoosed;
            slotMouse.OnMouseLeave += CheckAnimUnchoosed;
        }

        public void Disable()
        {
            slotMouse.OnMouseEnter -= CheckAnimChoosed;
            slotMouse.OnMouseLeave -= CheckAnimUnchoosed;
        }

        private void CheckAnimChoosed(ISettingsSlot settingsSlot)
        {
            ISettingsSlotToggle settingsSlotSlider = settingsSlot as ISettingsSlotToggle;
            if (settingsSlotSlider == null || settingsSlotSlider != view) return;

            view.AnimateChoosed();
        }

        private void CheckAnimUnchoosed(ISettingsSlot settingsSlot)
        {
            ISettingsSlotToggle settingsSlotSlider = settingsSlot as ISettingsSlotToggle;
            if (settingsSlotSlider == null || settingsSlotSlider != view) return;

            view.AnimateUnchoosed();
        }

    }
}