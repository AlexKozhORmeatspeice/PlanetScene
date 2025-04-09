using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Space_Screen;
using Random = UnityEngine.Random;

namespace Planet_Window
{
    public interface IPointsSpawner
    {
        Vector3 GetRandomPositionInCircle();
    }

    public class PointsSpawner : MonoBehaviour, IPointsSpawner, IStartable, IDisposable
    {
        [Inject] private IObjectResolver objectResolver;
        [Inject] private IPointOfInterestFactory pointFactory;
        [Inject] private IPlanetWindow planetWindow;
        [Inject] private ITravelManager travelManager;
        [SerializeField] private RectTransform _rectTransform;

        private Dictionary<IPlanet, List<IPOIObserver>> pointsByPlanet;
        private Dictionary<IPlanet, bool> setStatusByPlanet;

        private IPlanet nowPlanet;

        public void Start()
        {
            //
        }

        public void Initialize()
        {
            pointsByPlanet = new Dictionary<IPlanet, List<IPOIObserver>>();
            setStatusByPlanet = new Dictionary<IPlanet, bool>();

            travelManager.onTravelToPlanet += SetPlanetPOIs;
        }

        public void Dispose()
        {
            foreach (var planet in pointsByPlanet.Keys)
            {
                foreach(var point in pointsByPlanet[planet])
                {
                    point.Disable();
                }
            }

            pointsByPlanet.Clear();
            setStatusByPlanet.Clear();

            travelManager.onTravelToPlanet -= SetPlanetPOIs;
        }

        public void SetPlanetPOIs(IPlanet planet)
        {
            nowPlanet = planet;

            if (pointsByPlanet.ContainsKey(nowPlanet))
                return;

            pointsByPlanet[nowPlanet] = new List<IPOIObserver>();

            foreach (PointInfo info in nowPlanet.PointInfos)
            {
                PointInfo newInfo = info;
                newInfo.planet = nowPlanet;

                IPointOfInterest point = pointFactory.Create(newInfo, gameObject.transform);
                IPOIObserver observer = new POIObserver(point, this);
                objectResolver.Inject(observer);
                observer.Enable();
                
                pointsByPlanet[nowPlanet].Add(observer);
            }

            setStatusByPlanet[nowPlanet] = false;
        }

        private Vector3 GetRandomPositionInBox()
        {
            float posX = Random.Range(_rectTransform.anchorMin.x, _rectTransform.anchorMax.x);
            float posY = Random.Range(_rectTransform.anchorMin.y, _rectTransform.anchorMax.y);

            return new Vector3(posX, posY, 0);
        }
        public Vector3 GetRandomPositionInCircle()
        {
            Vector3 pivot = _rectTransform.position;

            float radius = _rectTransform.sizeDelta.magnitude - 1.0f;
            Vector3 randomDir = Random.insideUnitCircle.normalized * Random.Range(0.0f, radius);

            return pivot + randomDir;
        }
    }
}
