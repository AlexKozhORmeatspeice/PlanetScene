using System;
using VContainer;
using VContainer.Unity;

namespace Save_screen
{
    public interface IPopUpsManager
    {
        event Action onActivatePopUp;
        event Action onDisablePopUp;

        event Action<bool, ISaveSlot> rewriteChangeState;
        event Action<bool, ISaveSlot> loadChangeState;
        event Action<bool, ISaveSlot> newSaveChangeState;
        event Action<bool, ISaveSlot> deleteChangeState;

        event Action<string> onClickSaveButton;
        event Action<SaveInfo> onClickDeleteButton;

        void SetRewriteVisibility(bool isVisible, ISaveSlot saveSlot);
        void SetLoadVisibility(bool isVisible, ISaveSlot saveSlot);
        void SetDeleteVisibility(bool isVisible, ISaveSlot slot);
        void SetNewSaveVisibility(bool isVisible, ISaveSlot slot);

        void ClickSaveButton(string name);
        void ClickDeleteButton(SaveInfo deleteInfo);
    }

    public class SavePopUpsManager : IPopUpsManager
    {
        [Inject] private ISlotMouseEvents slotMouse;

        public event Action<bool, ISaveSlot> loadChangeState;
        public event Action<bool, ISaveSlot> newSaveChangeState;
        public event Action<bool, ISaveSlot> deleteChangeState;
        public event Action<bool, ISaveSlot> rewriteChangeState;

        public event Action<string> onClickSaveButton;
        public event Action<SaveInfo> onClickDeleteButton;
        public event Action onActivatePopUp;
        public event Action onDisablePopUp;

        public void SetLoadVisibility(bool isVisible, ISaveSlot saveSlot)
        {
            if (isVisible)
            {
                onActivatePopUp?.Invoke();
            }
            else
            {
                onDisablePopUp?.Invoke();
            }
            
            loadChangeState?.Invoke(isVisible, saveSlot);
        }

        public void SetNewSaveVisibility(bool isVisible, ISaveSlot slot)
        {
            if (isVisible)
            {
                onActivatePopUp?.Invoke();
            }
            else
            {
                onDisablePopUp?.Invoke();
            }

            newSaveChangeState?.Invoke(isVisible, slot);
        }

        public void SetRewriteVisibility(bool isVisible, ISaveSlot saveSlot)
        {
            if (isVisible)
            {
                onActivatePopUp?.Invoke();
            }
            else
            {
                onDisablePopUp?.Invoke();
            }

            rewriteChangeState?.Invoke(isVisible, saveSlot);
        }

        public void SetDeleteVisibility(bool isVisible, ISaveSlot slot)
        {
            if (isVisible)
            {
                onActivatePopUp?.Invoke();
            }
            else
            {
                onDisablePopUp?.Invoke();
            }

            deleteChangeState?.Invoke(isVisible, slot);
        }

        public void Start() { }

        public void ClickSaveButton(string name)
        {
            onClickSaveButton?.Invoke(name);
        }

        public void ClickDeleteButton(SaveInfo deleteInfo)
        {
            onClickDeleteButton?.Invoke(deleteInfo);
        }
    }
}