using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;
using VContainer.Unity;

public interface IPlanetToolTipSystem
{

}

public class PlanetTooltipSystem : MonoBehaviour, IPlanetToolTipSystem, IStartable
{
    [Inject] private ITravelManager travelManager;
    [Inject] private PlanetToolTip toolTip;
    [Inject] private IPlanetMouseEvents planetMouse;
    private IPlanet nowPlanet;

    public void Initialize()
    {
        planetMouse.OnMouseEnter += SetPlanet;
        planetMouse.OnMouseLeave += DisablePlanet;
    }

    public void Start() {}

    private void SetPlanet(IPlanet planet)
    {
        if (travelManager.IsNowPlanet(planet))
            return;

        nowPlanet = planet;
        toolTip.Enable(nowPlanet);
    }

    private void DisablePlanet(IPlanet planet)
    {
        if (travelManager.IsNowPlanet(planet))
            return;

        toolTip.Disable();
        nowPlanet = null;
    }
}
