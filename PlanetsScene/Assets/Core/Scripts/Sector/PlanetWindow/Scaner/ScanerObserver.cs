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
        }

        public void Enable()
        {
            lastSpeed = 0.0f;

            scaner.onScanerEnable += EnableScanning;
            pointer.OnUpdate += ChangeSize;

            view.isVisible = true;
            view.scanerFiledIsVisible = false;
        }

        public void Disable()
        {
            view.isVisible = false;
            view.scanerFiledIsVisible = false;

            scaner.onScanerEnable -= EnableScanning;
            pointer.OnUpdate -= ChangeSize;
        }

        private void ChangeSize()
        {
            float speed = Mathf.Clamp01(pointer.Speed.magnitude / view.MaxMouseSpeed);
            
            if(speed > lastSpeed)
            {
                lastSpeed = speed;
            }
            else
            {
                lastSpeed -= view.SpeedOfDecrise * Time.deltaTime;
            }

            view.ChangeSize(lastSpeed);
        }

        private void EnableScanning()
        {
            view.scanerFiledIsVisible = true;
        }
    }

}