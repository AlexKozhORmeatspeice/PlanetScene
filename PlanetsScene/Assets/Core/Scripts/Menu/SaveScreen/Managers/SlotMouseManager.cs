using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using UnityEngine;
using VContainer.Unity;
using VContainer;
using Planet_Window;

namespace Save_screen
{
    public interface ISlotMouseEvents
    {
        event Action<ISaveSlot> OnMouseEnter;
        event Action<ISaveSlot> OnMouseMove;
        event Action<ISaveSlot> OnMouseLeave;
        event Action<ISaveSlot> OnMouseClick;
        event Action<ISaveSlot> OnMouseDoubleClick;
    }

    public class SlotMouseEvents : ISlotMouseEvents, ITickable, IStartable, IDisposable
    {
        [Inject] private IPointerManager pointer;
        private ISaveSlot hoveredSlot;
        private PointerEventData eventData;
        private List<RaycastResult> raycastList = new();

        public event Action<ISaveSlot> OnMouseEnter;
        public event Action<ISaveSlot> OnMouseLeave;
        public event Action<ISaveSlot> OnMouseClick;
        public event Action<ISaveSlot> OnMouseDoubleClick;
        public event Action<ISaveSlot> OnMouseMove;

        private bool isDetectedButton = false;

        public void Initialize()
        {
            isDetectedButton = true;

            pointer.OnDoubleTap += InvokeDoubleClick;
            pointer.OnPointerDown += InvokeClick;

            eventData = new PointerEventData(EventSystem.current);
        }

        public void Tick()
        {
            raycastList.Clear();
            eventData.position = Input.mousePosition;
            EventSystem.current.RaycastAll(eventData, raycastList);

            if (raycastList.Count == 0)
            {
                if (hoveredSlot != null)
                {
                    OnMouseLeave?.Invoke(hoveredSlot);
                    hoveredSlot = null;
                }
                return;
            }

            ISaveSlot newSlot = null; //оставил только 1, чтобы не срабатывал инпут на экране сканера
            foreach (RaycastResult result in raycastList)
            {
                if(result.gameObject.GetComponent<ITextButton>() != null || result.gameObject.GetComponent<IIconButton>() != null)
                {
                    isDetectedButton = true;
                    return;
                }
                else
                {
                    isDetectedButton = false;
                }

                if (result.gameObject.TryGetComponent(out newSlot))
                {
                    break;
                }
            }
            
            if (newSlot == null)
            {
                if (hoveredSlot != null)
                {
                    OnMouseLeave?.Invoke(hoveredSlot);
                    hoveredSlot = null;
                }
                return;
            }

            if (hoveredSlot != newSlot)
            {
                if (hoveredSlot != null)
                {
                    OnMouseLeave?.Invoke(hoveredSlot);
                }

                hoveredSlot = newSlot;
                OnMouseEnter?.Invoke(hoveredSlot);
            }

            OnMouseMove?.Invoke(hoveredSlot);
        }
        public void Dispose()
        {
            pointer.OnDoubleTap -= InvokeDoubleClick;
            pointer.OnPointerDown -= InvokeClick;
        }

        private void InvokeClick()
        {
            if (hoveredSlot == null || isDetectedButton)
                return;

            OnMouseClick?.Invoke(hoveredSlot);
        }
        private void InvokeDoubleClick()
        {
            if (hoveredSlot == null || isDetectedButton)
                return;

            OnMouseDoubleClick?.Invoke(hoveredSlot);
        }

        public void Start() { }
    }
}
