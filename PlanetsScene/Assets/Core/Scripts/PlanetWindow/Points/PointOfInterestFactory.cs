using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
public enum PointActionType
{
    None
}

[Serializable]
public struct PointInfo
{
    public string nameOfPoint;
    public string desc;
    public PointActionType type;
    public Texture icon;
    public bool isVisible;
    public IPlanetInfo planet;
}

public interface IPointOfInterestFactory
{
    IPointOfInterest CreateBasicPoint();

    IPointOfInterest Create(PointInfo pointInfo);
    IPointOfInterest Create(PointActionType type);
    IPointOfInterest Create(PointInfo pointInfo, Transform parent);
}


public class PointOfInterestFactory : IPointOfInterestFactory
{
    [Inject] private PointOfInterest prefab;
    private IObjectResolver objectResolver;

    [Inject]
    public void Construct(IObjectResolver objectResolver)
    {
        this.objectResolver = objectResolver;
    }

    public IPointOfInterest Create(PointInfo pointInfo)
    {
        IPointOfInterest point;
        switch(pointInfo.type)
        {
            default:
                point = CreateBasicPoint();
                break;
        }

        objectResolver.Inject(point);
        point.SetInfo(pointInfo);
        return point;
    }

    public IPointOfInterest CreateBasicPoint()
    {
        IPointOfInterest obj = GameObject.Instantiate(prefab);

        return obj;
    }

    public IPointOfInterest Create(PointActionType type)
    {
        PointInfo pointSettings = new PointInfo();
        pointSettings.nameOfPoint = "None";
        pointSettings.type = type;
        pointSettings.isVisible = false;

        return Create(pointSettings);
    }

    public IPointOfInterest Create(PointInfo pointInfo, Transform parent)
    {
        IPointOfInterest point = Create(pointInfo);
        point.GameObject.transform.SetParent(parent, false);

        return point;
    }

    public IPointOfInterest Create(PointActionType type, IObjectResolver container)
    {
        throw new NotImplementedException();
    }
}
