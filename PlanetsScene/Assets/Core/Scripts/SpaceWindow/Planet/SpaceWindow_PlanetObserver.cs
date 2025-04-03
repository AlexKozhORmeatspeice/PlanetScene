using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;

public interface ISpaceWindow_PlanetObserver
{
    void Enable();
    void Disable();
}

public class SpaceWindow_PlanetObserver : MouseMoveGradientObserver, ISpaceWindow_PlanetObserver
{
    [Inject] private IPlanetWindow planetWindow;
    [Inject] private ITravelManager travelManager;
    
    private ISpaceWindow_PlanetView view;
    private IPlanet planet;

    private bool isChoosed;

    public SpaceWindow_PlanetObserver(ISpaceWindow_PlanetView view, IPlanet planet) : base(view, planet)
    {
        this.view = view;
        this.planet = planet;
    }

    public override void Enable()
    {
        base.Enable();
        SetInterective();

        planetWindow.onEnable += SetNotInterective;
        planetWindow.onDisable += SetInterective;

        travelManager.onTravelToPlanet += CheckChoosedPlanet;
    }

    public override void Disable()
    {
        base.Disable();

        SetNotInterective();

        planetWindow.onEnable -= SetNotInterective;
        planetWindow.onDisable -= SetInterective;

        travelManager.onTravelToPlanet -= CheckChoosedPlanet;
    }

    private void CheckChoosedPlanet(IPlanet choosedPlanet)
    {
        if (planet == choosedPlanet && !isChoosed)
        {
            EnableAsChoosedPlanet();
        }
        else if(planet != choosedPlanet && isChoosed)
        {
            DisableAsChoosedPlanet();
        }
    }

    private void EnableAsChoosedPlanet()
    {
        SetNotInterective();

        planetWindow.onEnable -= SetNotInterective;
        planetWindow.onDisable -= SetInterective;

        view.SetVisibility(true);
        isChoosed = true;
    }

    private void DisableAsChoosedPlanet()
    {
        SetInterective();

        planetWindow.onEnable += SetNotInterective;
        planetWindow.onDisable += SetInterective;

        view.SetVisibility(false);
        isChoosed = false;
    }
}
