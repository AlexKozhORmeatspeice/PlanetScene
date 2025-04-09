using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Planet_Window
{
    public interface IDrone
    {
        event Action<Vector3, IPointOfInterest> onLand;
        void Land(Vector3 pos, IPointOfInterest poi);
    }
    public class Drone : MonoBehaviour, IDrone, IStartable, IDisposable
    {
        private IPlanetWindow_DronePresenter presenter;

        [SerializeField] private PlanetWindow_Drone view;

        [SerializeField] private Transform startPoint;
        [SerializeField] private Transform middlePoint;

        public event Action onStartLand;
        public event Action<Vector3, IPointOfInterest> onLand;

        private Vector3 landPoint;
        private IPointOfInterest foundPOI;

        [Inject]
        public void Construct(IObjectResolver resolver)
        {
            resolver.Inject(presenter = new PlanetWindow_DronePresenter(view));
            view.Init(presenter);
        }
        public void Initialize()
        {
            presenter.OnAnimDone += Land;
        }

        public void Dispose()
        {
            presenter.OnAnimDone -= Land;
        }


        public void Land(Vector3 pos, IPointOfInterest poi)
        {
            foundPOI = poi;
            landPoint = pos;

            onStartLand?.Invoke();


            presenter.Enable(GetTrajectory(landPoint));
        }


        private List<Vector3> GetTrajectory(Vector3 endPos)
        {
            List<Vector3> trajectory = new List<Vector3>();

            trajectory.Add(startPoint.position);
            trajectory.Add(middlePoint.position);
            trajectory.Add(endPos);

            return trajectory;
        }

        private void Land()
        {
            onLand?.Invoke(landPoint, foundPOI);
        }

        public void Start() { }
    }

}