
using VContainer;

public interface IPlanetWindow_InfoObserver
{
    void Enable();
    void Disable();

    void SetNotVisible(bool visible);
}
public class PlanetWindow_InfoObserver : IPlanetWindow_InfoObserver
{
    [Inject] private IScaner scaner;
    [Inject] private ITravelManager _travelManager;
    private IPlanetWindow_ScanerObserver scanerObserver;
    private IPlanetWindow_Info view;

    public PlanetWindow_InfoObserver(IPlanetWindow_Info view)
    {
        this.view = view;
    }

    public void SetNotVisible(bool notVisible)
    {
        if(notVisible)
        {
            view.HideInfo();
        }
        else
        {
            view.ShowInfo();
        }
    }

    public void Enable()
    {
        _travelManager.onTravel += UpdateInfo;
        UpdateInfo();

        scaner.onChangeIsScanning += SetNotVisible;
    }

    public void Disable()
    {
        _travelManager.onTravel -= UpdateInfo;

        scaner.onChangeIsScanning -= SetNotVisible;
        SetNotVisible(false);
    }

    private void UpdateInfo()
    {
        view.Title = _travelManager.nowSectorName;
        view.Name = _travelManager.nowPlanetName;
        view.Description = _travelManager.nowPlanetDesc;
    }
}