using System;
using System.Collections.Generic;
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
        private bool isEntered;
        private IPointOfInterest nowObj;

        public event Action<IPointOfInterest> OnMouseEnter;
        public event Action<IPointOfInterest> OnMouseLeave;
        public event Action<IPointOfInterest> OnMouseClick;
        public event Action<IPointOfInterest> OnMouseDoubleClick;
        public event Action<IPointOfInterest> OnMouseMove;

        public void Initialize()
        {
            isEntered = false;
            pointer.OnDoubleTap += InvokeDoubleClick;
            pointer.OnPointerDown += InvokeClick;
        }

        public void Tick()
        {
            var eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            if (results.Count > 0 && results[0].gameObject.GetComponent<IPointOfInterest>() != null)
            {
                IPointOfInterest newPlanet = results[0].gameObject.GetComponent<IPointOfInterest>();
                if (newPlanet == null)
                    return;

                if (nowObj == null && !isEntered)
                {
                    nowObj = newPlanet;
                    isEntered = true;

                    OnMouseEnter?.Invoke(nowObj);
                }

                OnMouseMove?.Invoke(nowObj);
            }
            else if (isEntered)
            {
                isEntered = false;
                nowObj = default;
                OnMouseLeave?.Invoke(nowObj);
            }
        }
        public void Dispose()
        {
            pointer.OnDoubleTap -= InvokeDoubleClick;
            pointer.OnPointerDown -= InvokeClick;
        }

        private void InvokeClick()
        {
            if (nowObj == null)
                return;

            OnMouseClick?.Invoke(nowObj);
        }
        private void InvokeDoubleClick()
        {
            if (nowObj == null)
                return;

            OnMouseDoubleClick?.Invoke(nowObj);
        }

        public void Start()
        {
            //
        }
    }
}