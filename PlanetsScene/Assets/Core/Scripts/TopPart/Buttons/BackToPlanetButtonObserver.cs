using VContainer;
using Planet_Window;

namespace Top_Bar
{
    public interface IBackToPlanetButtonObserver
    {
        void Enable();
        void Disable();
    }

    public class BackToPlanetButtonObserver : IBackToPlanetButtonObserver
    {
        [Inject] private IPlanetWindow planetWindow;
        private ITopPart_GoBack view;
        public BackToPlanetButtonObserver(ITopPart_GoBack view)
        {
            this.view = view;
        }

        public void Enable()
        {
            view.onClick += ChangePlanetVisability;
        }
        public void Disable()
        {
            view.onClick -= ChangePlanetVisability;
        }

        private void ChangePlanetVisability()
        {
            if (planetWindow.IsEnabled)
            {
                planetWindow.Disable();
            }
            else
            {
                planetWindow.Enable();
            }
        }
    }

}