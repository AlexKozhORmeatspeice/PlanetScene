using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;
using VContainer.Unity;

namespace Space_Screen
{
    public interface IPlanetMouseEvents
    {
        event Action<IPlanet> OnMouseEnter;
        event Action<IPlanet> OnMouseMove;
        event Action<IPlanet> OnMouseLeave;
        event Action<IPlanet> OnMouseClick;
        event Action<IPlanet> OnMouseDoubleClick;
    }

    public class PlanetMouseEvents : MonoBehaviour, IPlanetMouseEvents, ITickable, IStartable, IDisposable
    {
        [Inject] private IPointerManager pointer;
        private bool isEntered;
        private IPlanet nowObj;

        public event Action<IPlanet> OnMouseEnter;
        public event Action<IPlanet> OnMouseLeave;
        public event Action<IPlanet> OnMouseClick;
        public event Action<IPlanet> OnMouseDoubleClick;
        public event Action<IPlanet> OnMouseMove;

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

            if (results.Count > 0 && results[0].gameObject.GetComponent<IPlanet>() != null)
            {
                IPlanet newPlanet = results[0].gameObject.GetComponent<IPlanet>();
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