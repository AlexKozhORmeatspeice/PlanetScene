using System.Diagnostics;
using Frontier_UI;
using UnityEditor;
using VContainer;

namespace Save_screen
{
    public interface IDeleteButtonObserver
    {
        void Enable();
        void Disable();
    }
    public class DeleteButtonObserver : IDeleteButtonObserver
    {
        [Inject] private IIconButtonMouseEvents iconMouse;
        [Inject] private ISlotMouseEvents slotMouse;
        [Inject] private IPopUpsManager popUpsManager;
        
        private IIconButton view;
        private ISaveSlot slot;

        private bool isInterective;
        public DeleteButtonObserver(IIconButton view, ISaveSlot slot)
        {
            this.view = view;
            this.slot = slot;

            isInterective = false;
        }

        public void Enable()
        {
            SetInterective();
            
            popUpsManager.onActivatePopUp += SetNotInterective;
            popUpsManager.onDisablePopUp += SetInterective;

            slotMouse.OnMouseEnter += CheckInSlot;
            slotMouse.OnMouseLeave += CheckOutSlot;
        }

        public void Disable()
        {
            SetNotInterective();
            
            popUpsManager.onActivatePopUp -= SetNotInterective;
            popUpsManager.onDisablePopUp -= SetInterective;

            slotMouse.OnMouseEnter -= CheckInSlot;
            slotMouse.OnMouseLeave -= CheckOutSlot;
        }

        private void SetInterective()
        {
            if (isInterective) return;
            isInterective = true;

            iconMouse.OnMouseEnter += AnimateHover;
            iconMouse.OnMouseLeave += AnimateBase;
            iconMouse.OnMouseClick += AnimatePress;

            view.onClick += EnablePopUp;
        }

        private void SetNotInterective()
        {
            isInterective = false;

            iconMouse.OnMouseEnter -= AnimateHover;
            iconMouse.OnMouseLeave -= AnimateBase;
            iconMouse.OnMouseClick -= AnimatePress;
         
            view.onClick -= EnablePopUp;
        }

        private void CheckInSlot(ISaveSlot slot)
        {
            if (this.slot != slot) return;

            SetInterective();
            view.AnimateShow();
        }

        private void CheckOutSlot(ISaveSlot slot)
        {
            if (this.slot != slot) return;
            
            SetNotInterective();
            view.AnimateHide();
        }

        private void EnablePopUp()
        {
            popUpsManager.SetDeleteVisibility(true, slot);
        }

        private void AnimateHover(IIconButton iconButton)
        {
            if (iconButton != view) return;

            view.AnimateHover();
        }

        private void AnimateBase(IIconButton iconButton)
        {
            if (iconButton != view) return;
            
            view.AnimateBaseState();
        }

        private void AnimatePress(IIconButton iconButton)
        {
            if (iconButton != view) return;

            view.AnimatePressed();
        }
    }
}
