using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;
using Planet_Window;
using Frontier_UI;

namespace Space_Screen
{
    public interface ISpaceWindow
    {

    }

    public class SpaceWindow : MonoBehaviour, ISpaceWindow, IDisposable
    {
        [SerializeField] private GameObject planetsContent;
        [SerializeField] private StartFadePanel fadePanel;

        private List<IPlanetObserver> planetObservers;
        private IFadePanelObserver fadePanelObserver;

        [Inject]
        public void Construct(IObjectResolver resolver)
        {
            planetObservers = new List<IPlanetObserver>();

            var planets = planetsContent.GetComponentsInChildren<IPlanet>().ToList();
            foreach (var planet in planets)
            {
                var planetObserver = new PlanetObserver(planet);
                resolver.Inject(planetObserver);
                planetObserver.Enable();
                planetObservers.Add(planetObserver);
            }

            fadePanelObserver = new FadePanelObserver(fadePanel);
            resolver.Inject(fadePanelObserver);
            fadePanelObserver.Enable();
        }

        public void Dispose()
        {
            foreach (var planet in planetObservers)
            {
                planet.Disable();
            }
            planetObservers.Clear();

            fadePanelObserver.Disable();
        }
    }

}
