using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public interface IPlanetWindow
{
    bool IsEnabled { get; }

    event Action onEnable;
    event Action onDisable;

    void Enable(IPlanet planet);
    void Enable();
    void Disable();
}

public class PlanetWindow : MonoBehaviour, IPlanetWindow, IStartable, IDisposable
{
    [Inject] private IScaner scaner;
    [Inject] private ITravelManager travelManager;

    private IPlanetWindowObserver observer;
    private IPlanetWindow_ScanerButtonPresenter scanerButtonPresenter;

    [SerializeField] private PlanetWindowView planetWindowView;
    [SerializeField] private PlanetWindow_ScanerButton scanerButton;

    private IPlanet showPlanet;

    public bool IsEnabled => gameObject.activeSelf;

    public event Action onEnable;
    public event Action onDisable;

    [Inject]
    public void Construct(IObjectResolver resolver)
    {
        resolver.Inject(scanerButtonPresenter = new PlanetWindow_ScanerButtonPresenter(scanerButton, scaner));
        resolver.Inject(observer = new PlanetWindowObserver(planetWindowView, scaner, travelManager));
    }

    public void Initialize()
    {
        travelManager.onTravelToPlanet += Enable;
    }

    public void Dispose()
    {
        Disable();
        travelManager.onTravelToPlanet -= Enable;
    }


    void IStartable.Start() { }

    public void Enable(IPlanet planet)
    {
        showPlanet = planet;

        observer.Enable(planet);
        scanerButtonPresenter.Enable();

        onEnable?.Invoke();
        gameObject.SetActive(true);
    }

    public void Enable()
    {
        if (showPlanet == null)
            return;

        Enable(showPlanet);
    }

    public void Disable()
    {
        observer.Disable();
        scanerButtonPresenter.Disable();
        
        onDisable?.Invoke();

        scaner.SetStatus(false);
    }
}
