using System.Collections;
using System.Collections.Generic;
using Frontier_UI;
using UnityEngine;
using VContainer;

namespace Save_screen
{
    public interface IRewriteButtonObserver
    {
        void Enable();
        void Disable();
    }
    public class RewriteButtonObserver : IRewriteButtonObserver
    {
        [Inject] private ITextButtonMouseEvents textBtnMouse;
        [Inject] private ISlotMouseEvents slotMouse;
        [Inject] private IPopUpsManager popUpsManager;

        private ITextButton view;
        private ISaveSlot slot;

        private bool isInterective;

        public RewriteButtonObserver(ITextButton view, ISaveSlot slot)
        {
            isInterective = false;
            this.view = view;
            this.slot = slot;
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
            if (isInterective)
                return;

            isInterective = true;

            textBtnMouse.OnMouseEnter += AnimateHover;
            textBtnMouse.OnMouseLeave += AnimateBase;
            textBtnMouse.OnMouseClick += AnimatePress;

            view.onClick += CheckSlot;
        }

        private void SetNotInterective()
        {
            isInterective = false;

            textBtnMouse.OnMouseEnter -= AnimateHover;
            textBtnMouse.OnMouseLeave -= AnimateBase;
            textBtnMouse.OnMouseClick -= AnimatePress;

            view.onClick -= CheckSlot;
        }

        private void CheckSlot()
        {
            if (slot == null)
                return;

            popUpsManager.SetRewriteVisibility(true, slot);
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

        private void AnimateShow(ISaveSlot saveSlot)
        { 
            if(saveSlot != slot) return;

            view.AnimateShow();
        }

        private void AnimateHide(ISaveSlot saveSlot)
        {
            if (saveSlot != slot) return;

            view.AnimateHide();
        }
    }

}

