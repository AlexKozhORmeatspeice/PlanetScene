using Game_camera;
using UnityEditor;
using UnityEngine;
using VContainer;

namespace Menu
{
    public interface ILogoObserver
    {
        void Enable();
        void Disable();
    }

    public class LogoObserver : ILogoObserver
    {
        [Inject] private ICameraView mainCamera;
        private ILogoView view;
        
        public LogoObserver(ILogoView view)
        {
            this.view = view;
        }

        public void Enable()
        {
            Color nowColor = view.ImageColor;
            nowColor.a = 0.0f;
            view.ImageColor = nowColor;

            mainCamera.onEndMove += AnimateAlpha;
        }

        public void Disable()
        {
            mainCamera.onEndMove -= AnimateAlpha;
        }
        
        private void AnimateAlpha()
        {
            view.AnimateAlpha(1.0f);
        }
    }
}