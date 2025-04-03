using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public interface IPlanetWindow
{
    bool IsEnabled { get; }

    event Action onEnable;
    event Action onDisable;

    void Enable();
    void Disable();
}

public class PlanetWindow : MonoBehaviour, IPlanetWindow, IStartable, IDisposable
{
    [Inject] private ITravelManager travelManager;
    [Inject] private IPointerManager pointerManager;
    [Inject] private IScaner scaner;
    
    private IPlanetWindow_ScanerButtonPresenter scanerButtonPresenter;
    private IPlanetWindow_InfoObserver infoObserver;
    private IPLanetWindow_GraphicWindowObserver graphicObserver;
    private IPlanetWindow_ScanerFiledObserver scanerFieldObserver;

    [SerializeField] private PlanetWindow_ScanerButton scanerButton;
    [SerializeField] private PlanetWindow_Info planetInfo;
    [SerializeField] private PLanetWindow_GraphicWindow graphic;
    [SerializeField] private PlanetWindow_ScanerFiled scanerField;

    public bool IsEnabled => gameObject.activeSelf;

    public event Action onEnable;
    public event Action onDisable;

    [Inject]
    public void Construct(IObjectResolver resolver)
    {
        resolver.Inject(infoObserver = new PlanetWindow_InfoObserver(planetInfo));
        resolver.Inject(scanerButtonPresenter = new PlanetWindow_ScanerButtonPresenter());
        resolver.Inject(graphicObserver = new PLanetWindow_GraphicWindowObserver(graphic));
        resolver.Inject(scanerFieldObserver = new PlanetWindow_ScanerFiledObserver(scanerField));
    }

    public void Initialize()
    {
        scanerButton.Init(scanerButtonPresenter);
        travelManager.onTravel += Enable;
    }

    public void Dispose()
    {
        infoObserver.Disable();
        travelManager.onTravel -= Enable;
    }


    void IStartable.Start()
    {
        
    }

    public void Enable()
    {
        gameObject.SetActive(true);
        
        scanerFieldObserver.Disable();

        infoObserver.Enable();
        graphicObserver.Enable();
        
        scaner.onScanerEnable += scanerFieldObserver.Enable;
        scaner.onScanerDisable += scanerFieldObserver.Disable;

        onEnable?.Invoke();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
        
        infoObserver.Disable();
        graphicObserver.Disable();
        scaner.SetStatus(false);

        scaner.onScanerEnable -= scanerFieldObserver.Enable;
        scaner.onScanerDisable -= scanerFieldObserver.Disable;

        onDisable?.Invoke();
    }
}
