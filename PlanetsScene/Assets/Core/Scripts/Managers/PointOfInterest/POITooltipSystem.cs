using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;
using VContainer.Unity;

public interface IPOITooltipSystem
{

}

public class POITooltipSystem : MonoBehaviour, IPOITooltipSystem, IStartable, IDisposable
{
    [Inject] private ITravelManager travelManager;
    [Inject] private POIToolTip toolTip;
    [Inject] private IPOIMouseEvents POIMouse;
    private IPointOfInterest nowPoint;

    public void Initialize()
    {
        POIMouse.OnMouseEnter += SetPOI;
        POIMouse.OnMouseLeave += DisablePOI;
    }

    public void Dispose()
    {
        POIMouse.OnMouseEnter -= SetPOI;
        POIMouse.OnMouseLeave -= DisablePOI;
    }

    public void Start() { }

    private void SetPOI(IPointOfInterest point)
    {
        if (point == null || !point.IsVisible || !point.IsInteractive)
            return;

        nowPoint = point;
        toolTip.Enable(nowPoint);
    }

    private void DisablePOI(IPointOfInterest point)
    {
        toolTip.Disable();
        nowPoint = null;
    }
}
