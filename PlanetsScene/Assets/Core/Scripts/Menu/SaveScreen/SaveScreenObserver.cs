using System.Diagnostics;
using VContainer;

namespace Save_screen
{
    public interface ISaveScreenObserver
    {
        void Enable();
        void Disable();
    }

    public class SaveScreenObserver : ISaveScreenObserver
    {
        private ISaveScreenView view;

        public SaveScreenObserver(ISaveScreenView view)
        {
            this.view = view;
        }

        public void Enable()
        {
            view.SetVisibility(true);
            view.AnimateEnable();
        }

        public void Disable()
        {
            view.onEndCloseAnim += DisableView;
            view.AnimateDisable();
        }

        private void DisableView()
        {
            view.SetVisibility(false);
            view.onEndCloseAnim -= DisableView;
        }
    }
}