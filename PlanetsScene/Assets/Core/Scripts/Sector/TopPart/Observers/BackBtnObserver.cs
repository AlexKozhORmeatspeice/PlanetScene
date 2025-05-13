using System;
using Frontier_UI;
using Planet_Window;
using VContainer;
namespace Space_TopBar
{
    public interface IBackBtnObserver
    {
        void Enable();
        void Disable();
    }

    public class BackBtnObserver : IBackBtnObserver
    {
        [Inject] private IIconButtonMouseEvents iconMouse;
        [Inject] private IPlanetWindow planetWindow;
        [Inject] private ITravelManager travelManager;

        private IIconButton view;

        public BackBtnObserver(IIconButton view)
        {
            this.view = view;
        }

        public void Enable()
        {
            view.onClick += OnBackBtnClick;

            iconMouse.OnMouseEnter += AnimateHover;
            iconMouse.OnMouseLeave += AnimateBase;
            iconMouse.OnMouseClick += AnimatePress;
        }

        public void Disable()
        {
            view.onClick -= OnBackBtnClick;

            iconMouse.OnMouseEnter -= AnimateHover;
            iconMouse.OnMouseLeave -= AnimateBase;
            iconMouse.OnMouseClick -= AnimatePress;
        }

        private void OnBackBtnClick()
        {
            if (planetWindow.IsEnabled)
            {
                planetWindow.Disable();
            }
            else if (travelManager.nowPlanet != null)
            {
                planetWindow.Enable();
            }
        }

        private void AnimateHover(IIconButton btn)
        {
            if (btn != view) return;

            view.AnimateHover();
        }

        private void AnimateBase(IIconButton btn)
        {
            if(btn != view) return;

            view.AnimateBaseState();
        }

        private void AnimatePress(IIconButton btn)
        {
            if (btn != view) return;
            
            view.AnimatePressed();
        }
    }
}
