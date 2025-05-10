namespace Settings_Screen
{
    public interface ISettingsObserver
    {
        void Enable();
        void Disable();
    }

    public class SettingsObserver : ISettingsObserver
    {
        private ISettingsView view;

        public SettingsObserver(ISettingsView view)
        {
            this.view = view;
        }

        public void Enable()
        {
            view.SetVisibility(true);
            view.AnimateShow();
        }

        public void Disable()
        {
            view.onEndCloseAnim += DisableView;
            view.AnimateHide();
        }

        private void DisableView()
        {
            view.SetVisibility(false);
            view.onEndCloseAnim -= DisableView;
        }
    }
}