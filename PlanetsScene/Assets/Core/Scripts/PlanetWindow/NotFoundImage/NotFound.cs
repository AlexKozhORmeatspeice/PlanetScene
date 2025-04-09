using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Planet_Window
{
    public interface INotFound
    {

    }

    public class NotFound : MonoBehaviour, INotFound, IStartable, IDisposable
    {
        private INotFoundObserver observer;
        [SerializeField] private PlanetWindow_NotFound view;

        [Inject]
        public void Construct(IObjectResolver resolver)
        {
            resolver.Inject(observer = new NotFoundObserver(view));
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

}