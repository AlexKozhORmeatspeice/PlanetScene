using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public interface INotFound
{

}

public class NotFound : MonoBehaviour, INotFound, IStartable, IDisposable
{
    private IPlanetWindow_NotFoundObserver observer;
    [SerializeField] private PlanetWindow_NotFound view;

    [Inject]
    public void Construct(IObjectResolver resolver)
    {
        resolver.Inject(observer = new PlanetWindow_NotFoundObserver(view));
    }
    public void Initialize()
    {
        observer.Enable();
    }

    public void Dispose()
    {
        observer.Disable();
    }


    public void Start() { }
}
