using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Planet_Window
{
    public interface INotFoundObserver
    {
        void Enable();
        void Disable();
    }

    public class NotFoundObserver : MonoBehaviour, INotFoundObserver
    {
        [Inject] private IDrone drone;
        private IPlanetWindow_NotFound view;

        public NotFoundObserver(IPlanetWindow_NotFound view)
        {
            this.view = view;
        }

        public void Enable()
        {
            drone.onLand += Activate;
        }

        public void Disable()
        {
            drone.onLand -= Activate;
        }

        private void Activate(Vector3 pos, IPointOfInterest poi)
        {
            if (poi != null)
                return;

            view.Pos = pos;
            view.PlayAnim();
        }
    }

}