

using Save_screen;
using UnityEngine;
using VContainer;

namespace Save_screen
{
    public class NewGameSaveSlotObserver : ISaveSlotObserver
    {
        [Inject] private INewSlotMouseEvents slotMouse;
        [Inject] private IPopUpsManager popUpsManager;

        private INewSaveSlot view;

        private bool isInterective;
        public NewGameSaveSlotObserver(INewSaveSlot view)
        {
            this.view = view;
            isInterective = false;
        }

        public void Enable()
        {
            popUpsManager.onActivatePopUp += SetNotInterective;
            popUpsManager.onDisablePopUp += SetInterective;
            
            view.SetVisibility(true);
            view.AnimateNotChoosed();

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
            if (isInterective)
                return;
            isInterective = true;

            slotMouse.OnMouseEnter += AnimateChoosed;
            slotMouse.OnMouseLeave += AnimateNotChoosed;
        }

        private void SetNotInterective()
        {
            isInterective = false;
            slotMouse.OnMouseEnter -= AnimateChoosed;
            slotMouse.OnMouseLeave -= AnimateNotChoosed;

            view.AnimateNotChoosed();
        }

        private void AnimateChoosed(INewSaveSlot slot)
        {
            if (slot != view) return;

            view.AnimateChoosed();
        }

        private void AnimateNotChoosed(INewSaveSlot slot)
        {
            if (slot != view) return;

            view.AnimateNotChoosed();
        }

    }
}
