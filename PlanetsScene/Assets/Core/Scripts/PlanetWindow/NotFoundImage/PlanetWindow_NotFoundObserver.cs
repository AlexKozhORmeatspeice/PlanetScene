using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlanetWindow_NotFoundObserver
{
    void Enable(Vector3 pos);
    void Disable();
}

public class PlanetWindow_NotFoundObserver : MonoBehaviour, IPlanetWindow_NotFoundObserver
{
    private IPlanetWindow_NotFound view;

    public PlanetWindow_NotFoundObserver(IPlanetWindow_NotFound view)
    {
        this.view = view;
    }

    public void Enable(Vector3 pos)
    {
        view.Pos = pos;
        view.PlayAnim();
    }

    public void Disable()
    {
        view.SetActive(false);
    }
}
