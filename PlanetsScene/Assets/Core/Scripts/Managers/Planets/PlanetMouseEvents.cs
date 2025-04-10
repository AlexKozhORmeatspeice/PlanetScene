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
        private IPlanet hoveredPlanet;
        private PointerEventData eventData;
        private List<RaycastResult> raycastList = new();

        public event Action<IPlanet> OnMouseEnter;
        public event Action<IPlanet> OnMouseLeave;
        public event Action<IPlanet> OnMouseClick;
        public event Action<IPlanet> OnMouseDoubleClick;
        public event Action<IPlanet> OnMouseMove;

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

            if(raycastList.Count == 0)
            {
                if(hoveredPlanet != null)
                {
                    OnMouseLeave?.Invoke(hoveredPlanet);
                    hoveredPlanet = null;
                }
                return;
            }

            IPlanet newPlanet = null;
            foreach(RaycastResult result in raycastList)
            {
                if(result.gameObject.TryGetComponent(out newPlanet))
                {
                    break;
                }
            }

            if(newPlanet == null)
            {
                if (hoveredPlanet != null)
                {
                    OnMouseLeave?.Invoke(hoveredPlanet);
                    hoveredPlanet = null;
                }
                return;
            }

            if (hoveredPlanet != newPlanet)
            {
                if(hoveredPlanet != null)
                {
                    OnMouseLeave?.Invoke(hoveredPlanet);
                }

                hoveredPlanet = newPlanet;
                OnMouseEnter?.Invoke(hoveredPlanet);
            }

            OnMouseMove?.Invoke(hoveredPlanet);
        }
        public void Dispose()
        {
            pointer.OnDoubleTap -= InvokeDoubleClick;
            pointer.OnPointerDown -= InvokeClick;
        }

        private void InvokeClick()
        {
            if (hoveredPlanet == null)
                return;

            OnMouseClick?.Invoke(hoveredPlanet);
        }
        private void InvokeDoubleClick()
        {
            if (hoveredPlanet == null)
                return;

            OnMouseDoubleClick?.Invoke(hoveredPlanet);
        }

        public void Start()
        {
            //
        }
    }
}