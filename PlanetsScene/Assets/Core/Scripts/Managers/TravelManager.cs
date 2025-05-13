using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Space_Screen;
using Planet_Window;
using Cysharp.Threading.Tasks;
using System.Numerics;
using Frontier_anim;

public interface ITravelManager
{
    IPlanet nowPlanet { get; }
    string nowPlanetName { get; }
    string nowSectorName { get; }
    string nowPlanetDesc { get; }

    bool IsNowPlanet(IPlanet planet);
    event Action<IPlanet> onTravelToPlanet;
    public event Action<IPlanet> onLandToPlanet;
    event Action onTravel;
}

public class TravelManager : MonoBehaviour, ITravelManager, IStartable, IDisposable
{
    [Inject] private IPlanetMouseEvents planetMouse;
    private IPlanet choosedPlanet;

    public event Action<IPlanet> onTravelToPlanet;
    public event Action<IPlanet> onLandToPlanet;
    public event Action onTravel;
    
    private bool isCanTravel = false;

    public IPlanet nowPlanet {
        get
        {
            return choosedPlanet;
        }
    }

    public string nowPlanetName => choosedPlanet == null ? "" : choosedPlanet.Name;

    public string nowSectorName => choosedPlanet == null ? "" : choosedPlanet.SectornName;

    public string nowPlanetDesc => choosedPlanet == null ? "" : choosedPlanet.Description;

    public bool IsNowPlanet(IPlanet planet)
    { 
        return choosedPlanet != null && choosedPlanet.Equals(planet);
    }
    private async void SetNowPlanet(IPlanet planet)
    {
        if (!isCanTravel)
            return;

        choosedPlanet = planet;

        onTravelToPlanet?.Invoke(planet);
        onTravel?.Invoke();

        await LandToPlanet(planet);
    }

    private async UniTask LandToPlanet(IPlanet planet)
    {
        await UniTask.Delay(500);

        onLandToPlanet?.Invoke(choosedPlanet);
    }


    public void Initialize()
    {
        isCanTravel = true;
        choosedPlanet = null;
        planetMouse.OnMouseClick += SetNowPlanet;
    }

    public void Dispose()
    {
        planetMouse.OnMouseClick -= SetNowPlanet;
    }

    public void Start() { }
}