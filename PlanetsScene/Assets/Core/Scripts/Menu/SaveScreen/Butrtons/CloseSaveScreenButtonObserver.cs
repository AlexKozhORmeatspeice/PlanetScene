using Frontier_UI;
using VContainer;

namespace Save_screen
{
    public interface ICLoseScreen
    {
        void Enable();
        void Disable();
    }

    public class CloseSaveScreenButtonObserver : ICLoseScreen
    {
        [Inject] private IIconButtonMouseEvents iconMouse;

        private ISaveScreen saveScreen;

        private IIconButton view;
        public CloseSaveScreenButtonObserver(IIconButton view, ISaveScreen screen)
        {
            this.view = view;
            this.saveScreen = screen; 
        }

        public void Enable()
        {
            iconMouse.OnMouseEnter += AnimateHover;
            iconMouse.OnMouseLeave += AnimateBase;
            iconMouse.OnMouseClick += AnimatePress;

            view.onClick += CloseScreen;
            view.onClick += view.AnimateBaseState;
        }

        public void Disable()
        {
            iconMouse.OnMouseEnter -= AnimateHover;
            iconMouse.OnMouseLeave -= AnimateBase;
            iconMouse.OnMouseClick -= AnimatePress;

            view.onClick -= CloseScreen;
            view.onClick -= view.AnimateBaseState;
        }
        
        private void AnimateHover(IIconButton iconButton)
        {
            if (iconButton != view) return;

            view.AnimateHover();
        }

        private void AnimateBase(IIconButton iconButton)
        {
            if(iconButton != view) return;

            view.AnimateBaseState();
        }

        private void AnimatePress(IIconButton iconButton)
        {
            if (iconButton != view) return;

            view.AnimatePressed();
        }

        private void CloseScreen()
        {
            saveScreen.Disable();
        }
    }
}