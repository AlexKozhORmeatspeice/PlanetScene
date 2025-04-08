using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public interface IPlanetWindow_NotFoundObserver
{
    void Enable();
    void Disable();
}

public class PlanetWindow_NotFoundObserver : MonoBehaviour, IPlanetWindow_NotFoundObserver
{
    [Inject] private IDrone drone;
    private IPlanetWindow_NotFound view;

    public PlanetWindow_NotFoundObserver(IPlanetWindow_NotFound view)
    {
        this.view = view;
    }

    public void Enable()
    {
        drone.onLand += Activate;
    }

    public void Disable()
    {
        drone.onLand -= Activate;
    }

    private void Activate(Vector3 pos, IPointOfInterest poi)
    {
        if (poi != null)
            return;

        view.Pos = pos;
        view.PlayAnim();
    }
}
