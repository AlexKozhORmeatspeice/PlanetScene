using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using UnityEngine;
using VContainer;

public interface IPOIInteraction
{
    void Enable();
    void Disable();
}

public class POIInteraction : MonoBehaviour, IPOIInteraction, IDisposable
{
    [Inject] private IScaner scaner;
    [Inject] private IPlanetWindow planetWindow;
    [Inject] private POIToolTip toolTip;

    [SerializeField] private PointOfInterest pointOfInterest;
    [SerializeField] private PlanetWindow_POIView view;

    private IGradientObserver gradientObserver;

    [Inject]
    public void Construct(IObjectResolver resolver)
    {
        resolver.Inject(gradientObserver = new MouseMoveGradientObserver(view, pointOfInterest));
    }

    public void Enable()
    {
        gradientObserver.Enable();
        gradientObserver.SetInterective();

        planetWindow.onEnable += gradientObserver.SetNotInterective;
        planetWindow.onDisable += gradientObserver.SetInterective;

        scaner.onScanerEnable += gradientObserver.SetNotInterective;
        scaner.onScanerDisable += gradientObserver.SetInterective;

        scaner.onScanerEnable += DisableToolTipInteraction;
        scaner.onScanerDisable += EnableToolTipInteraction;

        EnableToolTipInteraction();
    }

    public void Disable()
    {
        gradientObserver.Disable();
        gradientObserver.SetNotInterective();

        planetWindow.onEnable -= gradientObserver.SetNotInterective;
        planetWindow.onDisable -= gradientObserver.SetInterective;

        DisableToolTipInteraction();
    }

    private void EnableToolTip()
    {
        toolTip.Enable(pointOfInterest);
    }

    private void EnableToolTipInteraction()
    {
        gradientObserver.onEnterObj += EnableToolTip;
        gradientObserver.onExitObj += toolTip.Disable;
    }

    private void DisableToolTipInteraction()
    {
        gradientObserver.onEnterObj -= EnableToolTip;
        gradientObserver.onExitObj -= toolTip.Disable;

        toolTip.Disable();
    }

    public void Dispose()
    {
        Disable();
    }
}
