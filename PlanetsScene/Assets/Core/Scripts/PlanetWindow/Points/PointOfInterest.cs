using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
using VContainer.Unity;

public interface IPointOfInterest : IScreenObject
{
    void SetInfo(PointInfo info);
    void SetVisibility(bool visibility);
    void SetInterective(bool interective);
    void SetScanerDetection(bool scanerDetection);
    void SetSpriteVisability(bool visibility);
    void SetPosition(Vector3 position);

    bool IsVisible { get; }
    bool IsInteractive { get; }

    PointInfo GetInfo { get; }

    GameObject GameObject { get; }
}

public class PointOfInterest : MonoBehaviour, IPointOfInterest, IDisposable //TODO: объединить с POIInteraction
{
    [Inject] private IScaner scaner;
    [Inject] private IPlanetWindow planetWindow;
    [Inject] private IDrone drone;

    [SerializeField] private BoxCollider2D collider2d;
    [SerializeField] private PlanetWindow_POIView view;

    private IPlanetWindow_POIObserver observer;
    private IGradientObserver gradientObserver;

    private PointInfo info;
    private bool isVisible = false;
    private bool isInterective = false;
    public bool IsVisible => isVisible;

    public GameObject GameObject => gameObject;

    public string Name => info.nameOfPoint;

    public Vector3 Position => gameObject.transform.position;

    public Vector3 Size => gameObject.transform.localScale;

    public PointInfo GetInfo => info;

    public bool IsInteractive => isInterective;

    [Inject]
    public void Construct(IObjectResolver resolver)
    {
        resolver.Inject(observer = new PlanetWindow_POIObserver(view, this));
        resolver.Inject(gradientObserver = new MouseMoveGradientObserver(view, this));

        drone.onLand += SetPOIActive;
        scaner.onChangeIsScanning += SetNotInterective; 
    }

    public void Dispose()
    {
        drone.onLand -= SetPOIActive;
        scaner.onChangeIsScanning -= SetNotInterective;
        Disable();
    }

    public void SetInfo(PointInfo info)
    {
        this.info = info;
        SetVisibility(info.isVisible);
    }

    public void SetInterective(bool interective)
    {
        Debug.Log(interective);
        isInterective = interective;
        if (interective)
        {
            Enable();
        }
        else
        {
            Disable();
        }
    }
    public void SetNotInterective(bool notInterective)
    {
        SetInterective(!notInterective);
    }
    public void SetPosition(Vector3 position)
    {
        gameObject.transform.position = position;
    }

    public void SetScanerDetection(bool scanerDetection)
    {
        collider2d.enabled = scanerDetection;
    }

    public void SetSpriteVisability(bool visibility)
    {
        if(visibility)
        {
            observer.Enable();
        }
        else
        {
            observer.Disable();
        }
    }

    public void SetVisibility(bool visibility)
    {
        isVisible = visibility;
        SetSpriteVisability(isVisible);
    }

    private void SetPOIActive(Vector3 pos, IPointOfInterest poi)
    {        
        if (this != poi)
            return;

        SetVisibility(true);
        SetInterective(true);
    }

    private void Enable()
    {
        gradientObserver.Enable();
        gradientObserver.SetInterective();

        planetWindow.onEnable += gradientObserver.SetNotInterective;
        planetWindow.onDisable += gradientObserver.SetInterective;

        scaner.onScanerEnable += gradientObserver.SetNotInterective;
        scaner.onScanerDisable += gradientObserver.SetInterective;
    }

    private void Disable()
    {
        gradientObserver.Disable();
        gradientObserver.SetNotInterective();

        planetWindow.onEnable -= gradientObserver.SetNotInterective;
        planetWindow.onDisable -= gradientObserver.SetInterective;

        scaner.onScanerEnable -= gradientObserver.SetNotInterective;
        scaner.onScanerDisable -= gradientObserver.SetInterective;
    }
}
