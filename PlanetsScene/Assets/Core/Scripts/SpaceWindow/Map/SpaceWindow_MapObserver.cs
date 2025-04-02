using UnityEngine;
using VContainer;

public interface ISpaceWindow_MapObserver
{
    public void Enable();
    public void Disable();
}

public class SpaceWindow_MapObserver : ISpaceWindow_MapObserver
{
    [Inject] private IPlanetWindow planetWindow;
    [Inject] private IPointerManager _cursorManager;
    private Vector3 startViewPos;

    private ISpaceWindow_Map view;
    public SpaceWindow_MapObserver(ISpaceWindow_Map view)
    {
        this.view = view;
    }

    public void Enable()
    {
        SetInterective();

        planetWindow.onEnable += SetNotInterective;
        planetWindow.onDisable += SetInterective;
    }

    public void Disable()
    {
        SetNotInterective();
    }

    private void OnMouseDown()
    {
        startViewPos = view.Position;
    }

    private void OnMouseMove()
    {
        if (!_cursorManager.IsDown)
            return;

        view.Position = startViewPos - (Vector3)(_cursorManager.LastScreenClickPosition - _cursorManager.NowScreenPosition);
    }

    private void SetInterective()
    {
        _cursorManager.OnPointerDown += OnMouseDown;
        _cursorManager.OnPointerMove += OnMouseMove;
    }

    private void SetNotInterective()
    {
        _cursorManager.OnPointerDown -= OnMouseDown;
        _cursorManager.OnPointerMove -= OnMouseMove;
    }
}
