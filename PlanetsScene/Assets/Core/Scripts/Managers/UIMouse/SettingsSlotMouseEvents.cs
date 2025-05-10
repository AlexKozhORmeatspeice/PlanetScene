using Planet_Window;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using VContainer;
using UnityEngine;
using VContainer.Unity;
using Settings_Screen;

namespace Frontier_UI
{
    public interface ISettingsSlotMouseEvents
    {
        event Action<ISettingsSlot> OnMouseEnter;
        event Action<ISettingsSlot> OnMouseMove;
        event Action<ISettingsSlot> OnMouseLeave;
        event Action<ISettingsSlot> OnMouseClick;
        event Action<ISettingsSlot> OnMouseDoubleClick;
    }

    public class SettingsSlotMouseEvents : ISettingsSlotMouseEvents, ITickable, IStartable
    {
        [Inject] private IPointerManager pointer;
        private ISettingsSlot hoveredSlot;
        private PointerEventData eventData;
        private List<RaycastResult> raycastList = new();

        public event Action<ISettingsSlot> OnMouseEnter;
        public event Action<ISettingsSlot> OnMouseLeave;
        public event Action<ISettingsSlot> OnMouseClick;
        public event Action<ISettingsSlot> OnMouseDoubleClick;
        public event Action<ISettingsSlot> OnMouseMove;

        public void Initialize()
        {
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

            ISettingsSlot newButton = null;
            foreach (RaycastResult result in raycastList)
            {
                if (result.gameObject.TryGetComponent(out newButton))
                {
                    break;
                }
            }

            if (newButton == null)
            {
                if (hoveredSlot != null)
                {
                    OnMouseLeave?.Invoke(hoveredSlot);
                    hoveredSlot = null;
                }
                return;
            }

            if (hoveredSlot != newButton)
            {
                if (hoveredSlot != null)
                {
                    OnMouseLeave?.Invoke(hoveredSlot);
                }

                hoveredSlot = newButton;
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
            if (hoveredSlot == null)
                return;

            OnMouseClick?.Invoke(hoveredSlot);
        }
        private void InvokeDoubleClick()
        {
            if (hoveredSlot == null)
                return;

            OnMouseDoubleClick?.Invoke(hoveredSlot);
        }

        public void Start()
        {
            //
        }
    }
}