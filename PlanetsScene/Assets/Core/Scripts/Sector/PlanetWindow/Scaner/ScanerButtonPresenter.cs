using Frontier_UI;
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
        [Inject] private ITextButtonMouseEvents textBtnMouse;

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

            textBtnMouse.OnMouseEnter += AnimateHover;
            textBtnMouse.OnMouseLeave += AnimateBase;
            textBtnMouse.OnMouseClick += AnimatePress;
        }

        public void Disable()
        {
            scaner.onChangeIsScanning -= ChangeButtonStatus;
            view.onClick -= ChangeScanerStatus;

            textBtnMouse.OnMouseEnter -= AnimateHover;
            textBtnMouse.OnMouseLeave -= AnimateBase;
            textBtnMouse.OnMouseClick -= AnimatePress;
        }

        public void ChangeScanerStatus()
        {
            scaner.SwapStatus();
        }

        private void ChangeButtonStatus(bool isScanning)
        {
            view.SetStatus(isScanning);
        }

        private void AnimateHover(ITextButton txtBtn)
        {
            if (txtBtn != view as ITextButton) return;

            view.AnimateHover();
        }

        private void AnimateBase(ITextButton txtBtn)
        {
            if (txtBtn != view as ITextButton) return;

            view.AnimateBaseState();
        }

        private void AnimatePress(ITextButton txtBtn)
        {
            if (txtBtn != view as ITextButton) return;

            view.AnimatePressed();
        }
    }
}