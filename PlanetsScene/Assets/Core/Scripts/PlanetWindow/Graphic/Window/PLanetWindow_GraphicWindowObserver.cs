using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public interface IPLanetWindow_GraphicWindowObserver
{
    void Enable();
    void Disable();
}

public class PLanetWindow_GraphicWindowObserver : IPLanetWindow_GraphicWindowObserver
{
    [Inject] private IScaner scaner;
    [Inject] private IPointOfInterestGraphic graphic;

    private IPLanetWindow_GraphicWindow view;

    public PLanetWindow_GraphicWindowObserver(IPLanetWindow_GraphicWindow view)
    {
        this.view = view;
    }

    public void Enable()
    {
        view.Disable();
        graphic.Disable();
        scaner.onChangeIsScanning += SetVisible;
    }

    public void Disable()
    {
        view.Disable();
        graphic.Disable();
        scaner.onChangeIsScanning -= SetVisible;
    }

    private void SetVisible(bool notVisible)
    {
        if (notVisible)
        {
            graphic.Enable();
            view.Enable();
        }
        else
        {
            graphic.Disable();
            view.Disable();
        }
    }
}
