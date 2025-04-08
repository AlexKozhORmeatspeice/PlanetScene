using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Vector3 = UnityEngine.Vector3;

public interface IPlanet : IScreenObject
{
    void Init(IObjectResolver resolver, ITravelManager travelManager, IPlanetWindow planetWindow);
    void Enable();
    void Disable();
    List<PointInfo> PointInfos { get; }

    string SectornName { get; }
    string Description { get; }
}

public class Planet : MonoBehaviour, IPlanet
{
    private ISpaceWindow_PlanetObserver observer;
    [SerializeField] private SpaceWindow_PlanetView view;

    [Header("Info")]
    [SerializeField] private string planetName;
    [SerializeField] private string sector;
    [SerializeField] private string description;

    [Header("Points of interest")]
    [SerializeField] private List<PointInfo> points;
    
    public string Name => planetName;

    public string Description => description;

    public string SectornName => sector;

    public Vector3 Position => gameObject.transform.position;

    public Vector3 Size => gameObject.transform.localScale;

    List<PointInfo> IPlanet.PointInfos => points;

    public void Init(IObjectResolver resolver, ITravelManager travelManager, IPlanetWindow planetWindow)
    {
        resolver.Inject(observer = new SpaceWindow_PlanetObserver(view, this, travelManager, planetWindow));
        Enable();
    }

    public void Enable()
    {
        observer.Enable();
    }

    public void Disable()
    {
        observer.Disable();
    }

    public void Dispose()
    {
        Disable();
    }
}
