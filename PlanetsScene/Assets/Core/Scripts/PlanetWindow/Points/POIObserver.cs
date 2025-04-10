using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Space_Screen;
using UnityEngine;
using VContainer;

namespace Planet_Window
{
    public interface IPOIObserver
    {
        void Enable();
        void Disable();
    }

    public class POIObserver : IPOIObserver
    {
        [Inject] private ITravelManager travelManager;
        [Inject] private IPlanetWindow planetWindow;
        [Inject] private IDrone drone;
        [Inject] private IScaner scaner;
        [Inject] private IPointerManager pointer;
        private IPointsSpawner spawner;

        private IPointOfInterest view;
        private IPlanet poiPlanet;  
        private bool isInterective;

        public POIObserver(IPointOfInterest poi, IPointsSpawner spawner)
        {
            this.view = poi;
            poiPlanet = view.GetInfo.planet;

            this.spawner = spawner;

            view.SetPosition(spawner.GetRandomPositionInCircle());
        }

        public void Enable()
        {
            SetInterective();
            
            drone.onLand += SetPOIActive;
            scaner.onChangeIsScanning += SetIsNotInterective;

            planetWindow.onEnable += SetNotInterective;
            planetWindow.onDisable += SetInterective;

            scaner.onScanerEnable += SetNotInterective;
            scaner.onScanerDisable += SetInterective;
            scaner.onChangeIsScanning += CheckScanerDetection;

            planetWindow.onPlanetEnable += ShowPlanetPOI;
            planetWindow.onDisable += view.Hide;
        }

        public void Disable()
        {
            SetNotInterective();
            
            drone.onLand -= SetPOIActive;
            scaner.onChangeIsScanning -= SetIsNotInterective;

            planetWindow.onEnable -= SetNotInterective;
            planetWindow.onDisable -= SetInterective;

            scaner.onScanerEnable -= SetNotInterective;
            scaner.onScanerDisable -= SetInterective;
            scaner.onChangeIsScanning -= CheckScanerDetection;

            planetWindow.onEnable += view.Show;
            planetWindow.onDisable += view.Hide;
        }

        private void ShowPlanetPOI(IPlanet planet)
        {
            if (poiPlanet != planet || !view.GetInfo.isVisible)
                return;

            view.Show();
        }

        private void SetPOIActive(Vector3 pos, IPointOfInterest poi)
        {
            if (this.view != poi)
                return;

            view.SetVisibility(true);
            view.PlayActivationAnim();
            SetInterective();
        }

        private void SetIsNotInterective(bool isNotInterective)
        {
            this.isInterective = !isNotInterective;
            if (isInterective)
            {
                SetInterective();
            }
            else
            {
                SetNotInterective();
            }
        }

        private void SetInterective()
        {
            pointer.OnPointerMove += ChangeScale;
        }

        private void SetNotInterective()
        {
            pointer.OnPointerMove -= ChangeScale;
        }

        private void CheckScanerDetection(bool isScanning)
        {
            if (!travelManager.IsNowPlanet(poiPlanet))
                return;

            view.SetScanerDetection(isScanning);
        }

        private void ChangeScale()
        {
            float outerPOIRadius = GetOuterDist();
            float dist = GetMouseToObjDist();
            float alpha = 1.0f - (dist - outerPOIRadius * 1.1f) / (outerPOIRadius * 0.5f); //немного математики, чтобы размер не уменьшался, если мы в внутреннем радиусе

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
            return 0.5f * view.Size.magnitude;
        }
    }

}