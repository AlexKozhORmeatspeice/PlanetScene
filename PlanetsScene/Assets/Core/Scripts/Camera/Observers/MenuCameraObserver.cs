using Game_camera;
using Load_screen;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace Menu
{
    public interface IMenuCameraObserver
    {
        void Enable();
    }

    public class MenuCameraObserver : IMenuCameraObserver
    {
        [Inject] private ICameraView view;
        private IMenuLoadScreen menuLoadScreen;

        public MenuCameraObserver(IMenuLoadScreen loadScreen)
        {
            menuLoadScreen = loadScreen;
        }

        public void Enable()
        {
            view.AnimateNewPosition(menuLoadScreen.EndCameraPos);
            view.AnimateNewSize(menuLoadScreen.EndCameraSize);
        }
    }
}