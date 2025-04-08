using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public interface IPlanetWindowObserver
{
    void Enable(IPlanet planet);
    void Disable();
}

public class PlanetWindowObserver : MonoBehaviour, IPlanetWindowObserver
{
    private IScaner scaner;
    private ITravelManager travelManager;

    private IPlanetWindowView view;
    private IPlanet choosedPlanet;

    public PlanetWindowObserver(IPlanetWindowView view, IScaner scaner, ITravelManager travelManager)
    {
        this.view = view;
        this.scaner = scaner;
        this.travelManager = travelManager;
        view.HideScanerFiled();
    }

    public void Enable(IPlanet planet)
    {
        choosedPlanet = planet;

        EnableInfo();
        EnableScanerFiled();
        EnableGraphic();

        view.SetWindowVisabitility(true);
    }

    public void Disable()
    {
        view.SetWindowVisabitility(false);

        DisableInfo();
        DisableScanerFiled();
        DisableGraphic();
    }

    private void EnableInfo()
    {
        travelManager.onTravel += UpdateInfo;
        UpdateInfo();

        scaner.onChangeIsScanning += SetInfoNotVisible;
    }

    private void DisableInfo()
    {
        travelManager.onTravel -= UpdateInfo;

        scaner.onChangeIsScanning -= SetInfoNotVisible;
        SetInfoNotVisible(false);
    }

    private void EnableScanerFiled()
    {
        scaner.onScanerEnable += view.ShowScanerFiled;
        scaner.onScanerDisable += view.HideScanerFiled;
    }

    private void DisableScanerFiled()
    {
        view.HideScanerFiled();
        scaner.onScanerEnable -= view.ShowScanerFiled;
        scaner.onScanerDisable -= view.HideScanerFiled;
    }

    private void EnableGraphic()
    {
        view.SetGraphicVisibility(false);
        scaner.onChangeIsScanning += view.SetGraphicVisibility;
    }

    private void DisableGraphic()
    {
        view.SetGraphicVisibility(false);
        scaner.onChangeIsScanning -= view.SetGraphicVisibility;
    }

    private void UpdateInfo()
    {
        view.Title = choosedPlanet.SectornName;
        view.Name = choosedPlanet.Name;
        view.Description = choosedPlanet.Description;
    }

    private void SetInfoNotVisible(bool notVisible)
    {
        view.SetInfoVisibility(!notVisible);
    }
}
