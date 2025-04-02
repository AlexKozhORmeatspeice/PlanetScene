using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public interface IPlanetInteraction
{
    void Init(IObjectResolver resolver);
    void Enable();
    void Disable();
}

public class PlanetInteraction : MonoBehaviour, IPlanetInteraction, IDisposable
{
    [Inject] private IPlanetWindow planetWindow;
    [Inject] private IPointerManager pointer;
    [Inject] private ITravelManager travelManager;
    [Inject] private PlanetToolTip toolTip;

    [SerializeField] private PlanetInfo planet;
    [SerializeField] private SpaceWindow_PlanetView view;
    
    private IGradientObserver presenter;

    public void Init(IObjectResolver resolver)
    {
        resolver.Inject(presenter = new MouseMoveGradientObserver(view, planet));
    }

    public void Enable()
    {
        presenter.Enable();

        presenter.SetInterective();

        planetWindow.onEnable += presenter.SetNotInterective;
        planetWindow.onDisable += presenter.SetInterective;

        presenter.onEnterObj += EnableToolTip;
        presenter.onExitObj += toolTip.Disable;

        pointer.OnDoubleTap += EnableAsNowPlanet;
    }

    public void Disable()
    {
        presenter.Disable();

        presenter.SetNotInterective();

        planetWindow.onEnable -= presenter.SetNotInterective;
        planetWindow.onDisable -= presenter.SetInterective;

        presenter.onEnterObj -= EnableToolTip;
        presenter.onExitObj -= toolTip.Disable;

        pointer.OnDoubleTap -= EnableAsNowPlanet;
    }

    private void EnableToolTip()
    {
        toolTip.Enable(planet);
    }

    private void EnableAsNowPlanet()
    {
        if (travelManager.IsNowPlanet(planet) || !presenter.IsInObject)
            return;

        presenter.SetNotInterective();

        travelManager.nowPlanet = planet;
        travelManager.onTravelToAnotherPlanet += DisableAsNowPlanet;

        planetWindow.Enable();
        toolTip.Disable();

        presenter.onEnterObj -= EnableToolTip;
        planetWindow.onEnable -= presenter.SetNotInterective;
        planetWindow.onDisable -= presenter.SetInterective;

        view.SetVisibility(true);
    }


    private void DisableAsNowPlanet()
    {
        if (!travelManager.IsNowPlanet(planet))
            return;

        presenter.SetInterective();

        travelManager.onTravelToAnotherPlanet -= DisableAsNowPlanet;

        presenter.onEnterObj += EnableToolTip;
        planetWindow.onEnable += presenter.SetNotInterective;
        planetWindow.onDisable += presenter.SetInterective;

        view.SetVisibility(false);
    }

    public void Dispose()
    {
        Disable();
    }
}
