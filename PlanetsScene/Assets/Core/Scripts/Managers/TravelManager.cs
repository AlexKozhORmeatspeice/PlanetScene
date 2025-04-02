using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;

public interface ITravelManager
{
    IPlanetInfo nowPlanet { set; get; }
    string nowPlanetName { get; }
    string nowSectorName { get; }
    string nowPlanetDesc { get; }

    bool IsNowPlanet(IPlanetInfo planet);
    event Action onTravelToAnotherPlanet;
}

public class TravelManager : MonoBehaviour, ITravelManager
{
    private IPlanetInfo activePlanet;
    public IPlanetInfo nowPlanet {
        set
        {
            onTravelToAnotherPlanet?.Invoke();
            activePlanet = value;
        }
        get
        {
            return activePlanet;
        }
    }

    public string nowPlanetName => activePlanet == null ? "" : activePlanet.Name;

    public string nowSectorName => activePlanet == null ? "" : activePlanet.SectornName;

    public string nowPlanetDesc => activePlanet == null ? "" : activePlanet.Description;

    public bool IsNowPlanet(IPlanetInfo planet)
    {
        return activePlanet != null && activePlanet.Equals(planet);
    }

    public event Action onTravelToAnotherPlanet;

    [Inject]
    public void Construct()
    {
        nowPlanet = null;
    }
}
