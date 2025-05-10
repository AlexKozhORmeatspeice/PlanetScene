

using UnityEngine;
using VContainer;

namespace Save_screen
{
    public interface ISaveSlotObserver
    {
        void Enable();
        void Disable();
    }

    public class SaveSlotObserver : ISaveSlotObserver //TOOD: ���� ��������� ��������������� ��� ��������� pop_up-��� ��������
    {
        [Inject] private ISlotMouseEvents slotMouse;

        [Inject] private ISaveSlotsManager slotSpawner;
        [Inject] private IPopUpsManager popUpsManager;

        private ISaveSlot view;
        private SaveInfo info;

        private bool isInterective;

        public SaveSlotObserver(ISaveSlot view, SaveInfo info)
        {
            this.view = view;
            this.info = info;

            isInterective = false;
        }

        public void Enable()
        {
            view.SetVisibility(true);
            view.AnimateCreation();

            view.AnimateNotChoosed(); //TODO: ������ �� Awake �� view ���������� ����� ��� ���� �����

            view.Info = info;
            
            popUpsManager.onActivatePopUp += SetNotInterective;
            popUpsManager.onDisablePopUp += SetInterective;

            slotSpawner.onDeleteSlot += CheckDestroy;

            SetInterective();
        }

        public void Disable()
        {
            popUpsManager.onActivatePopUp -= SetNotInterective;
            popUpsManager.onDisablePopUp -= SetInterective;

            slotSpawner.onDeleteSlot -= CheckDestroy;

            SetNotInterective();
        } 

        private void SetInterective()
        {
            if (isInterective)
                return;

            isInterective = true;
            slotMouse.OnMouseClick += CheckLoad;

            slotMouse.OnMouseEnter += AnimateChoosed;
            slotMouse.OnMouseLeave += AnimateNotChoosed;
        }

        private void SetNotInterective()
        {
            isInterective = false;
            slotMouse.OnMouseClick -= CheckLoad;

            slotMouse.OnMouseEnter -= AnimateChoosed;
            slotMouse.OnMouseLeave -= AnimateNotChoosed;

            view.AnimateNotChoosed();
        }

        private void CheckLoad(ISaveSlot slot)
        {
            if(slot != view) return;
            
            popUpsManager.SetLoadVisibility(true, slot);
        }

        private void CheckDestroy(SaveInfo info)
        {
            if (this.info.name != info.name)
                return;

            view.onEndAnimDelete += OnEndAnimDestroy;
            view.AnimateDelete();
        }

        private void OnEndAnimDestroy()
        {
            view.Destroy();
            Disable();
            view.onEndAnimDelete -= OnEndAnimDestroy;
        }

        private void AnimateChoosed(ISaveSlot slot)
        {
            if (slot != view) return;

            view.AnimateChoosed();
        }

        private void AnimateNotChoosed(ISaveSlot slot)
        {
            if (slot != view) return;

            view.AnimateNotChoosed();
        }
    }
}
