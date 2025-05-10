using System.Collections;
using System.Collections.Generic;
using Frontier_UI;
using Game_UI;
using UnityEngine;
using VContainer;

namespace Save_screen
{
    public interface ILoadPopUpObserver
    {
        void Enable();
        void Disable();
    }

    public class LoadPopUpObserver : ILoadPopUpObserver
    {
        [Inject] private ITextButtonMouseEvents textBtnMouse;

        [Inject] private ISaveManager saveManager;
        [Inject] private IPopUpsManager popUpsManager;
        private ITextPopUp view;

        private ISaveSlot choosedSlot;

        public LoadPopUpObserver(ITextPopUp view)
        {
            this.view = view;
        }

        public void Enable()
        {
            EnableAcceptBtn();
            EnableCancelBtn();

            popUpsManager.loadChangeState += CheckPopUpVisability;

            view.onCancel += DisablePopUp;
            view.onAccept += LoadSlot;
        }

        public void Disable()
        {
            DisableCancelBtn();
            DisableAcceptBtn();

            popUpsManager.loadChangeState -= CheckPopUpVisability;

            view.onCancel -= DisablePopUp;
            view.onAccept -= LoadSlot;
        }

        private void DisablePopUp()
        {
            popUpsManager.SetLoadVisibility(false, null);
            choosedSlot = null;
        }

        private void CheckPopUpVisability(bool isVisible, ISaveSlot slot)
        {
            if (isVisible)
            {
                if (choosedSlot == slot)
                    return;

                choosedSlot = slot;
                
                view.SetVisibility(true);
                view.AnimateOpen();
            }
            else
            {
                choosedSlot = null;
                view.onEndCloseAnim += SetNotVisible;
                view.AnimateClose();
            }
            
        }

        private void SetNotVisible()
        {
            view.SetVisibility(false);
            view.onEndCloseAnim -= SetNotVisible;
        }

        private void LoadSlot()
        {
            if(choosedSlot == null)
            {
                Debug.LogError("No choosed save slot");
                return;
            }

            saveManager.Load(choosedSlot.Info);
            view.SetVisibility(false);
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


