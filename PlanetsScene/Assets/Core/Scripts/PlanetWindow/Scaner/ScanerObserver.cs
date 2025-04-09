using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;

namespace Planet_Window
{
    public interface IScanerObserver
    {
        void Enable();
        void Disable();
    }

    public class ScanerObserver : IScanerObserver
    {
        private IScaner scaner;
        [Inject] private IPointerManager pointer;

        private IPlanetWindow_Scaner view;
        private float lastSpeed;
        public ScanerObserver(IPlanetWindow_Scaner view, IScaner scaner)
        {
            this.scaner = scaner;
            this.view = view;
            lastSpeed = 0.0f;
        }

        public void Enable()
        {
            scaner.onStartScanning += EnableScanning;
            pointer.OnUpdate += ChangeSize;

            view.isVisible = true;
            view.scanerFiledIsVisible = false;
        }

        public void Disable()
        {
            view.isVisible = false;
            view.scanerFiledIsVisible = false;

            scaner.onStartScanning -= EnableScanning;
            pointer.OnUpdate -= ChangeSize;
        }

        private void ChangeSize()
        {
            float speed = Mathf.Pow(pointer.Speed.magnitude, view.MouseSpeedInfluence);
            lastSpeed = Mathf.Lerp(lastSpeed, speed, Time.deltaTime * view.SpeedOfChange);
            lastSpeed = Mathf.Clamp01(lastSpeed);

            view.ChangeSize(lastSpeed);
        }

        private void EnableScanning()
        {
            view.scanerFiledIsVisible = true;
        }
    }

}