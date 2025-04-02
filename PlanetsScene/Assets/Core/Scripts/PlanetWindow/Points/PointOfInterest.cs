using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

public interface IPointOfInterest : IScreenObject
{
    void SetInfo(PointInfo info);
    void SetVisibility(bool visibility);
    void SetInterective(bool interective);
    void SetScanerDetection(bool scanerDetection);
    void SetSpriteVisability(bool visibility);
    void SetPosition(Vector3 position);

    bool IsVisible { get; }

    PointInfo GetInfo { get; }

    GameObject GameObject { get; }
}

public class PointOfInterest : MonoBehaviour, IPointOfInterest
{
    [SerializeField] private BoxCollider2D collider2d;
    [SerializeField] private POIInteraction interaction;
    [SerializeField] private PlanetWindow_POIView view;

    private IPlanetWindow_POIObserver observer;

    private PointInfo info;
    private bool isVisible = false;

    public bool IsVisible => isVisible;

    public GameObject GameObject => gameObject;

    public string Name => info.nameOfPoint;

    public Vector3 Position => gameObject.transform.position;

    public Vector3 Size => gameObject.transform.localScale;

    public PointInfo GetInfo => info;

    [Inject]
    public void Construct(IObjectResolver resolver)
    {
        resolver.Inject(interaction);
        resolver.Inject(observer = new PlanetWindow_POIObserver(view, this));
    }

    public void SetInfo(PointInfo info)
    {
        this.info = info;
        SetVisibility(info.isVisible);
    }

    public void SetInterective(bool interective)
    {
        if (interective)
        {
            interaction.Enable();
        }
        else
        {
            interaction.Disable();
        }
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
}
