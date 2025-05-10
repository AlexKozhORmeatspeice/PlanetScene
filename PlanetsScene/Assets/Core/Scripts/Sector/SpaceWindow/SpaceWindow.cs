using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;
using Planet_Window;

namespace Space_Screen
{
    public interface ISpaceWindow
    {

    }

    public class SpaceWindow : MonoBehaviour, ISpaceWindow, IDisposable
    {
        [SerializeField] private GameObject planetsContent;
        private List<IPlanetObserver> planetObservers;

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
        }

        public void Dispose()
        {
            foreach (var planet in planetObservers)
            {
                planet.Disable();
            }
            planetObservers.Clear();
        }
    }

}
