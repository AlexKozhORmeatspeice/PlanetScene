using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public interface IPlanetWindow_ScanerFiledObserver
{
    void Enable();
    void Disable();
}
public class PlanetWindow_ScanerFiledObserver : IPlanetWindow_ScanerFiledObserver
{
    private IPlanetWindow_ScanerFiled view;
    public PlanetWindow_ScanerFiledObserver(IPlanetWindow_ScanerFiled view)
    {
        this.view = view;
        view.SetAlpha(0.0f);
    }

    public void Enable()
    {
        view.SetAlpha(1.0f);
    }

    public void Disable()
    {
        view.SetAlpha(0.0f);
    }
}
