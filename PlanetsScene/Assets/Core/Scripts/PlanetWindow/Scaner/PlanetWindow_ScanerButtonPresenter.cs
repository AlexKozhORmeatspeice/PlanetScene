using VContainer;

public interface IPlanetWindow_ScanerButtonPresenter
{
    void Init(IPlanetWindow_ScanerButton view);
    bool ChangeScanerStatus();
}

public class PlanetWindow_ScanerButtonPresenter : IPlanetWindow_ScanerButtonPresenter
{
    [Inject] private Scaner scaner;
    private IPlanetWindow_ScanerButton view;

    public void Init(IPlanetWindow_ScanerButton view)
    {
        this.view = view;

        view.SetStatus(scaner.IsScanning);
        scaner.onScanerDisable += DisableScaner;
    }

    public bool ChangeScanerStatus()
    {
        scaner.SwapStatus();
        return scaner.IsScanning;
    }

    private void DisableScaner()
    {
        view.SetStatus(scaner.IsScanning);
    }
}
