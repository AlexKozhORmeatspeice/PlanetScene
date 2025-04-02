using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public interface IToolTip
{
    void Enable(string title = "", string lable = "", string text = "");
    void Enable();
    void Disable();
}

public class MouseMoveToolTip : MonoBehaviour, IToolTip, ITickable, IDisposable
{
    [SerializeField] private ToolTipView toolTipView;
    [Inject] private IPointerManager pointer;

    protected IToolTipObserver toolTipPresenter;

    [Inject]
    public void Construct(IObjectResolver resolver)
    {
        resolver.Inject(toolTipPresenter = new ToolTipObserver(toolTipView));

        Disable();
    }

    public void Dispose()
    {
        Disable();
    }
    public virtual void Tick()
    {
        if (this == null)
            return;

        gameObject.transform.position = pointer.NowWorldPosition;
    }

    public void Enable(string title, string lable, string text)
    {
        toolTipPresenter.Enable(title, lable, text);
    }

    public void Enable()
    {
        toolTipPresenter.Enable();
    }
    public void Disable()
    {
        toolTipPresenter.Disable();
    }
}
