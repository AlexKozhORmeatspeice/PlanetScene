using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public interface IPlanetWindow_POIObserver
{
    void Enable();
    void Disable();
}

public class PlanetWindow_POIObserver : IPlanetWindow_POIObserver
{
    private PlanetWindow_POIView view;
    private IPointOfInterest poi;

    public PlanetWindow_POIObserver(PlanetWindow_POIView view, IPointOfInterest poi)
    {
        this.view = view;
        this.poi = poi;
    }

    public void Enable()
    {
        view.SetIcon(poi.GetInfo.icon);
        view.PlayActivationAnim();
    }

    public void Disable()
    {
        view.SetVisibility(false);
    }
}
