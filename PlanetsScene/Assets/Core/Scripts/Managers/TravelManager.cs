using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Space_Screen;

public interface ITravelManager
{
    IPlanet nowPlanet { get; }
    string nowPlanetName { get; }
    string nowSectorName { get; }
    string nowPlanetDesc { get; }

    bool IsNowPlanet(IPlanet planet);
    event Action<IPlanet> onTravelToPlanet;
    event Action onTravel;
}

public class TravelManager : MonoBehaviour, ITravelManager, IStartable, IDisposable
{
    [Inject] private IPlanetMouseEvents planetMouse;
    private IPlanet activePlanet;
    public event Action<IPlanet> onTravelToPlanet;
    public event Action onTravel;

    public IPlanet nowPlanet {
        get
        {
            return activePlanet;
        }
    }

    public string nowPlanetName => activePlanet == null ? "" : activePlanet.Name;

    public string nowSectorName => activePlanet == null ? "" : activePlanet.SectornName;

    public string nowPlanetDesc => activePlanet == null ? "" : activePlanet.Description;

    public bool IsNowPlanet(IPlanet planet)
    {
        return activePlanet != null && activePlanet.Equals(planet);
    }
    private void SetNowPlanet(IPlanet planet)
    {
        activePlanet = planet;
        
        onTravelToPlanet?.Invoke(planet);
        onTravel?.Invoke();
    }

    public void Initialize()
    {
        activePlanet = null;
        planetMouse.OnMouseDoubleClick += SetNowPlanet;
    }

    public void Dispose()
    {
        planetMouse.OnMouseDoubleClick -= SetNowPlanet;
    }

    public void Start() { }
}
