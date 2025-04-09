using System;
using System.Collections;
using System.Collections.Generic;
using Space_Screen;
using UnityEngine;
using VContainer;

namespace Planet_Window
{
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
        public IPlanet planet;
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

        public IPointOfInterest Create(PointInfo pointInfo)
        {
            IPointOfInterest point;
            switch (pointInfo.type)
            {
                default:
                    point = CreateBasicPoint();
                    break;
            }

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

}