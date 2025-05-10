using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game_camera;
using Game_managers;
using Game_UI;
using Menu;
using Save_screen;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Load_screen
{

}

namespace Load_screen
{
    public interface IMenuLoadScreen
    {
        Vector3 EndCameraPos { get; }
        float EndCameraSize { get; }
    }

    public class MenuLoadScreen : MonoBehaviour, IMenuLoadScreen, IStartable
    {
        [Inject] private IObjectResolver resolver;

        [SerializeField] private PulsObj pulsPlanetBg;
        [SerializeField] private GameObject orbitsParent;

        [Header("Cam settings")]
        [SerializeField] private Vector3 camPosition;
        [SerializeField] private float camSize;

        private List<IPlanetOrbitObserver> planetOrbitObservers;
        private ICrystalBgObserver crystalBgObserver;
        private IMenuCameraObserver menuCameraObserver;

        public Vector3 EndCameraPos => camPosition;

        public float EndCameraSize => camSize;

        public void Awake()
        {
            planetOrbitObservers = new List<IPlanetOrbitObserver>();

            foreach (RotatingObj rotatingObj in orbitsParent.GetComponentsInChildren<RotatingObj>())
            {
                IPlanetOrbitObserver newObserver;
                newObserver = new PlanetOrbitObserver(rotatingObj);

                newObserver.Enable();

                planetOrbitObservers.Add(newObserver);
            }

            //Подключаем анимацию камеры
            resolver.Inject(menuCameraObserver = new MenuCameraObserver(this));
            menuCameraObserver.Enable();
        }

        public void Initialize()
        {

        }

        public void Start()
        {
            crystalBgObserver = new CrystalObserver(pulsPlanetBg);
            crystalBgObserver.Enable();
        }
    }
}


