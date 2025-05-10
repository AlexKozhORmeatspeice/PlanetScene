using Planet_Window;
using Space_Screen;
using VContainer;


namespace Top_Bar
{
    internal interface ITopPart_ButtonsObserver
    {
        void Enable();
        void Disable();
    }

    public class TopPart_ButtonsObserver : ITopPart_ButtonsObserver
    {
        [Inject] private IScaner scaner;
        private ITopBar topBar;

        public TopPart_ButtonsObserver(ITopBar topBar)
        {
            this.topBar = topBar;
        }

        public void Enable()
        {
            scaner.onScanerEnable += SetButtonsInvisible;
            scaner.onScanerDisable += SetButtonsVisible;
        }

        public void Disable()
        {
            scaner.onScanerEnable -= SetButtonsInvisible;
            scaner.onScanerDisable -= SetButtonsVisible;
        }

        private void SetButtonsVisible()
        {
            topBar.SetButtonsVisability(true);
        }

        private void SetButtonsInvisible()
        {
            topBar.SetButtonsVisability(false);
        }
    }
}