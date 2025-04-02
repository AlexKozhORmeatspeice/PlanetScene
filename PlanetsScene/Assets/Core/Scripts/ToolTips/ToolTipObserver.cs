public interface IToolTipObserver
{
    void Enable(string title = "", string lable = "", string text = "");
    void Enable();
    void Disable();
}
public class ToolTipObserver : IToolTipObserver
{
    IToolTipView view;
    public ToolTipObserver(IToolTipView view)
    {
        this.view = view;
    }

    public void Enable(string title = "", string lable = "", string text = "")
    {
        view.Name = title;
        view.Lable = lable;
        view.Description = text;

        Enable();
    }

    public void Enable()
    {
        view.Enable();
    }

    public void Disable()
    {
        view.Disable();
    }
}