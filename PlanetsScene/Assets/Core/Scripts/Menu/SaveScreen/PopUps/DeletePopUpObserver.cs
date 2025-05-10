using System.Collections;
using System.Collections.Generic;
using Frontier_UI;
using Game_UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using VContainer;

namespace Save_screen
{
    public interface IDeletePopUpObserver
    {
        void Enable();
        void Disable();
    }

    public class DeletePopUpObserver : IDeletePopUpObserver
    {
        [Inject] private ITextButtonMouseEvents textBtnMouse;
        [Inject] private IPopUpsManager popUpsManager;
        
        private ITextPopUp view;

        private ISaveSlot choosedSaveSlot;

        public DeletePopUpObserver(ITextPopUp view)
        {
            this.view = view;
        }

        public void Enable()
        {
            EnableAcceptBtn();
            EnableCancelBtn();

            popUpsManager.deleteChangeState += OnChangePopUpVisability;
            
            view.onCancel += DisablePopUp;
            view.onAccept += DeleteSlot;
        }

        public void Disable()
        {
            DisableAcceptBtn();
            DisableCancelBtn();

            popUpsManager.deleteChangeState -= OnChangePopUpVisability;

            view.onCancel -= DisablePopUp;
            view.onAccept -= DeleteSlot;
        }

        private void OnChangePopUpVisability(bool isVisible, ISaveSlot slot)
        {
            choosedSaveSlot = slot;
            
            if(isVisible)
            {
                view.SetVisibility(true);
                view.AnimateOpen();
            }
            else
            {
                view.onEndCloseAnim += PopUpSetNotVisible;
                view.AnimateClose();
            }
        }

        private void PopUpSetNotVisible()
        {
            view.SetVisibility(false);
            view.onEndCloseAnim -= PopUpSetNotVisible;
        }

        private void DisablePopUp()
        {
            popUpsManager.SetDeleteVisibility(false, choosedSaveSlot);
            choosedSaveSlot = null;
        }

        private void DeleteSlot()
        {
            popUpsManager.SetDeleteVisibility(false, choosedSaveSlot);

            if (choosedSaveSlot == null)
            {
                Debug.LogError("No choosed SaveSlot in PopUp");
                return;
            }

            popUpsManager.ClickDeleteButton(choosedSaveSlot.Info);
            choosedSaveSlot = null;
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

