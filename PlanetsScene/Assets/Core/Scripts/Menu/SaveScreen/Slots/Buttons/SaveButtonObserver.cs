using Frontier_UI;
using VContainer;

namespace Save_screen
{
    public interface ISaveButtonObserver
    {
        void Enable();
        void Disable();
    }
    public class SaveButtonObserver : ISaveButtonObserver
    {
        [Inject] private ITextButtonMouseEvents textBtnMouse;
        [Inject] private IPopUpsManager popUpsManager;

        private ITextButton view;

        public SaveButtonObserver(ITextButton view)
        {
            this.view = view;
        }

        public void Enable()
        {
            popUpsManager.onActivatePopUp += SetNotInterective;
            popUpsManager.onDisablePopUp += SetInterective;

            SetInterective();
        }

        public void Disable()
        {
            popUpsManager.onActivatePopUp -= SetNotInterective;
            popUpsManager.onDisablePopUp -= SetInterective;

            SetNotInterective();
        }

        private void SetInterective()
        {
            textBtnMouse.OnMouseEnter += AnimateHover;
            textBtnMouse.OnMouseLeave += AnimateBase;
            textBtnMouse.OnMouseClick += AnimatePress;

            view.onClick += EnablePopUp;
        }

        private void SetNotInterective()
        {
            textBtnMouse.OnMouseEnter -= AnimateHover;
            textBtnMouse.OnMouseLeave -= AnimateBase;
            textBtnMouse.OnMouseClick -= AnimatePress;

            view.onClick -= EnablePopUp;
        }

        private void EnablePopUp()
        {
            popUpsManager.SetNewSaveVisibility(true, null);
        }

        private void AnimateHover(ITextButton textBtn)
        {
            if (textBtn != view) return;

            view.AnimateHover();
        }

        private void AnimateBase(ITextButton textBtn)
        {
            if (textBtn != view) return;

            view.AnimateBaseState();
        }

        private void AnimatePress(ITextButton textBtn)
        {
            if (textBtn != view) return;

            view.AnimatePressed();
        }
    }
}
