using System;
using UnityEngine;
using VContainer;

public interface IGradientObserver
{
    event Action onEnterObj;
    event Action onInObj;
    event Action onExitObj;

    bool IsInObject { get; }

    void Enable();
    void Disable();

    void SetInterective();
    void SetNotInterective();
}

public class MouseMoveGradientObserver : IGradientObserver
{
    [Inject] private IPointerManager _cursorManager;
    
    private IScreenObject screenObj;
    private IGradientChangable view;

    private bool isInObject;

    public event Action onEnterObj;
    public event Action onInObj;
    public event Action onExitObj;

    private bool isMouseEntered;

    public bool IsInObject => isInObject;

    public MouseMoveGradientObserver(IGradientChangable view, IScreenObject screenObj)
    {
        this.view = view;
        this.screenObj = screenObj;

        isMouseEntered = false;
    }

    public virtual void Enable()
    {
        view.Name = screenObj.Name;
        view.SetVisibility(0.0f);
    }

    public virtual void Disable()
    {
        SetNotInterective();
    }

    private void ChangeScale()
    {
        float outerPlanetRadius = GetOuterRadius();
        float dist = GetMouseToObjDist();
        float alpha = 1.0f - (dist - outerPlanetRadius * 1.1f) / (outerPlanetRadius * 0.5f); //немного математики, чтобы размер не уменьшался, если мы в внутреннем радиусе

        view.SetVisibility(alpha);
    }

    public void SetInterective()
    {
        _cursorManager.OnPointerMove += ChangeScale;
        _cursorManager.OnPointerMove += CheckMouseInStatus;
    }

    public void SetNotInterective()
    {
        view.SetVisibility(0.0f);
        
        isInObject = false;

        _cursorManager.OnPointerMove -= ChangeScale;
        _cursorManager.OnPointerMove -= CheckMouseInStatus;
    }

    private void CheckMouseInStatus()
    {
        isInObject = IsMouseInObject();
        
        if (!isMouseEntered && isInObject)
        {
            isMouseEntered = true;
            onEnterObj?.Invoke();
        }
        else if (isMouseEntered && !isInObject)
        {
            isMouseEntered = false;
            onExitObj?.Invoke();
        }

        if (isInObject)
        {
            onInObj?.Invoke();
        }        
    }

    private bool IsMouseInObject()
    {
        float outerPlanetRadius = GetOuterRadius();
        float dist = GetMouseToObjDist();

        return dist <= outerPlanetRadius;
    }

    private float GetMouseToObjDist()
    {
        Vector3 objPos = (Vector2)(screenObj.Position);
        Vector3 cursorPos = _cursorManager.NowWorldPosition;

        return Mathf.Max(Mathf.Abs(objPos.x - cursorPos.x), Mathf.Abs(objPos.y - cursorPos.y));
    }

    private float GetOuterRadius()
    {
        return Mathf.Max(screenObj.Size.x, screenObj.Size.x);
    }
}
