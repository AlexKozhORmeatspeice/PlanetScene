using System.Collections;
using System.Collections.Generic;
using Frontier_UI;
using Game_UI;
using UnityEngine;
using VContainer;

namespace Save_screen
{
    public interface INewSavePopUpObserver
    {
        void Enable();
        void Disable();
    }

    public class NewSavePopUpObserver : INewSavePopUpObserver //TODO: если имя сейва не задано или уже занято - кнопка создания не должна быть активна
    {
        [Inject] private ITextButtonMouseEvents textBtnMouse;
        [Inject] private IPopUpsManager popUpsManager;

        private ITextInputPopUp view;

        private ISaveSlot choosedSlot;

        public NewSavePopUpObserver(ITextInputPopUp view)
        {
            this.view = view;
        }

        public void Enable()
        {
            view.InputText = "";

            EnableAcceptBtn();
            EnableCancelBtn();

            popUpsManager.newSaveChangeState += CheckPopUpState;

            view.onCancel += DisablePopUp;
            view.onAccept += CreateNewSave;
        }

        public void Disable()
        {
            DisableAcceptBtn();
            DisableCancelBtn();

            popUpsManager.newSaveChangeState -= CheckPopUpState;

            view.onCancel -= DisablePopUp;
            view.onAccept -= CreateNewSave;
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

        private void CheckPopUpState(bool isVisible, ISaveSlot slot)
        {
            choosedSlot = slot;

            if(isVisible)
            {
                view.SetVisibility(true);
                view.AnimateOpen();
            }
            else
            {
                view.onEndCloseAnim += SetNotVisible;
                view.AnimateClose();
            }
        }

        private void SetNotVisible()
        {
            view.SetVisibility(false);
            view.onEndCloseAnim -= SetNotVisible;
        }

        private void DisablePopUp()
        {
            popUpsManager.SetNewSaveVisibility(false, null);
        }

        private void CreateNewSave()
        {
            popUpsManager.ClickSaveButton(view.InputText);

            if(choosedSlot != null) 
                popUpsManager.ClickDeleteButton(choosedSlot.Info);

            popUpsManager.SetNewSaveVisibility(false, null);
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



