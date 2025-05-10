using Planet_Window;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using VContainer;
using UnityEngine;
using VContainer.Unity;

namespace Frontier_UI
{
    public interface IIconButtonMouseEvents
    {
        event Action<IIconButton> OnMouseEnter;
        event Action<IIconButton> OnMouseMove;
        event Action<IIconButton> OnMouseLeave;
        event Action<IIconButton> OnMouseClick;
        event Action<IIconButton> OnMouseDoubleClick;
    }

    public class IconButtonMouseEvents : IIconButtonMouseEvents, ITickable, IStartable
    {
        [Inject] private IPointerManager pointer;
        private IIconButton hoveredButton;
        private PointerEventData eventData;
        private List<RaycastResult> raycastList = new();

        public event Action<IIconButton> OnMouseEnter;
        public event Action<IIconButton> OnMouseLeave;
        public event Action<IIconButton> OnMouseClick;
        public event Action<IIconButton> OnMouseDoubleClick;
        public event Action<IIconButton> OnMouseMove;

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
                if (hoveredButton != null)
                {
                    OnMouseLeave?.Invoke(hoveredButton);
                    hoveredButton = null;
                }
                return;
            }

            IIconButton newButton = null;
            foreach (RaycastResult result in raycastList)
            {
                if (result.gameObject.TryGetComponent(out newButton))
                {
                    break;
                }
            }

            if (newButton == null)
            {
                if (hoveredButton != null)
                {
                    OnMouseLeave?.Invoke(hoveredButton);
                    hoveredButton = null;
                }
                return;
            }

            if (hoveredButton != newButton)
            {
                if (hoveredButton != null)
                {
                    OnMouseLeave?.Invoke(hoveredButton);
                }

                hoveredButton = newButton;
                OnMouseEnter?.Invoke(hoveredButton);
            }

            OnMouseMove?.Invoke(hoveredButton);
        }

        public void Dispose()
        {
            pointer.OnDoubleTap -= InvokeDoubleClick;
            pointer.OnPointerDown -= InvokeClick;
        }

        private void InvokeClick()
        {
            if (hoveredButton == null)
                return;

            OnMouseClick?.Invoke(hoveredButton);
        }
        private void InvokeDoubleClick()
        {
            if (hoveredButton == null)
                return;

            OnMouseDoubleClick?.Invoke(hoveredButton);
        }

        public void Start()
        {
            //
        }
    }
}