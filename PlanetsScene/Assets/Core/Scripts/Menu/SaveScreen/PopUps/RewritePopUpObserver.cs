

using System.Diagnostics;
using Frontier_UI;
using Game_UI;
using VContainer;

namespace Save_screen
{
    public interface IRewritePopUpObserver
    {
        void Enable();
        void Disable();
    }

    public class RewritePopUpObserver : IRewritePopUpObserver
    {
        [Inject] private ITextButtonMouseEvents textBtnMouse;
        [Inject] private IPopUpsManager popUpsManager;

        private ITextPopUp view;

        private ISaveSlot choosedSlot;

        public RewritePopUpObserver(ITextPopUp view)
        {
            this.view = view;
        }

        public void Enable()
        {
            EnableAcceptBtn();
            EnableCancelBtn();

            popUpsManager.rewriteChangeState += SetVisibility;

            view.onCancel += SetNotVisibleState;
            view.onAccept += EnableNewSave;
        }    

        public void Disable()
        {
            DisableAcceptBtn();
            DisableCancelBtn();

            popUpsManager.rewriteChangeState -= SetVisibility;

            view.onCancel -= SetNotVisibleState;
            view.onAccept -= EnableNewSave;
        }

        private void SetVisibility(bool isVisible, ISaveSlot slot)
        {
            choosedSlot = slot;

            if(isVisible)
            {
                view.SetVisibility(true);
                view.AnimateOpen();
            }
            else
            {
                view.onEndCloseAnim += DisablePopUp;
                view.AnimateClose();
            }
        }

        private void SetNotVisibleState()
        {
            popUpsManager.SetRewriteVisibility(false, null);
        }

        private void DisablePopUp()
        {
            view.onEndCloseAnim -= DisablePopUp;
            view.SetVisibility(false);
        }

        private void EnableNewSave()
        {
            view.SetVisibility(false);
            if (choosedSlot == null)
                return;


            popUpsManager.SetNewSaveVisibility(true, choosedSlot);
            
            choosedSlot = null;
        }

        private void EnableCancelBtn()
        {
            textBtnMouse.OnMouseEnter += AnimateHoverCancel;
            textBtnMouse.OnMouseLeave += AnimateBaseCancel;
            textBtnMouse.OnMouseClick += AnimatePressCancel;
        }

        private void DisableCancelBtn()
        {
            textBtnMouse.OnMouseEnter -= AnimateHoverCancel;
            textBtnMouse.OnMouseLeave -= AnimateBaseCancel;
            textBtnMouse.OnMouseClick -= AnimatePressCancel;
        }

        private void EnableAcceptBtn()
        {
            textBtnMouse.OnMouseEnter += AnimateHoverAccept;
            textBtnMouse.OnMouseLeave += AnimateBaseAccept;
            textBtnMouse.OnMouseClick += AnimatePressAccept;
        }

        private void DisableAcceptBtn()
        {
            textBtnMouse.OnMouseEnter -= AnimateHoverAccept;
            textBtnMouse.OnMouseLeave -= AnimateBaseAccept;
            textBtnMouse.OnMouseClick -= AnimatePressAccept;
        }

        private void AnimateHoverCancel(ITextButton textBtn)
        {
            if (textBtn != view.CancelBtnView) return;

            view.AnimateHoverCancel();
        }

        private void AnimateBaseCancel(ITextButton textBtn)
        {
            if (textBtn != view.CancelBtnView) return;

            view.AnimateBaseCancel();
        }

        private void AnimatePressCancel(ITextButton textBtn)
        {
            if (textBtn != view.CancelBtnView) return;

            view.AnimatePressCancel();
        }

        private void AnimateHoverAccept(ITextButton textBtn)
        {
            if (textBtn != view.AcceptBtnView) return;

            view.AnimateHoverAccept();
        }

        private void AnimateBaseAccept(ITextButton textBtn)
        {
            if (textBtn != view.AcceptBtnView) return;

            view.AnimateBaseAccept();
        }

        private void AnimatePressAccept(ITextButton textBtn)
        {
            if (textBtn != view.AcceptBtnView) return;

            view.AnimatePressAccept();
        }
    }
}
