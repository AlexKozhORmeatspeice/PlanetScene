using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Space_Screen;
using System.Numerics;

namespace Planet_Window
{
    public interface IPlanetWindow
    {
        bool IsEnabled { get; }

        event Action onEnable;
        event Action onDisable;
        event Action<bool> onChangeStatus;
        event Action<IPlanet> onPlanetEnable;
        event Action<IPlanet> onPlanetDisable;

        void Enable(IPlanet planet);
        void Enable();
        void Disable();
    }

    public class PlanetWindow : MonoBehaviour, IPlanetWindow, IStartable, IDisposable
    {
        [Inject] private IScaner scaner;
        [Inject] private ITravelManager travelManager;

        private IPlanetWindowObserver observer;
        private IScanerButtonPresenter scanerButtonPresenter;

        [SerializeField] private PlanetWindowView planetWindowView;
        [SerializeField] private PlanetWindow_ScanerButton scanerButton;

        private IPlanet showPlanet;

        public bool IsEnabled => gameObject.activeSelf;

        public event Action onEnable;
        public event Action onDisable;
        public event Action<bool> onChangeStatus;
        public event Action<IPlanet> onPlanetEnable;
        public event Action<IPlanet> onPlanetDisable;

        [Inject]
        public void Construct(IObjectResolver resolver)
        {
            resolver.Inject(scanerButtonPresenter = new ScanerButtonPresenter(scanerButton, scaner));
            resolver.Inject(observer = new PlanetWindowObserver(planetWindowView, scaner, travelManager));
        }

        public void Initialize()
        {
            travelManager.onLandToPlanet += Enable;
        }

        public void Dispose()
        {
            Disable();
            travelManager.onLandToPlanet -= Enable;
        }


        void IStartable.Start() { }

        public void Enable(IPlanet planet)
        {
            showPlanet = planet;

            observer.Enable(planet);
            scanerButtonPresenter.Enable();

            onEnable?.Invoke();
            onPlanetEnable?.Invoke(planet);

            onChangeStatus?.Invoke(true);
            gameObject.SetActive(true);
        }

        public void Enable()
        {
            if (showPlanet == null)
                return;

            Enable(showPlanet);
        }

        public void Disable()
        {
            observer.Disable();
            scanerButtonPresenter.Disable();

            onDisable?.Invoke();
            onPlanetDisable?.Invoke(showPlanet);
            onChangeStatus?.Invoke(false);

            scaner.SetStatus(false);
        }
    }

}
