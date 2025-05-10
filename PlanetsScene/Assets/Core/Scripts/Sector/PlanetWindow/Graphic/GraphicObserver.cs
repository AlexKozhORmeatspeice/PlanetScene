using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public interface IGraphicObserver
{
    public void SetFunction(IGraphicView.Func func);

    void Enable();
    void Disable();
}

public class GraphicObserver : IGraphicObserver
{
    [Inject] private IPointerManager pointer;
    private IGraphicView view;
    
    public GraphicObserver(IGraphicView view)
    {
        this.view = view;
    }

    public void Enable()
    {
        view.Enable();
        pointer.OnUpdate += view.Render;
    }

    public void Disable()
    {
        pointer.OnUpdate -= view.Render;
        view.Disable();
    }

    public void SetFunction(IGraphicView.Func func)
    {
        view.SetFunction(func);
    }
}
