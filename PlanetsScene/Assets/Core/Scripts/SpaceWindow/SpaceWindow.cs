using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public interface ISpaceWindow
{

}

public class SpaceWindow : MonoBehaviour, ISpaceWindow, IDisposable
{
    [SerializeField] private GameObject PlanetsContent;
    private List<IPlanet> planetUIs;

    [Inject]
    public void Construct(IObjectResolver resolver, ITravelManager travelManager, IPlanetWindow planetWindow)
    {
        planetUIs = PlanetsContent.GetComponentsInChildren<IPlanet>().ToList();
        foreach(var planet in planetUIs)
        {
            resolver.Inject(planet);
            planet.Init(resolver, travelManager, planetWindow);
        }
    }

    public void Dispose()
    {
        foreach(var planet in planetUIs)
        {
            planet.Disable();
        }
    }
}
