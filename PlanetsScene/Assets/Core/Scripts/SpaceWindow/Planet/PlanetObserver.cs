using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;
using Planet_Window;

namespace Space_Screen
{
    public interface IPlanetObserver
    {
        void Enable();
        void Disable();
    }

    public class PlanetObserver : IPlanetObserver
    {
        [Inject] private IPointerManager pointer;
        [Inject] private IPlanetWindow planetWindow;
        [Inject] private ITravelManager travelManager;

        private IPlanet view;
        private bool isChoosed;

        public PlanetObserver(IPlanet planet)
        {
            this.view = planet;
        }

        public void Enable()
        {
            SetInterective();

            planetWindow.onEnable += SetNotInterective;
            planetWindow.onDisable += SetInterective;

            travelManager.onTravelToPlanet += CheckChoosedPlanet;
        }

        public void Disable()
        {
            SetNotInterective();

            planetWindow.onEnable -= SetNotInterective;
            planetWindow.onDisable -= SetInterective;

            travelManager.onTravelToPlanet -= CheckChoosedPlanet;
        }

        private void CheckChoosedPlanet(IPlanet choosedPlanet)
        {
            if (view == choosedPlanet && !isChoosed)
            {
                EnableAsChoosedPlanet();
            }
            else if (view != choosedPlanet && isChoosed)
            {
                DisableAsChoosedPlanet();
            }
        }

        private void EnableAsChoosedPlanet()
        {
            SetNotInterective();

            planetWindow.onEnable -= SetNotInterective;
            planetWindow.onDisable -= SetInterective;

            view.ChangeScale(true);
            isChoosed = true;
        }

        private void DisableAsChoosedPlanet()
        {
            SetInterective();

            planetWindow.onEnable += SetNotInterective;
            planetWindow.onDisable += SetInterective;

            view.ChangeScale(false);
            isChoosed = false;
        }

        public void SetInterective()
        {
            pointer.OnPointerMove += ChangeScale;
        }

        public void SetNotInterective()
        {
            pointer.OnPointerMove -= ChangeScale;
        }
        private void ChangeScale()
        {
            float outerPlanetRadius = GetOuterDist();
            float dist = GetMouseToObjDist();
            float alpha = 1.0f - (dist - outerPlanetRadius * 1.1f) / (outerPlanetRadius * 0.5f); //немного математики, чтобы размер не уменьшался, если мы в внутреннем радиусе

            view.ChangeScale(alpha);
        }

        private float GetMouseToObjDist()
        {
            Vector3 objPos = (Vector2)(view.Position);
            Vector3 cursorPos = pointer.NowWorldPosition;

            //данная формула задает расстояние по квадрату
            return Mathf.Max(Mathf.Abs(objPos.x - cursorPos.x), Mathf.Abs(objPos.y - cursorPos.y)); 
        }

        private float GetOuterDist()
        {
            return view.Size.magnitude; 
        }
    }

}