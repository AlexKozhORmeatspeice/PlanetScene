using System;
using System.Collections.Generic;
using Space_Screen;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;
using VContainer.Unity;

namespace Planet_Window
{
    public interface IPOIMouseEvents
    {
        event Action<IPointOfInterest> OnMouseEnter;
        event Action<IPointOfInterest> OnMouseMove;
        event Action<IPointOfInterest> OnMouseLeave;
        event Action<IPointOfInterest> OnMouseClick;
        event Action<IPointOfInterest> OnMouseDoubleClick;
    }

    public class POIMouseEvents : MonoBehaviour, IPOIMouseEvents, ITickable, IStartable, IDisposable
    {
        [Inject] private IPointerManager pointer;
        private IPointOfInterest hoveredPOI;
        private PointerEventData eventData;
        private List<RaycastResult> raycastList = new();

        public event Action<IPointOfInterest> OnMouseEnter;
        public event Action<IPointOfInterest> OnMouseLeave;
        public event Action<IPointOfInterest> OnMouseClick;
        public event Action<IPointOfInterest> OnMouseDoubleClick;
        public event Action<IPointOfInterest> OnMouseMove;

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
                if (hoveredPOI != null)
                {
                    OnMouseLeave?.Invoke(hoveredPOI);
                    hoveredPOI = null;
                }
                return;
            }

            IPointOfInterest newPOI = null;
            foreach (RaycastResult result in raycastList)
            {
                if (result.gameObject.TryGetComponent(out newPOI))
                {
                    break;
                }
            }

            if (newPOI == null)
            {
                if (hoveredPOI != null)
                {
                    OnMouseLeave?.Invoke(hoveredPOI);
                    hoveredPOI = null;
                }
                return;
            }

            if (hoveredPOI != newPOI)
            {
                if (hoveredPOI != null)
                {
                    OnMouseLeave?.Invoke(hoveredPOI);
                }

                hoveredPOI = newPOI;
                OnMouseEnter?.Invoke(hoveredPOI);
            }

            OnMouseMove?.Invoke(hoveredPOI);
        }
        public void Dispose()
        {
            pointer.OnDoubleTap -= InvokeDoubleClick;
            pointer.OnPointerDown -= InvokeClick;
        }

        private void InvokeClick()
        {
            if (hoveredPOI == null)
                return;

            OnMouseClick?.Invoke(hoveredPOI);
        }
        private void InvokeDoubleClick()
        {
            if (hoveredPOI == null)
                return;

            OnMouseDoubleClick?.Invoke(hoveredPOI);
        }

        public void Start()
        {
            //
        }
    }
}