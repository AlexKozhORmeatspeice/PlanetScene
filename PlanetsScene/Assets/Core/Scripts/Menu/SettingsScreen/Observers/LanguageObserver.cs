using System.Diagnostics;
using Frontier_UI;
using Game_UI;
using VContainer;

namespace Settings_Screen
{
    public interface ILanguageObserver
    {
        void Enable();
        void Disable();
    }

    public class LanguageObserver : ILanguageObserver
    {
        [Inject] private ISettingsSlotMouseEvents slotMouse;

        private ISettingsSlotDropdown view;

        public LanguageObserver(ISettingsSlotDropdown view)
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
            ISettingsSlotDropdown settingsSlotSlider = settingsSlot as ISettingsSlotDropdown;
            if (settingsSlotSlider == null || settingsSlotSlider != view) return;

            view.AnimateChoosed();
        }

        private void CheckAnimUnchoosed(ISettingsSlot settingsSlot)
        {
            ISettingsSlotDropdown settingsSlotSlider = settingsSlot as ISettingsSlotDropdown;
            if (settingsSlotSlider == null || settingsSlotSlider != view) return;

            view.AnimateUnchoosed();
        }

    }
}