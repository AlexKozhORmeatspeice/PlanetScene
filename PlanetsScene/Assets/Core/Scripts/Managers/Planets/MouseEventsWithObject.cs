using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;
using VContainer.Unity;

public interface IMouseEventsWithObject<T>
{
    event Action<T> OnMouseEnter;
    event Action<T> OnMouseMove;
    event Action<T> OnMouseLeave;
    event Action<T> OnMouseClick;
    event Action<T> OnMouseDoubleClick;
}

public class MouseEventsWithObject<T> : MonoBehaviour, IMouseEventsWithObject<T>, ITickable, IStartable, IDisposable
{
    [Inject] private IPointerManager pointer;
    private bool isEntered;
    private T nowObj;

    public event Action<T> OnMouseEnter;
    public event Action<T> OnMouseLeave;
    public event Action<T> OnMouseClick;
    public event Action<T> OnMouseDoubleClick;
    public event Action<T> OnMouseMove;

    public void Initialize()
    {
        isEntered = false;
        pointer.OnDoubleTap += InvokeDoubleClick;
        pointer.OnPointerDown += InvokeClick;
    }

    public void Tick()
    {
        var eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        if (results.Count > 0 && results[0].gameObject.GetComponent<T>() != null)
        {
            T newPlanet = results[0].gameObject.GetComponent<T>();
            if (newPlanet == null)
                return;

            if (nowObj == null && !isEntered)
            {
                nowObj = newPlanet;
                isEntered = true;

                OnMouseEnter?.Invoke(nowObj);
            }

            OnMouseMove?.Invoke(nowObj);
        }
        else if(isEntered)
        {
            isEntered = false;
            nowObj = default;
            OnMouseLeave?.Invoke(nowObj);
        }
    }
    public void Dispose()
    {
        pointer.OnDoubleTap -= InvokeDoubleClick;
        pointer.OnPointerDown -= InvokeClick;
    }

    private void InvokeClick()
    {
        if (nowObj == null)
            return;

        OnMouseClick?.Invoke(nowObj);
    }
    private void InvokeDoubleClick()
    {
        if (nowObj == null)
            return;

        OnMouseDoubleClick?.Invoke(nowObj);
    }

    public void Start()
    {
        //
    }
}
