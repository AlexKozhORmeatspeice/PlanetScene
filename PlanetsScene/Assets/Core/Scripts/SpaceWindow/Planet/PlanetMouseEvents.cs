using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;
using VContainer.Unity;

public interface IPlanetMouseEvents
{
    event Action<IPlanet> OnMouseEnter;
    event Action<IPlanet> OnMouseMove;
    event Action<IPlanet> OnMouseLeave;
    event Action<IPlanet> OnMouseClick;
    event Action<IPlanet> OnMouseDoubleClick;
}

public class PlanetMouseEvents : MonoBehaviour, IPlanetMouseEvents, ITickable, IStartable, IDisposable
{
    [Inject] private IPointerManager pointer;
    private bool isEntered;
    private IPlanet nowPlanet;

    public event Action<IPlanet> OnMouseEnter;
    public event Action<IPlanet> OnMouseLeave;
    public event Action<IPlanet> OnMouseClick;
    public event Action<IPlanet> OnMouseDoubleClick;
    public event Action<IPlanet> OnMouseMove;

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

        if (results.Count > 0 && results[0].gameObject.GetComponent<IPlanet>() != null)
        {
            IPlanet newPlanet = results[0].gameObject.GetComponent<IPlanet>();
            if (newPlanet == null)
                return;

            if (nowPlanet == null && !isEntered)
            {
                nowPlanet = newPlanet;
                isEntered = true;

                OnMouseEnter?.Invoke(nowPlanet);
            }

            OnMouseMove?.Invoke(nowPlanet);
        }
        else if(isEntered)
        {
            isEntered = false;
            nowPlanet = null;
            OnMouseLeave?.Invoke(nowPlanet);
        }
    }
    public void Dispose()
    {
        pointer.OnDoubleTap -= InvokeDoubleClick;
        pointer.OnPointerDown -= InvokeClick;
    }

    private void InvokeClick()
    {
        if (nowPlanet == null)
            return;

        OnMouseClick?.Invoke(nowPlanet);
    }
    private void InvokeDoubleClick()
    {
        if (nowPlanet == null)
            return;

        OnMouseDoubleClick?.Invoke(nowPlanet);
    }

    public void Start()
    {
        //
    }
}
