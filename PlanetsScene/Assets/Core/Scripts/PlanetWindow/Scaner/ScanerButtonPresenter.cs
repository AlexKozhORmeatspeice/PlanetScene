using VContainer;

namespace Planet_Window
{
    public interface IScanerButtonPresenter
    {
        void ChangeScanerStatus();
        void Enable();
        void Disable();
    }

    public class ScanerButtonPresenter : IScanerButtonPresenter
    {
        private IScaner scaner;
        private IPlanetWindow_ScanerButton view;

        public ScanerButtonPresenter(IPlanetWindow_ScanerButton view, IScaner scaner)
        {
            this.view = view;
            this.scaner = scaner;
        }
        public void Enable()
        {
            view.SetStatus(scaner.IsScanning);
            scaner.onChangeIsScanning += ChangeButtonStatus;
            view.onClick += ChangeScanerStatus;
        }

        public void Disable()
        {
            scaner.onChangeIsScanning -= ChangeButtonStatus;
            view.onClick -= ChangeScanerStatus;
        }

        public void ChangeScanerStatus()
        {
            scaner.SwapStatus();
        }

        private void ChangeButtonStatus(bool isScanning)
        {
            view.SetStatus(isScanning);
        }

    }

}