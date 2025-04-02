using VContainer;

internal interface ITopPart_ButtonsObserver
{
    void Enable();
    void Disable();
}

public class TopPart_ButtonsObserver : ITopPart_ButtonsObserver
{
    [Inject] private IScaner scaner;
    private ITopPart_ButtonsView view;

    public TopPart_ButtonsObserver(TopPart_ButtonsView view)
    {
        this.view = view;
    }

    public void Enable()
    {
        scaner.onScanerEnable += view.Disable;
        scaner.onScanerDisable += view.Enable;
    }

    public void Disable()
    {
        scaner.onScanerEnable -= view.Disable;
        scaner.onScanerDisable -= view.Enable;
    }
}