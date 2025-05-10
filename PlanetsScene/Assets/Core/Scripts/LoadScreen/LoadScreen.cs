using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game_managers;
using Game_UI;
using Save_screen;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Load_screen
{

}

namespace Load_screen
{
    public interface ILoadScreen
    {

    }

    public class LoadScreen : MonoBehaviour, ILoadScreen, IStartable, IAsyncStartable, IDisposable
    {
        [Inject] private IObjectResolver resolver;
        [Inject] private ILoadManager loadManager;

        [SerializeField] private PulsObj pulsPlanetBg;
        [SerializeField] private LoadBar loadBar;
        [SerializeField] private GameObject orbitsParent;
        [SerializeField] private bool isLoading;

        private List<IPlanetOrbitObserver> planetOrbitObservers;
        private ICrystalBgObserver crystalBgObserver;
        private ILoadBarObserver loadBarObserver;

        public void Start()
        {
            planetOrbitObservers = new List<IPlanetOrbitObserver>();

            foreach (RotatingObj rotatingObj in orbitsParent.GetComponentsInChildren<RotatingObj>())
            {
                IPlanetOrbitObserver newObserver;
                newObserver = new PlanetOrbitObserver(rotatingObj);

                newObserver.Enable();

                planetOrbitObservers.Add(newObserver);
            }

            //Подключаем анимацию кристала
            crystalBgObserver = new CrystalObserver(pulsPlanetBg);
            crystalBgObserver.Enable();

            //Подключаем загрузку
            if (isLoading)
            {
                loadBarObserver = new LoadBarObserver(loadBar);
                resolver.Inject(loadBarObserver);
                loadBarObserver.Enable();
            }
        }

        public async UniTask StartAsync(CancellationToken cancellation = default)
        {   
            await loadManager.LoadSceneAsync(SceneType.Menu);
        }

        public void Dispose()
        {
            loadBarObserver.Disable();
        }

        public void Initialize() { }
    }
}


