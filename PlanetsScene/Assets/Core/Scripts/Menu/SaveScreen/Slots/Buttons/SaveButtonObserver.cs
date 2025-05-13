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
        [Inject] private INewSlotMouseEvents slotMouse;
        [Inject] private ITextButtonMouseEvents textBtnMouse;
        [Inject] private IPopUpsManager popUpsManager;

        private ITextButton view;
        private INewSaveSlot newSaveSlot;

        public SaveButtonObserver(ITextButton view, INewSaveSlot slot)
        {
            this.view = view;
            newSaveSlot = slot;
        }

        public void Enable()
        {
            popUpsManager.onActivatePopUp += SetNotInterective;
            popUpsManager.onDisablePopUp += SetInterective;

            slotMouse.OnMouseEnter += AnimateShow;
            slotMouse.OnMouseLeave += AnimateHide;

            SetInterective();
        }

        public void Disable()
        {
            popUpsManager.onActivatePopUp -= SetNotInterective;
            popUpsManager.onDisablePopUp -= SetInterective;

            slotMouse.OnMouseEnter -= AnimateShow;
            slotMouse.OnMouseLeave -= AnimateHide;

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

        private void AnimateHide(INewSaveSlot slot)
        {
            if (slot != newSaveSlot) return;

            view.AnimateHide();
        }

        private void AnimateShow(INewSaveSlot slot)
        {
            if (slot != newSaveSlot) return;

            view.AnimateShow();
        }
    }
}
