using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public interface INotFound
{
    void Enable(Vector3 pos);
}

public class NotFound : MonoBehaviour, INotFound
{
    private IPlanetWindow_NotFoundObserver observer;
    [SerializeField] private PlanetWindow_NotFound view;

    [Inject]
    public void Construct(IObjectResolver resolver)
    {
        resolver.Inject(observer = new PlanetWindow_NotFoundObserver(view));
    }

    public void Enable(Vector3 pos)
    {
        observer.Enable(pos);
    }
}
