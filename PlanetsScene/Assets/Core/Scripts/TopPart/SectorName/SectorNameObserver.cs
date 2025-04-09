using System.Collections;
using System.Collections.Generic;
using Planet_Window;
using UnityEngine;
using VContainer;

namespace Top_Bar
{
    public interface ISectorNameObserver
    {
        void Enable();
        void Disable();
    }

    public class SectorNameObserver : MonoBehaviour, ISectorNameObserver
    {
        private ITopPart_SectorName view;
        [Inject] private IPlanetWindow planetWindow;
        [Inject] private ITravelManager travelManager;

        public SectorNameObserver(ITopPart_SectorName view)
        {
            this.view = view;
        }

        public void Enable()
        {
            travelManager.onTravel += ChangeSectorName;

            planetWindow.onEnable += view.Disable;
            planetWindow.onDisable += view.Enable;
        }

        public void Disable()
        {
            travelManager.onTravel -= ChangeSectorName;

            planetWindow.onEnable -= view.Disable;
            planetWindow.onDisable -= view.Enable;
        }

        void ChangeSectorName()
        {
            view.Name = travelManager.nowSectorName;
        }
    }

}