using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;
using VContainer.Unity;

using Random = UnityEngine.Random;

public interface IPointsSpawner
{
    event Action<List<IPointOfInterest>> onUpdatePOIList;
    Dictionary<IPlanet, List<IPointOfInterest>> PointsByPlanet { get; }
}

public class PointsSpawner : MonoBehaviour, IPointsSpawner, IStartable, IDisposable
{
    [Inject] private IPointOfInterestFactory pointFactory;
    [Inject] private IPlanetWindow planetWindow;
    [Inject] private ITravelManager travelManager;
    [SerializeField] private RectTransform _rectTransform;

    private Dictionary<IPlanet, List<IPointOfInterest>> pointsByPlanet;
    private Dictionary<IPlanet, bool> setStatusByPlanet;

    private IPlanet nowPlanet;

    public event Action<List<IPointOfInterest>> onUpdatePOIList;

    public Dictionary<IPlanet, List<IPointOfInterest>> PointsByPlanet => pointsByPlanet;

    public void Start()
    {
        //
    }

    public void Initialize()
    {
        pointsByPlanet = new Dictionary<IPlanet, List<IPointOfInterest>>();
        setStatusByPlanet = new Dictionary<IPlanet, bool>();

        planetWindow.onEnable += SetPlanet;
        planetWindow.onEnable += SpawnPoints;
        planetWindow.onEnable += ShowPoints;
        planetWindow.onDisable += HidePoints;
    }

    public void Dispose()
    {
        pointsByPlanet.Clear();
        setStatusByPlanet.Clear();

        planetWindow.onEnable -= SetPlanet;
        planetWindow.onEnable -= SpawnPoints;
        planetWindow.onEnable -= ShowPoints;
        planetWindow.onDisable -= HidePoints;
    }

    public void SetPlanet()
    {
        nowPlanet = travelManager.nowPlanet;

        if (pointsByPlanet.ContainsKey(nowPlanet))
            return;

        pointsByPlanet[nowPlanet] = new List<IPointOfInterest>();

        foreach (PointInfo info in nowPlanet.PointInfos)
        {
            PointInfo newInfo = info;
            newInfo.planet = nowPlanet;

            IPointOfInterest point = pointFactory.Create(newInfo, gameObject.transform);

            pointsByPlanet[nowPlanet].Add(point);
        }

        setStatusByPlanet[nowPlanet] = false;

        onUpdatePOIList?.Invoke(pointsByPlanet[nowPlanet]);
    }

    private void SpawnPoints() //надо будет доделать, чтобы точки спаунились случайно, но не близко к друг другу
    {
        if (nowPlanet == null || setStatusByPlanet[nowPlanet])
            return;

        foreach(IPointOfInterest point in pointsByPlanet[nowPlanet])
        {
            point.SetPosition(GetRandomPositionInCircle());
        }

        setStatusByPlanet[nowPlanet] = true;
    }

    private Vector3 GetRandomPositionInBox()
    {
        float posX = Random.Range(_rectTransform.anchorMin.x, _rectTransform.anchorMax.x);
        float posY = Random.Range(_rectTransform.anchorMin.y, _rectTransform.anchorMax.y);

        return new Vector3(posX, posY, 0);
    }
    private Vector3 GetRandomPositionInCircle()
    {
        Vector3 pivot = _rectTransform.position;

        float radius = _rectTransform.sizeDelta.magnitude - 1.0f;
        Vector3 randomDir = Random.insideUnitCircle.normalized * Random.Range(0.0f, radius);
        
        return pivot + randomDir;
    }


    private void ShowPoints()
    {
        foreach(IPointOfInterest point in pointsByPlanet[nowPlanet])
        {
            point.SetSpriteVisability(point.IsVisible);
            point.SetInterective(point.IsVisible);
            point.SetScanerDetection(true);
        }
    }

    private void HidePoints()
    {
        foreach (IPointOfInterest point in pointsByPlanet[nowPlanet])
        {
            point.SetSpriteVisability(false);
            point.SetInterective(false);
            point.SetScanerDetection(false);
        }
    }

}
