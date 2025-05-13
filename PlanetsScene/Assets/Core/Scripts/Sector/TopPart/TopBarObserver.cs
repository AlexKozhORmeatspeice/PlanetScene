using System.Diagnostics;
using Cysharp.Threading.Tasks.Triggers;
using Frontier_UI;
using Planet_Window;
using Space_Screen;
using Top_Bar;
using VContainer;

namespace Space_TopBar
{
    public interface ITopBarObserver
    {
        void Enable();
        void Disable();
    }

    public class TopBarObserver : ITopBarObserver
    {
        [Inject] private IStartFadePanel startFadePanel;
        [Inject] private IPlanetWindow planetWindow;
        [Inject] private ITravelManager travelManager;
        [Inject] private IIconButtonMouseEvents iconButtonMouseEvents;

        private ITopBarView view;

        public TopBarObserver(ITopBarView view)
        {
            this.view = view;
        }

        public void Enable()
        {
            startFadePanel.onEndShowScreen += ShowAllBars;

            planetWindow.onEnable += view.AnimateHideSectorBar;
            planetWindow.onDisable += view.AnimateShowSectorBar;
        }

        public void Disable()
        {
            startFadePanel.onEndShowScreen -= ShowAllBars;

            planetWindow.onEnable -= view.AnimateHideSectorBar;
            planetWindow.onDisable -= view.AnimateShowSectorBar;
        }

        private void ShowAllBars()
        {
            view.AnimateShowSectorBar();
            view.AnimateShowButtonsBar();
        }
    }
}