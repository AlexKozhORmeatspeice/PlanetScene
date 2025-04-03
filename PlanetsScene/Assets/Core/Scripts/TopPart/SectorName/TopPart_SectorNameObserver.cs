using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public interface ITopPart_SectorNameObserver
{
    void Enable();
    void Disable();
}

public class TopPart_SectorNameObserver : MonoBehaviour, ITopPart_SectorNameObserver
{
    private ITopPart_SectorName view;
    [Inject] private IPlanetWindow planetWindow;
    [Inject] private ITravelManager travelManager;

    public TopPart_SectorNameObserver(ITopPart_SectorName view)
    {
        this.view = view;
    }

    public void Enable()
    {
        travelManager.onTravel += ChangeSectorName;
        
        planetWindow.onEnable += view.Disable;
        planetWindow.onDisable += view.Enable;
    }

    public void Disable()
    {
        travelManager.onTravel -= ChangeSectorName;

        planetWindow.onEnable -= view.Disable;
        planetWindow.onDisable -= view.Enable;
    }

    void ChangeSectorName()
    {
        view.Name = travelManager.nowSectorName;
    }
}
