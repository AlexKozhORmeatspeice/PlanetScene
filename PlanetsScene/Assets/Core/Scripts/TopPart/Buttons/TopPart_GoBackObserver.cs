using VContainer;

public interface ITopPart_GoBackObserver
{
    void Enable();
    void Disable();
}

public class TopPart_GoBackObserver : ITopPart_GoBackObserver
{
    [Inject] private IPlanetWindow planetWindow;
    private ITopPart_GoBack view;
    public TopPart_GoBackObserver(ITopPart_GoBack view)
    {
        this.view = view;
    }
    
    public void Enable()
    {
        view.onClick += ChangePlanetVisability;
    }
    public void Disable()
    {
        view.onClick -= ChangePlanetVisability;
    }

    private void ChangePlanetVisability()
    {
        if(planetWindow.IsEnabled)
        {
            planetWindow.Disable();
        }
        else
        {
            planetWindow.Enable();
        }
    }
}
