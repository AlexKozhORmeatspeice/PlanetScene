using Game_camera;
using UnityEngine;
using VContainer;

namespace Menu
{
    public interface IMenuSideBarObserver
    {
        void Enable();
        void Disable();
    }

    public class MenuSideBarObserver : IMenuSideBarObserver
    {
        [Inject] private IMenuScreen menuScreen;
        [Inject] private ICameraView mainCamera;

        private IMenuSideBarView view;
        Vector3 startPos;
        public MenuSideBarObserver(IMenuSideBarView view)
        {
            this.view = view;
        }

        public void Enable()
        {
            startPos = view.LocalPosition;

            Vector3 animStartPos = startPos;
            animStartPos.x += menuScreen.XStartAnimOffset;
            view.LocalPosition = animStartPos;

            mainCamera.onEndMove += AnimatePosition;
        }

        public void Disable()
        {
            mainCamera.onEndMove -= AnimatePosition;
        }

        private void AnimatePosition()
        {
            view.AnimatePosition(startPos);
        }
    }
}