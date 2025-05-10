
using VContainer;

namespace Settings_Screen
{
    public interface ICloseSettingsButtonObserver
    {
        void Enable();
        void Disable();
    }


    public class CloseSettingsButtonObserver : ICloseSettingsButtonObserver
    {
        private ISettingsScreen settingsScreen;

        private IIconButton view;

        public CloseSettingsButtonObserver(IIconButton view, ISettingsScreen settingsScreen)
        {
            this.view = view;
            this.settingsScreen = settingsScreen;
        }

        public void Enable()
        {
            view.onClick += settingsScreen.Disable;
        }

        public void Disable()
        {
            view.onClick -= settingsScreen.Disable;
        }
    }
}

